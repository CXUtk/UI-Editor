using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components {
    public class UIImageButton : UIElement {
        /// <summary>
        /// 按钮本体的图案
        /// </summary>
        [JsonIgnore]
        public Texture2D Texture { get; set; }
        /// <summary>
        /// 按钮白边的图案，会在鼠标移动上去的时候显示
        /// </summary>
        [JsonIgnore]
        public Texture2D WhiteTexture { get; set; }
        /// <summary>
        /// 按钮容器大小的计算方式，inline代表只计算贴图大小，block代表计算指定的大小
        /// </summary>
        public SizeStyle SizeStyle { get; set; }
        public Color DefaultColor { get; set; }
        public Color MouseOverColor { get; set; }
        public float TextureRotation { get; set; }

        private Color _color;
        private bool _isMouseOver;
        private int _timer;


        public UIImageButton() : base() {
            Name = "图标按钮";
            SizeStyle = SizeStyle.Inline;
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

        public override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);
            if (SizeStyle == SizeStyle.Inline)
            {
                sb.Draw(Texture, Pivot * new Vector2(Width, Height), null, _color, TextureRotation, Pivot * Texture.Size(),
                   new Vector2(1, 1), SpriteEffects.None, 0f);
                if (IsMouseHover && WhiteTexture != null)
                {
                    sb.Draw(WhiteTexture, Pivot * new Vector2(Width, Height), null, Color.White, TextureRotation, Pivot * WhiteTexture.Size(),
                  new Vector2(1, 1), SpriteEffects.None, 0f);
                }
            }
            else
            {
                sb.Draw(Texture, new Rectangle(0, 0, Width, Height), null, _color, TextureRotation, Pivot * new Vector2(Width, Height), SpriteEffects.None, 0f);
                if (IsMouseHover && WhiteTexture != null)
                {
                    sb.Draw(WhiteTexture, new Rectangle(0, 0, Width, Height), null, Color.White, TextureRotation, Pivot * new Vector2(Width, Height), SpriteEffects.None, 0f);
                }
            }
        }

        public override object Clone() {
            UIImageButton button = (UIImageButton)base.Clone();
            button.SizeStyle = this.SizeStyle;
            button.Texture = this.Texture;
            button.WhiteTexture = this.WhiteTexture;
            return button;
        }
    }
}
