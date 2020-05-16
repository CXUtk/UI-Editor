using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using UIEditor.UILib.Components;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.Components {
    public class UIColorBar : UIElement {
        public event ActionEvent OnValueChanged;
        public float Value { get; set; }
        private Effect _shader;
        private UIImage _arrow;
        private bool _isMouseDown;
        public override void MouseLeftDown(UIMouseEvent e) {
            _isMouseDown = true;
            base.MouseLeftDown(e);
        }

        public override void MouseLeftUp(UIMouseEvent e) {
            _isMouseDown = false;
            base.MouseLeftUp(e);
        }
        public UIColorBar() : base() {
            UseShader = true;
            PropagationRule = UILib.Enums.PropagationFlags.FocusEvents | UILib.Enums.PropagationFlags.ScrollWheel;
            _arrow = new UIImage() {
                AnchorPoint = new Vector2(1, 0),
                Pivot = new Vector2(1, 0.5f),
                Texture = UIEditor.Instance.SkinManager.GetTexture("ArrowSmall"),
            };
            Value = 0;
            AppendChild(_arrow);
            _shader = UIEditor.Instance.GetEffect("Effects/ColorWheel");
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (_isMouseDown) {
                int topY = 0, bottomY = Height;
                var posLocal = _arrow.ScreenPositionToParentAR(Main.MouseScreen);
                float r = (posLocal.Y - topY) / (bottomY - topY);
                if (float.IsNaN(r)) r = 0;
                Value = MathHelper.Clamp(r, 0, 1);
                OnValueChanged?.Invoke(new UIActionEvent(this, gameTime.TotalGameTime), this);
            }
            _arrow.Position = new Vector2(25, Value * Height);
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            _shader.CurrentTechnique.Passes["ColorBar"].Apply();
            _shader.Parameters["uDegree"].SetValue(0f);
            sb.Draw(UIEditor.Instance.SkinManager.GetTexture("NoTexture"), new Rectangle(0, 0, Width, Height), Color.White);
        }
    }
}
