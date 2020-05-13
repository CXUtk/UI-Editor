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
using UIEditor.UILib.Components.Interface;

namespace UIEditor.Editor.States {
    public class Inspecter : UIEditorPart {
        private UIPanel _inspecterPanel;
        private UIList _inspectorList;
        private EditorState _editor;
        public Inspecter(EditorState editor) : base() {
            _editor = editor;

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
                if (e.Target != _currentFocus)
                    Add(e.Target);
                _currentFocus = e.Target;
            } else {
                _currentFocus = null;
                _inspectorList.Clear();
            }
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }

        private UIElement GetRightElement(PropertyInfo info, UIElement element) {
            object value = info.GetValue(element);
            if (info.PropertyType == typeof(bool)) {
                var check = new UICheckBox() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    Value = (bool)value,
                };
                check.OnCheckedChange += (e, s) => {
                    info.SetValue(element, e.Value);
                    _editor.NotifyElementPropertyChange(this);
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
                    _editor.NotifyElementPropertyChange(this);
                };
                return changer;
            } else if (info.PropertyType == typeof(Color)) {
                var color = new UIColorIdentifier() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                    Color = (Color)value,
                };
                color.OnClick += (e, s) => {
                    _editor.OpenColorChooser((Color)value, info, element);
                };
                return color;
            } else if (info.PropertyType == typeof(Vector2)) {
                var vector2 = new UIVector2(element, info) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                };
                vector2.OnValueChanged += (e, s) => {
                    info.SetValue(element, vector2.Value);
                    _editor.NotifyElementPropertyChange(this);
                };
                return vector2;
            } else if (info.PropertyType == typeof(float)) {
                var textF = new UIValueTextBoxEx<float>(element, info) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                };
                textF.OnValueChanged += (e, s) => {
                    info.SetValue(element, textF.Value);
                    _editor.NotifyElementPropertyChange(this);
                };
                return textF;
            }
            var text = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                Text = (value == null) ? "null" : value.ToString(),
            };
            return text;
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

        public override void Initialize() {
            _editor.OnSelectionChange += _editor_OnSelectionChange;
            _editor.OnSizerAttached += _editor_OnSelectionChange;
            _editor.OnSizerChanged += _editor_OnSizerChanged;
            //_editor.OnPropertyChanged += _editor_OnPropertyChanged;
        }

        //private void _editor_OnPropertyChanged(UIActionEvent e, UIElement sender) {
        //    foreach (var child in _inspectorList.Elements) {
        //        UITableBar uibar = (UITableBar)child;
        //        if (uibar.Right is IUIUpdateable) {
        //            (uibar.Right as IUIUpdateable).UpdateValue();
        //        }
        //    }
        //}

        private void _editor_OnSizerChanged(UIActionEvent e, UIElement sender) {
            foreach (var child in _inspectorList.Elements) {
                UITableBar uibar = (UITableBar)child;
                if (uibar.Right is IUIUpdateable) {
                    (uibar.Right as IUIUpdateable).UpdateValue();
                }
            }
        }
    }

}
