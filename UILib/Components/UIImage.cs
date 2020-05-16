using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Events;
using UIEditor.UILib.Components;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using UIEditor.Editor.Attributes;

namespace UIEditor.UILib.Components {
    public class UIImage : UIElement {
        [JsonIgnore]
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public SizeStyle SizeStyle { get; set; }

        public UIImage() : base() {
            Name = "图标按钮";
            SizeStyle = SizeStyle.Inline;
            Texture = Main.magicPixel;
            Color = Color.White;
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (SizeStyle == SizeStyle.Inline) {
                SizeFactor = new Vector2(0, 0);
                Size = Texture.Size();
            }
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            if (SizeStyle == SizeStyle.Inline) {
                sb.Draw(Texture, Pivot * new Vector2(Width, Height), null, Color, 0f, Pivot * Texture.Size(),
                   new Vector2(1, 1), SpriteEffects.None, 0f);
            } else {
                sb.Draw(Texture, Pivot * new Vector2(Width, Height), null, Color, 0f, Pivot * Texture.Size(),
                     new Vector2(Width / (float)Texture.Width, Height / (float)Texture.Height), SpriteEffects.None, 0f);
            }
        }
        public override object Clone() {
            UIImage image = (UIImage)base.Clone();
            image.SizeStyle = this.SizeStyle;
            image.Texture = this.Texture;
            image.Color = this.Color;
            return image;
        }
    }
}
