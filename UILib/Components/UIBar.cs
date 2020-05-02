using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;

namespace UIEditor.UILib.Components {
    /// <summary>
    /// 一个简易的条状UI，由两端和中间组成
    /// </summary>
    public class UIBar : UIElement {
        public Texture2D Texture { get; set; }
        public int EndSize { get; set; }
        public Color Color { get; set; }
        public UIBar() : base() {
            Name = "条状UI";
            Texture = TextureManager.Load("Images/UI/ScrollbarInner");
            EndSize = 6;
            Color = Color.White;
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            sb.Draw(Texture, new Rectangle(0, 0, Width, Height), Color);
            //sb.Draw(Texture, new Rectangle(0, 0, Width, EndSize), new Rectangle(0, 0, Texture.Width, EndSize), Color);
            //sb.Draw(Texture, new Rectangle(0, EndSize, Width, Height - 2 * EndSize), new Rectangle(0, EndSize, Texture.Width, Texture.Height - 2 * EndSize), Color);
            //sb.Draw(Texture, new Rectangle(0, Height - EndSize, Width, EndSize), new Rectangle(0, Texture.Height - EndSize, Texture.Width, EndSize), Color);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }
    }
}
