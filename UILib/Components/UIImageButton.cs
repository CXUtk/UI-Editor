using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components {
    public class UIImageButton : UIElement {
        public Texture2D Texture { get; set; }
        public Texture2D WhiteTexture { get; set; }
        public float TextureScale { get; set; }
        public SizeStyle SizeStyle { get; set; }
        public Color DefaultColor { get; set; }
        public Color MouseOverColor { get; set; }
        public float TextureRotation { get; set; }

        private Color _color;
        private bool _isMouseOver;
        private int _timer;


        public UIImageButton() : base() {
            SizeStyle = SizeStyle.Inline;
            TextureScale = 1f;
            Texture = Main.magicPixel;
            DefaultColor = Color.Gray * 1.2f;
            MouseOverColor = Color.White;
            TextureRotation = 0f;

            this.OnMouseEnter += UIImageButton_OnMouseEnter;
            this.OnMouseOut += UIImageButton_OnMouseOut;
        }

        private void UIImageButton_OnMouseOut(Events.UIMouseEvent e, UIElement sender) {
            _isMouseOver = false;
        }

        private void UIImageButton_OnMouseEnter(Events.UIMouseEvent e, UIElement sender) {
            _isMouseOver = true;
            Main.PlaySound(12);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

            if (SizeStyle == SizeStyle.Inline) {
                SizeFactor = new Vector2(0, 0);
                Size = Texture.Size();
            }

            if (_isMouseOver && _timer < 15)
                _timer++;
            else if (!_isMouseOver && _timer > 0)
                _timer--;
            float factor = _timer / 15f;
            this._color = Color.Lerp(DefaultColor, MouseOverColor, factor);
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            if (SizeStyle == SizeStyle.Inline) {
                sb.Draw(Texture, Pivot * new Vector2(Width, Height), null, _color, TextureRotation, Pivot * Texture.Size(),
                   new Vector2(1, 1), SpriteEffects.None, 0f);
                if (IsMouseHover && WhiteTexture != null) {
                    sb.Draw(WhiteTexture, Pivot * new Vector2(Width, Height), null, _color, TextureRotation, Pivot * WhiteTexture.Size(),
                  new Vector2(1, 1), SpriteEffects.None, 0f);
                }
            } else {
                sb.Draw(Texture, Pivot * new Vector2(Width, Height), null, _color, TextureRotation, Pivot * Texture.Size(),
                     new Vector2(Width / (float)Texture.Width, Height / (float)Texture.Height), SpriteEffects.None, 0f);
                if (IsMouseHover && WhiteTexture != null) {
                    sb.Draw(WhiteTexture, Pivot * new Vector2(Width, Height), null, _color, TextureRotation, Pivot * WhiteTexture.Size(),
                     new Vector2(Width / (float)Texture.Width, Height / (float)Texture.Height), SpriteEffects.None, 0f); ;
                }
            }
        }
    }
}
