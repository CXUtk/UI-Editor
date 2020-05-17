using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Events;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib.Components.Composite;
using Terraria;
using Microsoft.Xna.Framework;
using UIEditor.UILib.Components;
using UIEditor.Editor.States;

namespace UIEditor.Editor.Components {
    public class UIBrowserTreeList : UIList {
        public event ActionEvent OnSelect;
        public float LayerPaddingLeft { get; set; }
        private List<UIBrowserTreeNode> _roots;
        public EditorState Editor { get; }

        public UIBrowserTreeList(EditorState editor) : base() {
            Editor = editor;
            Name = "拖动树状列表";
            LayerPaddingLeft = 20f;
            _roots = new List<UIBrowserTreeNode>();
        }
        public override void AddElement(UIElement treeNode) {
            if (!(treeNode is UIBrowserTreeNode)) throw new Exception("树状列表必须传入UITreeNode对象");
            var root = (UIBrowserTreeNode)treeNode;

            _roots.Add(root);
            _assignEvent(root);
        }
        internal void RemoveTreeRoot(UIBrowserTreeNode node) {
            _roots.Remove(node);
        }

        private void _assignEvent(UIBrowserTreeNode treenode) {
            treenode.Parent = this;
            treenode.DisplayElement.OnMouseDown += DisplayElement_OnMouseDown;
            foreach (var child in treenode.TreeNodes) {
                _assignEvent(child);
            }
        }

        private void DisplayElement_OnMouseDown(UIMouseEvent e, UIElement sender) {
            SelectedElement = sender;
            OnSelect?.Invoke(new UIActionEvent(this, Main._drawInterfaceGameTime.TotalGameTime), this);
        }

        public void ClearRoots() { _roots.Clear(); Clear(); }

        private void _addElement(UIElement element) {
            base.AddElement(element);
            ShouldRecalculate = true;
        }

        private float _maxLeftPadding;
        private void _dfsCalculate(UIBrowserTreeNode node, float leftPadding, GameTime gameTime) {
            _maxLeftPadding = Math.Max(_maxLeftPadding, leftPadding);
            node.DisplayElement.LeftOffset = leftPadding;
            node.DisplayElement.Position = new Vector2(0, _totHeight);
            node.DisplayElement.IsSelected = (node.DisplayElement == SelectedElement);
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

        public void NotifyDragAction(UIBrowserTreeNode src, UIElement dest) {
            Main.NewText(src.ToString() + " " + dest.ToString());
            if (dest is BrowserTreeDisplayNode) {
                var dest2 = (BrowserTreeDisplayNode)dest;
                if (src == dest2.Info) return;
                dest2.Info.AddChildTreeNode(src);
            } else if (dest == _viewPort) {
                src.InfoParent.RemoveChildTreeNode(src);
                src.DisplayElement.BindingElement.Parent = Editor.Viewer.Canvas.Root;
                src.InfoParent = null;
                AddElement(src);
            }
        }
    }
}
