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
    public class UIColorRect : UIElement {
        public event ActionEvent OnValueChanged;
        public float Hue { get; set; }
        public Vector2 SV { get; set; }
        private Effect _shader;
        private bool _isMouseDown;
        public override void MouseLeftDown(UIMouseEvent e) {
            _isMouseDown = true;
            base.MouseLeftDown(e);
        }

        public override void MouseLeftUp(UIMouseEvent e) {
            _isMouseDown = false;
            base.MouseLeftUp(e);
        }
        public UIColorRect() : base() {
            UseShader = true;
            PropagationRule = UILib.Enums.PropagationFlags.FocusEvents | UILib.Enums.PropagationFlags.ScrollWheel;
            _shader = UIEditor.Instance.GetEffect("Effects/ColorWheel");
            PostDrawSelf += UIColorRect_PostDrawSelf;
        }

        private void UIColorRect_PostDrawSelf(UIDrawEvent e, UIElement sender) {
            e.SpriteBatch.Draw(ModContent.GetTexture("UIEditor/Images/WhiteCircle"), new Vector2(Width * SV.X, Height * (1 - SV.Y)), null, Color.White, 0f, new Vector2(8f, 8f), 1f, SpriteEffects.None, 0f);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (_isMouseDown) {
                var pos = ScreenPositionToNodeAR(Main.MouseScreen, new Vector2(0, 0));
                SV = new Vector2(pos.X / Width, (1 - pos.Y / Height));
                OnValueChanged?.Invoke(new UIActionEvent(this, gameTime.TotalGameTime), this);
            }
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            _shader.Parameters["uDegree"].SetValue(Hue);
            _shader.CurrentTechnique.Passes["ColorRect"].Apply();
            sb.Draw(UIEditor.Instance.SkinManager.GetTexture("NoTexture"), new Rectangle(0, 0, Width, Height), Color.White);

        }
    }
}
