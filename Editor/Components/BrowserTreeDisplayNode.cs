using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib;
using UIEditor.UILib.Events;
using Microsoft.Xna.Framework;
using static UIEditor.UILib.Components.UIDraggable;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace UIEditor.Editor.Components {
    public class BrowserTreeDisplayNode : UITreeNodeTextDisplay {
        public UIElement BindingElement { get; }
        public bool IsDragOver { get; set; }
        public UIBrowserTreeNode Info { get; }
        public event ActionEvent OnDragging;
        private bool _isDragging;
        public BrowserTreeDisplayNode(string text, UIElement element, UIBrowserTreeNode info) : base(text) {
            BindingElement = element;
            Info = info;
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            this.Text = BindingElement.Name;
            if (_isDragging) {
                OnDragging?.Invoke(new UIActionEvent(this, gameTime.TotalGameTime), this);
            }
        }
        public override void DragStart(UIMouseEvent e) {
            _isDragging = true;
            base.DragStart(e);
        }

        public override void DragEnd(UIDragEndEvent e) {
            _isDragging = false;
            Info?.DragEnd(e);
            base.DragEnd(e);
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            // 容器处于交点颜色就变成highlight
            if (IsDragOver && this.Parent.IsFocused) {
                Drawing.DrawAdvBox(sb, 0, 0, Width, Height, UIEditor.Instance.SkinManager.GetColor("Highlight2"), UIEditor.Instance.SkinManager.GetTexture("BoxFrame_Default"),
                    new Vector2(4, 4));
            }


        }
    }

    public class UIBrowserTreeNode : UIElement {
        public bool CanFold { get { return DisplayElement.CanFold; } set { DisplayElement.CanFold = value; } }
        public BrowserTreeDisplayNode DisplayElement { get; }
        public UIBrowserTreeNode InfoParent { get; set; }
        public IList<UIBrowserTreeNode> TreeNodes { get; }
        public event ActionEvent OnDragging;

        public UIBrowserTreeNode(string text, UIElement element, IList<UIBrowserTreeNode> nodes) : base() {
            Pivot = new Vector2(0, 0);
            var display = new BrowserTreeDisplayNode(text, element, this) {
                Pivot = new Vector2(0f, 0f),
                AnchorPoint = new Vector2(0f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0f, 30f),
            };
            DisplayElement = display;
            TreeNodes = nodes;
            foreach (var node in nodes) {
                node.InfoParent = this;
            }
        }

        public void AddChildTreeNode(UIBrowserTreeNode node) {
            if (node.InfoParent != null) {
                node.InfoParent.RemoveChildTreeNode(node);
                node.InfoParent.CanFold = node.InfoParent.TreeNodes.Count > 0;
            } else {
                var tree = (UIBrowserTreeList)Parent;
                tree.RemoveTreeRoot(node);
            }
            node.InfoParent = this;
            DisplayElement.BindingElement.AppendChild(node.DisplayElement.BindingElement);
            TreeNodes.Add(node);
        }
        public void RemoveChildTreeNode(UIBrowserTreeNode node) {
            DisplayElement.BindingElement.RemoveChild(node.DisplayElement.BindingElement);
            TreeNodes.Remove(node);
            CanFold = TreeNodes.Count > 0;
        }
        public override void DragEnd(UIDragEndEvent e) {
            var tree = (UIBrowserTreeList)Parent;
            tree.NotifyDragAction(this, e.DestElement);
            base.DragEnd(e);
        }
    }
}
