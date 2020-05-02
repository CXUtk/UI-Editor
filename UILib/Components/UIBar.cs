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
        public DrawStyle DrawStyle { get; set; }
        public UIBar() : base() {
            Texture = TextureManager.Load("Images/UI/ScrollbarInner");
            EndSize = 6;
            Color = Color.White;
            DrawStyle = DrawStyle.Vertical;
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            if (DrawStyle == DrawStyle.Vertical) {
                sb.Draw(Texture, new Rectangle(0, 0, Width, Height), Color);
                //sb.Draw(Texture, new Rectangle(0, 0, Width, EndSize), new Rectangle(0, 0, Texture.Width, EndSize), Color);
                //sb.Draw(Texture, new Rectangle(0, EndSize, Width, Height - 2 * EndSize), new Rectangle(0, EndSize, Texture.Width, Texture.Height - 2 * EndSize), Color);
                //sb.Draw(Texture, new Rectangle(0, Height - EndSize, Width, EndSize), new Rectangle(0, Texture.Height - EndSize, Texture.Width, EndSize), Color);
            } else {
                sb.Draw(Texture, new Rectangle(0, 0, Width, Height), Color);
                //sb.Draw(Texture, new Rectangle(0, 0, EndSize, Height), new Rectangle(0, 0, EndSize, Texture.Height), Color);
                //sb.Draw(Texture, new Rectangle(EndSize, 0, Width - 2 * EndSize, Height), new Rectangle(EndSize, 0, Texture.Width - 2 * EndSize, Texture.Height), Color);
                //sb.Draw(Texture, new Rectangle(Width - EndSize, 0, EndSize, Height), new Rectangle(Texture.Width - EndSize, 0, EndSize, Texture.Height), Color);
            }
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }
    }
}
