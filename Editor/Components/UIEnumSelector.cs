using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Components.Interface;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.Components {
    public class UIEnumSelector : UIPanel, IUIUpdateable {
        private class EnumValueButton : UIButton {
            public Enum Value { get; }
            public EnumValueButton(Enum enumValue) : base() {
                Value = enumValue;
                Name = enumValue.ToString();
                Text = enumValue.ToString();
                SizeFactor = new Vector2(1, 0f);
                Size = new Vector2(0, 30f);
            }
            public override void UpdateSelf(GameTime gameTime) {
                base.UpdateSelf(gameTime);
                //var size = Parent.Size;
                //size.Y = 18;
                //Size = size;
                //if (IsMouseHover) {
                //    _label.TextColor = Color.Blue;
                //} else {
                //    _label.TextColor = Color.White;
                //}
            }
        }

        private UISelectableList _list;
        private UILabel _label;
        private UIPanel _listPanel;
        private UIImageButton _expandButton;

        private Enum _currentValue;
        public PropertyInfo PropertyInfo { get; }
        public UIElement Target { get; }


        public Enum Value {
            get {
                return _currentValue;
            }
            set {
                _currentValue = value;
                _label.Text = value.ToString();

            }
        }
        /// <summary>
        /// 决定这个元件的隐藏列表元件挂载在哪个节点的子节点下，用于改变隐藏列表的行为
        /// </summary>
        public UIElement HangElement { get; set; }
        public event ActionEvent OnValueChange;

        public UIEnumSelector(UIElement target, PropertyInfo property) : base() {
            _label = new UILabel() {
                Pivot = new Vector2(0f, 0.5f),
                AnchorPoint = new Vector2(0f, 0.5f),
                NoEvent = true,
                SizeStyle = SizeStyle.Block,
                Text = property.GetValue(target).ToString(),
                Position = new Vector2(2f, 0f),
            };
            _expandButton = new UIImageButton() {
                Texture = UIEditor.Instance.SkinManager.GetTexture("Down"),
                WhiteTexture = UIEditor.Instance.SkinManager.GetTexture("Down_Border"),
                Pivot = new Vector2(1, 0.5f),
                AnchorPoint = new Vector2(1, 0.5f),
                SizeStyle = SizeStyle.Inline,
                Position = new Vector2(-1, 0),
            };
            _expandButton.OnClick += _expandButton_OnClick;

            var values = property.PropertyType.GetEnumValues();
            _list = new UISelectableList {
                Name = "Values",
                InnerContainerPadding = 2,
                AnchorPoint = new Vector2(0f, 0),
                Pivot = new Vector2(0f, 0),
                SizeFactor = new Vector2(1f, 1f),
                Position = new Vector2(2, 2),
                Size = new Vector2(-4, -4),
            };
            var scrollBar = new UIScrollBarV() {
                Name = "Enum_ScrollBar",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            _list.SetScrollBarV(scrollBar);
            foreach (var value in values) {
                _list.AddElement(new EnumValueButton((Enum)value));
            }
            _list.OnSelect += List_OnSelect;
            _list.IsActive = true;

            _listPanel = new UIPanel() {
                Size = new Vector2(Width, 200),
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                IsActive = false,
            };
            _listPanel.AppendChild(_list);
            AppendChild(_label);
            // AppendChild(_list);
            AppendChild(_expandButton);
        }

        public override void Recalculate() {
            base.Recalculate();
            if (_listPanel != null && _listPanel.Parent != null)
                _listPanel.Recalculate();
        }

        private void _expandButton_OnClick(UIMouseEvent e, UIElement sender) {
            if (HangElement == null) throw new ArgumentNullException();
            _listPanel.Size = new Vector2(Width, _listPanel.Size.Y);
            HangElement.AppendChild(_listPanel);
            _listPanel.IsActive ^= true;
            _listPanel.Position = _listPanel.ScreenPositionToParentAR(this.InnerRectangleScreen.BottomLeft());
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _label.Size = new Vector2(Width - _expandButton.Width - 5f, _label.Size.Y);
        }

        private void List_OnSelect(UIActionEvent e, UIElement sender) {
            Value = ((EnumValueButton)((UISelectableList)e.Target).SelectedElement).Value;
            OnValueChange?.Invoke(new UIActionEvent(this, e.TimeStamp), this);
            _listPanel.IsActive = false;
        }

        public void UpdateValue() {
            Value = (Enum)PropertyInfo.GetValue(Target);
        }
    }
}
