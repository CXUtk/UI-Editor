using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using UIEditor.UILib.Components.Advanced;

namespace UIEditor.UILib.Components.Composite {
    public class UITreeList : UIList {
        public float LayerPaddingLeft { get; set; }
        private List<UITreeNode> _roots;

        public UITreeList() : base() {
            Name = "树状列表";
            LayerPaddingLeft = 20f;
            _roots = new List<UITreeNode>();
        }
        public override void AddElement(UIElement treeNode) {
            if (!(treeNode is UITreeNode)) throw new Exception("树状列表必须传入UITreeNode对象");
            var root = (UITreeNode)treeNode;
            _roots.Add(root);
            _assignEvent(root);
        }

        private void _assignEvent(UITreeNode treenode) {
            treenode.DisplayElement.OnMouseDown += DisplayElement_OnMouseDown;
            foreach (var child in treenode.TreeNodes) {
                _assignEvent(child);
            }
        }

        private void DisplayElement_OnMouseDown(Events.UIMouseEvent e, UIElement sender) {
            _selectedItem = sender;
        }

        public void ClearRoots() { _roots.Clear(); Clear(); }

        private void _addElement(UIElement element) {
            base.AddElement(element);
            ShouldRecalculate = true;
        }

        private float _maxLeftPadding;
        private void _dfsCalculate(UITreeNode node, float leftPadding, GameTime gameTime) {
            _maxLeftPadding = Math.Max(_maxLeftPadding, leftPadding);
            node.DisplayElement.LeftOffset = leftPadding;
            node.DisplayElement.Position = new Vector2(0, _totHeight);
            node.DisplayElement.Selected = (node.DisplayElement == _selectedItem);
            node.DisplayElement.Update(gameTime);
            _addElement(node.DisplayElement);
            _totHeight += node.DisplayElement.Height + ItemMargin;
            node.CanFold = node.TreeNodes.Count != 0;
            if (node.DisplayElement.IsFolded) return;
            foreach (var child in node.TreeNodes) {
                _dfsCalculate(child, leftPadding + LayerPaddingLeft, gameTime);
            }
        }

        public override void UpdateElementPos(GameTime gameTime) {
            Clear();
            _totHeight = InnerContainerPadding;
            _maxLeftPadding = 0;
            _maxWidth = 0;
            foreach (var root in _roots)
                _dfsCalculate(root, 0, gameTime);
            foreach (var element in _elements) {
                element.Size = new Vector2(_maxLeftPadding, element.Size.Y);
                _maxWidth = Math.Max(_maxWidth, element.Width);
            }
            CalculateViewPortScrollRelated();
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

        }
    }
}
