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

namespace UIEditor.Editor.Components {
    public class UIVector2 : UIElement {
        public event ValueChangeEvent<float> OnValueChanged;
        private UILabel _xl, _yl;
        private UIValueTextBox<float> _textX, _textY;
        public float X { get { return _textX.Value; } set { _textX.Value = value; } }
        public float Y { get { return _textY.Value; } set { _textY.Value = value; } }
        public UIVector2(Vector2 vec) : base() {

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
    }
}
