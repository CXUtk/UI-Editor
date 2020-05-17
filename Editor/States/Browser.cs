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
using UIEditor.UILib.Enums;

namespace UIEditor.Editor.States {
    public class Browser : UIEditorPart {

        public event ActionEvent OnSelectionChange;

        public UIElement SelectedElement { get { return _treeList.SelectedElement; } }
        private UIPanel _listPanel;
        private UIBrowserTreeList _treeList;
        private UIList _toolBarList;
        public Browser(EditorState editor) : base(editor) {
            PropagationRule = PropagationFlags.FocusEvents;
            _listPanel = new UIPanel() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                Size = new Vector2(-15f, -10f),
                Position = new Vector2(15f, 5f),
                PanelTexture = UIEditor.Instance.SkinManager.GetTexture("Box_Default"),
            };
            _treeList = new UIBrowserTreeList(Editor) {
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
                PropagationRule = PropagationFlags.BLOCK_ALL,
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

        private void _treeList_OnSelect(UIActionEvent e, UIElement sender) {
            var list = (UIBrowserTreeList)sender;
            var target = (BrowserTreeDisplayNode)list.SelectedElement;
            OnSelectionChange?.Invoke(new UIActionEvent(target.BindingElement, e.TimeStamp), this);
        }

        private UIBrowserTreeNode _copy(UIElement element) {
            List<UIBrowserTreeNode> children = new List<UIBrowserTreeNode>();
            foreach (var child in element.Children) {
                children.Add(_copy(child));
            }
            return new UIBrowserTreeNode(element.Name, element, children) {
                Pivot = new Vector2(0f, 0f),
                AnchorPoint = new Vector2(0f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0f, 30f),
            };
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
            var empty = new ToolBarButton() {
                Text = $"空节点",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("NoTexture"),
            };
            empty.OnClick += Empty_OnClick;
            _toolBarList.AddElement(empty);
            var button = new ToolBarButton() {
                Text = $"按钮",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("Icon_Button"),
            };
            button.OnClick += Button_OnClick;
            _toolBarList.AddElement(button);
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
            var panelBar = new ToolBarButton() {
                Text = $"面板",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("Icon_Panel"),
            };
            panelBar.OnClick += PanelBar_OnClick;
            _toolBarList.AddElement(panelBar);
            var picture = new ToolBarButton() {
                Text = $"图片",
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(0, 30),
                ButtonTexture = UIEditor.Instance.SkinManager.GetTexture("Icon_Picture"),
            };
            picture.OnClick += Picture_OnClick;
            _toolBarList.AddElement(picture);
        }

        private void Empty_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(new UIElement() {
                Size = new Vector2(50, 50),
                IsPreview = true,
            });
        }

        private void Picture_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(new UIImage() {
                Size = new Vector2(50, 50),
                IsPreview = true,
                SizeStyle = SizeStyle.Block,
                Texture = Main.magicPixel,
            });
        }

        private void PanelBar_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(new UIPanel() {
                Size = new Vector2(100, 100),
                IsPreview = true,
            });
        }

        private void ProgressBar_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(new UIProgressBar() {
                Size = new Vector2(60, 20),
                IsPreview = true,
            });
        }

        private void TextBox_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(new UITextBox() {
                Size = new Vector2(50, 30),
                IsPreview = true,
            });
        }

        private void Label_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(new UILabel() {
                Size = new Vector2(50, 30),
                IsPreview = true,
            });
        }

        private void Button_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(new UIButton() {
                Size = new Vector2(100, 50),
                IsPreview = true,
            });
        }

        private void Pointer_OnClick(UIMouseEvent e, UIElement sender) {
            Editor.SetPlaceMode(null);
        }

        public void AddNode(UIElement element) {
            _treeList.AddElement(_copy(element));
        }

        public void Refresh() {
            var e = Editor.Viewer.Canvas.Root;
            _treeList.ClearRoots();
            if (e != null) {
                foreach (var child in e.Children) {
                    _treeList.AddElement(_copy(child));
                }
            }
        }
        public UIElement FindTreeElement(UIElement element) {
            return _treeList.Elements.FirstOrDefault((e) => {
                var a = ((BrowserTreeDisplayNode)e);
                return a.BindingElement == element;
            });
        }

        public override void Initialize() {
            Refresh();
            Editor.OnPlaceElement += _editor_OnPlaceElement;
            Editor.OnSizerAttached += _editor_OnSizerChanged;
        }

        private void _editor_OnSizerChanged(UIActionEvent e, UIElement sender) {
            _treeList.SelectedElement = FindTreeElement(e.Target);
        }

        private void _editor_OnPlaceElement(UIActionEvent e, UIElement sender) {
            AddNode(e.Target);
        }
    }
}
