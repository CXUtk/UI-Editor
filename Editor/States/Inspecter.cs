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
using UIEditor.Editor.Helper;
using System.Xml;

namespace UIEditor.Editor.States {
    public class Inspecter : UIEditorPart {
        private UIPanel _inspecterPanel;
        private UIList _inspectorList;
        public Inspecter(EditorState editor) : base(editor) {

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

        private UIElement GetRightElement(PropertyInfo info, UIElement element, ref int maxHeight) {
            object value = info.GetValue(element);
            if (info.PropertyType == typeof(bool)) {
                var check = new UICheckBox() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    Value = (bool)value,
                };
                check.OnCheckedChange += (e, s) => {
                    info.SetValue(element, e.Value);
                    Editor.NotifyElementPropertyChange(this);
                };
                return check;
            } else if (info.PropertyType == typeof(string)) {
                var changer = new UITextBox() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                    Text = value.ToString(),
                    Size = new Vector2(-8, 0),
                };
                if (info.SetMethod == null) {
                    changer.Editable = false;
                }
                changer.OnTextChange += (e, s) => {
                    info.SetValue(element, e.NewString);
                    Editor.NotifyElementPropertyChange(this);
                };
                return changer;
            } else if (info.PropertyType == typeof(Color)) {
                var color = new UIColorIdentifier(info, element) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                    Color = (Color)value,
                    Size = new Vector2(-8, 0),
                };
                color.OnClick += (e, s) => {
                    Editor.OpenColorChooser(info, element, color);
                };
                return color;
            } else if (info.PropertyType == typeof(Vector2)) {
                var vector2 = new UIVector2(element, info) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                };
                if (info.SetMethod == null) {
                    vector2.Editable = false;
                }
                vector2.OnValueChanged += (e, s) => {
                    info.SetValue(element, vector2.Value);
                    Editor.NotifyElementPropertyChange(this);
                };
                return vector2;
            } else if (info.PropertyType == typeof(Rectangle)) {
                var rect = new UIRectangle(element, info) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                };
                if (info.SetMethod == null) {
                    rect.Editable = false;
                }
                rect.OnValueChanged += (e, s) => {
                    info.SetValue(element, rect.Value);
                    Editor.NotifyElementPropertyChange(this);
                };
                maxHeight = 65;
                return rect;
            } else if (info.PropertyType == typeof(float)) {
                var textF = new UIValueTextBoxEx<float>(element, info) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                    Size = new Vector2(-8, 0),
                };
                if (info.SetMethod == null) {
                    textF.Editable = false;
                }
                textF.OnValueChanged += (e, s) => {
                    info.SetValue(element, textF.Value);
                    Editor.NotifyElementPropertyChange(this);
                };
                return textF;
            } else if (info.PropertyType.IsEnum) {
                var instance = new UIEnumSelector(element, info) {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    SizeFactor = new Vector2(1, 1),
                    Size = new Vector2(-8, 0),
                };
                instance.HangElement = this;
                instance.OnValueChange += (e, s) => {
                    info.SetValue(element, instance.Value);
                    Editor.NotifyElementPropertyChange(this);
                };
                return instance;
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
                int height = 30;
                UIElement right = GetRightElement(info, element, ref height);
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
                    Size = new Vector2(0, height),
                };
                XmlNode info2 = null;
                if ((info2 = UIEditor.Instance.Documentation.GetPropertyInfo(info.DeclaringType.FullName + "." + info.Name)) != null) {
                    string desc = info2.InnerText.Trim();
                    item.LeftTooltip = desc;
                    left.Tooltip = desc;
                }
                _inspectorList.AddElement(item);
            }
        }

        public override void Initialize() {
            Editor.OnSelectionChange += _editor_OnSelectionChange;
            Editor.OnSizerAttached += _editor_OnSelectionChange;
            Editor.OnSizerChanged += _editor_OnSizerChanged;
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
