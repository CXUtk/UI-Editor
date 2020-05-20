using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using Microsoft.Xna.Framework;
using UIEditor.UILib.Components.Composite;
using Terraria;
using System.Reflection;
using UIEditor.UILib.Components.Interface;

namespace UIEditor.Editor.Components {
    public class UIRectangle : UIElement, IUIUpdateable, IUIValue<Rectangle> {
        public event ValueChangeEvent<float> OnValueChanged;
        private UILabel _xl, _yl, _wl, _hl;
        private UIValueTextBox<float> _textX, _textY, _textW, _textH;
        public bool Editable {
            get { return _textX.Editable && _textY.Editable; }
            set { _textX.Editable = _textY.Editable = _textW.Editable = _textH.Editable = value; }
        }
        public Rectangle Value {
            get {
                return new Rectangle((int)_textX.Value, (int)_textY.Value, (int)_textW.Value, (int)_textH.Value);
            }
            set {
                _textX.Value = value.X;
                _textY.Value = value.Y;
                _textW.Value = value.Width;
                _textH.Value = value.Height;
            }
        }
        public PropertyInfo PropertyInfo { get; }
        public UIElement Target { get; }

        public UIRectangle(UIElement target, PropertyInfo property) : base() {
            PropertyInfo = property;
            Target = target;
            Rectangle rect = (Rectangle)PropertyInfo.GetValue(target);
            _xl = new UILabel() {
                Text = "X",
                Pivot = new Vector2(0, 0f),
                AnchorPoint = new Vector2(0, 0f),
                SizeStyle = SizeStyle.Block,
                SizeFactor = new Vector2(0.1f, 0f),
            };
            _yl = new UILabel() {
                Text = "Y",
                Pivot = new Vector2(0, 0f),
                AnchorPoint = new Vector2(0, 0f),
                SizeStyle = SizeStyle.Block,
                SizeFactor = new Vector2(0.1f, 0f),
            };
            _wl = new UILabel() {
                Text = "W",
                Pivot = new Vector2(0, 1f),
                AnchorPoint = new Vector2(0, 1f),
                SizeStyle = SizeStyle.Block,
                SizeFactor = new Vector2(0.1f, 0f),
            };
            _hl = new UILabel() {
                Text = "H",
                Pivot = new Vector2(0, 1f),
                AnchorPoint = new Vector2(0, 1f),
                SizeStyle = SizeStyle.Block,
                SizeFactor = new Vector2(0.1f, 0f),
            };
            _textX = new UIValueTextBox<float>(rect.X) {
                Pivot = new Vector2(0, 0f),
                AnchorPoint = new Vector2(0, 0f),
                SizeFactor = new Vector2(0.4f, 0f),
                Size = new Vector2(-5, 30f),
            };
            _textY = new UIValueTextBox<float>(rect.Y) {
                Pivot = new Vector2(0, 0f),
                AnchorPoint = new Vector2(0, 0f),
                SizeFactor = new Vector2(0.4f, 0f),
                Size = new Vector2(-5, 30f),
            };
            _textW = new UIValueTextBox<float>(rect.Width) {
                Pivot = new Vector2(0, 1f),
                AnchorPoint = new Vector2(0, 1f),
                SizeFactor = new Vector2(0.4f, 0f),
                Size = new Vector2(-5, 30f),
            };
            _textH = new UIValueTextBox<float>(rect.Height) {
                Pivot = new Vector2(0, 1f),
                AnchorPoint = new Vector2(0, 1f),
                SizeFactor = new Vector2(0.4f, 0f),
                Size = new Vector2(-5, 30f),
            };

            _textX.OnValueChanged += _textX_OnValueChanged;
            _textY.OnValueChanged += _textX_OnValueChanged;
            AppendChild(_xl);
            AppendChild(_yl);
            AppendChild(_wl);
            AppendChild(_hl);
            AppendChild(_textX);
            AppendChild(_textY);
            AppendChild(_textW);
            AppendChild(_textH);
        }

        private void _textX_OnValueChanged(UILib.Events.UIValueChangeEvent<float> e, UIElement sender) {
            OnValueChanged?.Invoke(e, sender);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _textX.Position = new Vector2(_xl.Width, 0);
            _xl.Position = new Vector2(0f, 5f);
            _yl.Position = new Vector2(_textX.Position.X + _textX.Width + 5f, 5f);
            _textY.Position = new Vector2(_yl.Position.X + _yl.Width, 0f);

            _textW.Position = new Vector2(_xl.Width, 0);
            _wl.Position = new Vector2(0f, -5f);
            _hl.Position = new Vector2(_textW.Position.X + _textW.Width + 5f, -5f);
            _textH.Position = new Vector2(_hl.Position.X + _hl.Width, 0);
        }

        public void UpdateValue() {
            Value = (Rectangle)PropertyInfo.GetValue(Target);
            _textX.Apply();
            _textY.Apply();
            _textW.Apply();
            _textH.Apply();
        }
    }
}
