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
    public class UIVector2 : UIElement, IUIUpdateable, IUIValue<Vector2> {
        public event ValueChangeEvent<float> OnValueChanged;
        private UILabel _xl, _yl;
        private UIValueTextBox<float> _textX, _textY;
        public bool Editable {
            get { return _textX.Editable && _textY.Editable; }
            set { _textX.Editable = _textY.Editable = value; }
        }
        public Vector2 Value {
            get {
                return new Vector2(_textX.Value, _textY.Value);
            }
            set {
                _textX.Value = value.X;
                _textY.Value = value.Y;
            }
        }
        public PropertyInfo PropertyInfo { get; }
        public UIElement Target { get; }

        public UIVector2(UIElement target, PropertyInfo property) : base() {
            PropertyInfo = property;
            Target = target;
            Vector2 vec = (Vector2)PropertyInfo.GetValue(target);
            _xl = new UILabel() {
                Text = "X",
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                SizeStyle = SizeStyle.Block,
                SizeFactor = new Vector2(0.1f, 0f),
            };
            _yl = new UILabel() {
                Text = "Y",
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                SizeStyle = SizeStyle.Block,
                SizeFactor = new Vector2(0.1f, 0f),
            };
            _textX = new UIValueTextBox<float>(vec.X) {
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                SizeFactor = new Vector2(0.4f, 1f),
                Size = new Vector2(-5, 0f),
            };
            _textY = new UIValueTextBox<float>(vec.Y) {
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                SizeFactor = new Vector2(0.4f, 1f),
                Size = new Vector2(-5, 0f),
            };

            _textX.OnValueChanged += _textX_OnValueChanged;
            _textY.OnValueChanged += _textX_OnValueChanged;
            AppendChild(_xl);
            AppendChild(_yl);
            AppendChild(_textX);
            AppendChild(_textY);
        }

        private void _textX_OnValueChanged(UILib.Events.UIValueChangeEvent<float> e, UIElement sender) {
            OnValueChanged?.Invoke(e, sender);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _textX.Position = new Vector2(_xl.Width, 0);
            _yl.Position = new Vector2(_textX.Position.X + _textX.Width + 5f, 0);
            _textY.Position = new Vector2(_yl.Position.X + _yl.Width, 0);
        }

        public void UpdateValue() {
            Value = (Vector2)PropertyInfo.GetValue(Target);
            _textX.Apply();
            _textY.Apply();
        }
    }
}
