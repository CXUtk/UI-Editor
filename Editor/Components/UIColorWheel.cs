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

namespace UIEditor.Editor.Components {
    public class UIColorWheel : UIElement {
        private Effect _shader;
        public UIColorWheel() : base() {
            UseShader = true;
            _shader = UIEditor.Instance.GetEffect("Effects/ColorWheel");
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            _shader.CurrentTechnique.Passes["ColorWheel"].Apply();
            sb.Draw(UIEditor.Instance.SkinManager.GetTexture("NoTexture"), new Rectangle(0, 0, Width, Height), Color.White);
        }
    }
}
