﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using UIEditor.UILib.Components.Advanced;

namespace UIEditor.UILib.Components.Composite {
    public class UITreeList : UIList {
        public float LayerPaddingLeft { get; set; }
        private List<UITreeNode> _roots;
        public UITreeList() : base() {
            LayerPaddingLeft = 20f;
            _roots = new List<UITreeNode>();
        }
        public override void AddElement(UIElement treeNode) {
            if (!(treeNode is UITreeNode)) throw new Exception("树状列表必须传入UITreeNode对象");
            _roots.Add((UITreeNode)treeNode);
        }

        private void _addElement(UIElement element) {
            base.AddElement(element);
        }
        private float _maxLeftPadding;
        private void _dfsCalculate(UITreeNode node, float leftPadding) {
            _maxLeftPadding = Math.Max(_maxLeftPadding, leftPadding);
            node.DisplayElement.Position = new Vector2(leftPadding, _totHeight);
            _addElement(node.DisplayElement);
            _totHeight += node.DisplayElement.Height + ItemMargin;
            if (node.IsFolded) return;
            foreach (var child in node.TreeNodes) {
                _dfsCalculate(child, leftPadding + LayerPaddingLeft);
            }
        }

        public override void UpdateElementPos(GameTime gameTime) {
            Clear();
            _totHeight = InnerContainerPadding;
            _maxLeftPadding = 0;
            foreach (var root in _roots)
                _dfsCalculate(root, 0);
            foreach (var element in _elements)
                element.Size = new Vector2(_maxLeftPadding, element.Size.Y);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

        }
    }
}
