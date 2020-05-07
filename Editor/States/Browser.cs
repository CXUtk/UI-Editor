using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using System.Reflection;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Events;
using Microsoft.Xna.Framework.Graphics;
using UIEditor.Editor.Components;

namespace UIEditor.Editor.States {
    public class Browser : UIElement {
        public UIElement SelectedElement { get { return _treeList.SelectedElement; } }
        private UIPanel _listPanel;
        private UITreeList _treeList;
        private UIList _toolBarList;
        private EditorState _editor;
        public Browser(EditorState editor) : base() {
            _editor = editor;
            _listPanel = new UIPanel() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                Size = new Vector2(-15f, -10f),
                Position = new Vector2(15f, 5f),
                PanelTexture = UIEditor.Instance.SkinManager.GetTexture("Box_Default"),
            };
            _treeList = new UITreeList() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                Size = new Vector2(-10f, -10f),
                Position = new Vector2(5f, 5f),
            };
            var scrollBar1 = new UIScrollBarV() {
                Name = "ScrollBar",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            var scrollBar2 = new UIScrollBarH() {
                Name = "ScrollBarH",
            };
            var toolbar2 = new UIToolBarV() {
                SizeFactor = new Vector2(1f, 1f),
                AnchorPoint = new Vector2(0f, 0.5f),
                Pivot = new Vector2(0f, 0.5f),
                BlockPropagation = true,
                ButtonTooltip = "工具栏",
            };
            AppendChild(_listPanel);
            AppendChild(toolbar2);
            _listPanel.AppendChild(_treeList);
            _treeList.SetScrollBarV(scrollBar1);
            _treeList.SetScrollBarH(scrollBar2);

            _toolBarList = new UIList() {
                Pivot = new Vector2(0, 0),
                Position = new Vector2(10, 10),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-20, -20),
            };

            SetupButtons();
            var scrollBar3 = new UIScrollBarV() {
                Name = "工具栏滚动条",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            _toolBarList.SetScrollBarV(scrollBar3);
            toolbar2.AddToPanel(_toolBarList);

        }
        private UITreeNode _copy(UIElement element) {
            List<UITreeNode> children = new List<UITreeNode>();
            UITreeNode node = new UITreeNode(new BrowserTreeNode(element.Name, element) {
                Pivot = new Vector2(0f, 0f),
                AnchorPoint = new Vector2(0f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0f, 30f),
            }, children);
            foreach (var child in element.Children) {
                children.Add(_copy(child));
            }
            return node;
        }


        private void SetupButtons() {
            var pointer = new UIButton() {
                Text = $"指针",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
            };
            pointer.OnClick += Pointer_OnClick;
            _toolBarList.AddElement(pointer);
            var button = new UIButton() {
                Text = $"按钮",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
            };
            button.OnClick += Button_OnClick;
            _toolBarList.AddElement(button);
        }

        private void Button_OnClick(UIMouseEvent e, UIElement sender) {
            _editor.SetPlaceMode(new UIButton() {
                Size = new Vector2(50, 50),
            });
        }

        private void Pointer_OnClick(UIMouseEvent e, UIElement sender) {
            _editor.SetPlaceMode(null);
        }

        public void AddNode(UIElement element) {
            _treeList.AddElement(_copy(element));
        }

        public void Refresh() {
            var e = _editor.Viewer.Canvas.Root;
            _treeList.ClearRoots();
            if (e != null) {
                _treeList.AddElement(_copy(e));
            }
        }

    }
}
