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
using System.Linq;

namespace UIEditor.Editor.States {
    public class Browser : UIElement {
        public UIElement SelectedElement { get { return _treeList.SelectedElement; } }
        private UIPanel _listPanel;
        private UITreeList _treeList;
        private UIList _toolBarList;
        private EditorState _editor;
        public Browser(EditorState editor) : base() {
            _editor = editor;
            _editor.OnSelectionChange += _editor_OnSelectionChange;
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
                PropagationRule = UILib.Enums.PropagationFlags.BLOCK_ALL,
                ButtonTooltip = "工具栏",
            };
            AppendChild(_listPanel);
            AppendChild(toolbar2);
            _listPanel.AppendChild(_treeList);
            _treeList.SetScrollBarV(scrollBar1);
            _treeList.SetScrollBarH(scrollBar2);
            _treeList.OnSelect += _treeList_OnSelect;

            _toolBarList = new UISelectableList() {
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

        private void _editor_OnSelectionChange(UIActionEvent e, UIElement sender) {
            _treeList.SelectedElement = e.Target;
        }

        private void _treeList_OnSelect(UIActionEvent e, UIElement sender) {
            var list = (UITreeList)sender;
            var target = (BrowserTreeNode)list.SelectedElement;
            _editor.Viewer.Canvas.PlaceSizer(target.BindingElement);
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
            var pointer = new ToolBarButton() {
                Text = $"指针",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("NoTexture"),
            };
            pointer.OnClick += Pointer_OnClick;
            _toolBarList.AddElement(pointer);
            var button = new ToolBarButton() {
                Text = $"按钮",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("Icon_Button"),
            };
            button.OnClick += Button_OnClick;
            var label = new ToolBarButton() {
                Text = $"标签文本",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("Icon_Label"),
            };
            label.OnClick += Label_OnClick;
            _toolBarList.AddElement(label);
            var textBox = new ToolBarButton() {
                Text = $"文本框",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("Icon_Textbox"),
            };
            textBox.OnClick += TextBox_OnClick;
            _toolBarList.AddElement(textBox);
            var progressBar = new ToolBarButton() {
                Text = $"进度条",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("Icon_ProgressBar"),
            };
            progressBar.OnClick += ProgressBar_OnClick;
            _toolBarList.AddElement(progressBar);
        }

        private void ProgressBar_OnClick(UIMouseEvent e, UIElement sender) {
            _editor.SetPlaceMode(new UIProgressBar() {
                Size = new Vector2(60, 20),
                IsPreview = true,
            });
        }

        private void TextBox_OnClick(UIMouseEvent e, UIElement sender) {
            _editor.SetPlaceMode(new UITextBox() {
                Size = new Vector2(50, 30),
                IsPreview = true,
            });
        }

        private void Label_OnClick(UIMouseEvent e, UIElement sender) {
            _editor.SetPlaceMode(new UILabel() {
                Size = new Vector2(50, 30),
                IsPreview = true,
            });
        }

        private void Button_OnClick(UIMouseEvent e, UIElement sender) {
            _editor.SetPlaceMode(new UIButton() {
                Size = new Vector2(100, 50),
                IsPreview = true,
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
                foreach (var child in e.Children) {
                    _treeList.AddElement(_copy(child));
                }

            }
        }
        public UIElement FindTreeElement(UIElement element) {
            return _treeList.Elements.FirstOrDefault((e) => {
                var a = ((BrowserTreeNode)e);
                return a.BindingElement == element;
            });
        }

    }
}
