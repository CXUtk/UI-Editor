﻿using Microsoft.Xna.Framework;
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
    public class Inspecter : UIElement {
        private UIPanel _inspecterPanel;
        private UIList _inspectorList;
        private EditorState _editor;
        public Inspecter(EditorState editor) : base() {
            _editor = editor;
            _editor.OnSelectionChange += _editor_OnSelectionChange;
            _inspecterPanel = new UIPanel() {
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                Position = new Vector2(5, 5),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-10, -9),
            };
            _inspectorList = new UIList() {
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
            _inspectorList.SetScrollBarV(scrollBar1);
            _inspecterPanel.AppendChild(_inspectorList);
            AppendChild(_inspecterPanel);
        }

        private UIElement _currentFocus;

        private void _editor_OnSelectionChange(UIActionEvent e, UIElement sender) {
            if (e.Target != null) {
                Add(((BrowserTreeNode)e.Target).BindingElement);
                _currentFocus = ((BrowserTreeNode)e.Target).BindingElement;
            } else {
                _currentFocus = null;
            }
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (_currentFocus != null && _currentFocus.ShouldRecalculate) {
                Add(_currentFocus);
            }
        }

        private UIElement GetRightElement(PropertyInfo info, UIElement element) {
            object value = info.GetValue(element);
            if (info.PropertyType == typeof(bool)) {
                var check = new UICheckBox() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    Checked = (bool)value,
                };
                check.OnCheckedChange += (e, s) => {
                    info.SetValue(element, e.Value);
                };
                return check;
            } else if (info.PropertyType == typeof(string)) {
                var changer = new UITextBox() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                    Text = value.ToString(),
                };
                changer.OnTextChange += (e, s) => {
                    info.SetValue(element, e.NewString);
                };
                return changer;
            } else if (info.PropertyType == typeof(Color)) {
                var color = new UIColorIdentifier() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                    Color = (Color)value,
                };
                return color;
            } else if (info.PropertyType == typeof(Vector2)) {
                var vector2 = new UIVector2((Vector2)value) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                };
                vector2.OnValueChanged += (e, s) => {
                    info.SetValue(element, new Vector2(vector2.X, vector2.Y));
                };
                return vector2;
            }
            var text = new UILabel() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0f),
                SizeFactor = new Vector2(1, 1),
                Text = (value == null) ? "null" : value.ToString(),
            };
            return text;
        }


        public void UpdateProperties() {

        }
        public void Add(UIElement element) {
            _inspectorList.Clear();
            foreach (var info in element.GetType().GetProperties()) {
                if (info.IsDefined(typeof(Attributes.EditorPropertyIgnoreAttribute), true))
                    continue;
                UIElement right = GetRightElement(info, element);
                var left = new UILabel() {
                    Text = info.Name,
                    Size = new Vector2(-10, 20),
                    SizeStyle = SizeStyle.Inline,
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    Position = new Vector2(5, 0),
                };
                var item = new UITableBar(left, right) {
                    SizeFactor = new Vector2(1, 0),
                    Size = new Vector2(0, 30),
                };
                _inspectorList.AddElement(item);

            }

        }
    }

}
