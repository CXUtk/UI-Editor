using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components {
    public class UIProgressBar : UIElement {

        public Texture2D FrameTexture { get; set; }
        /// <summary>
        /// 边框贴图四个角的大小，除去这四个角后的区域为填充区域
        /// </summary>
        public Vector2 FrameCornerSize { get; set; }
        public Color BackgroundColor { get; set; }
        public Color FillColor { get; set; }
        /// <summary>
        /// 当前进度条的进度值，是一个0-1之间的浮点数
        /// </summary>
        public float CurrentValue {
            get { return _value; }
            set { _value = MathHelper.Clamp(value, 0, 1); }
        }
        private float _value;

        public UIProgressBar() : base() {
            FrameTexture = Drawing.ProgressBarTexture;
            FrameCornerSize = new Vector2(8f, 8f);
            BackgroundColor = new Color(90, 90, 112);
            FillColor = Color.White;
            CurrentValue = 0.5f;
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            float width = Width - FrameCornerSize.X * 2;
            Drawing.DrawAdvBox(sb, 0, 0, Width, Height, Color.White, FrameTexture, FrameCornerSize);
            sb.Draw(Main.magicPixel, new Rectangle((int)(FrameCornerSize.X), (int)(FrameCornerSize.Y),
                (int)width, (int)(Height - FrameCornerSize.Y * 2)), BackgroundColor);
            width *= CurrentValue;
            sb.Draw(Main.magicPixel, new Rectangle((int)(FrameCornerSize.X), (int)(FrameCornerSize.Y),
                (int)width, (int)(Height - FrameCornerSize.Y * 2)), FillColor);

        }
    }
}
