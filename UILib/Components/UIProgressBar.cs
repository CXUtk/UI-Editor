﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib.Components.Interface;

namespace UIEditor.UILib.Components {
    public class UIProgressBar : UIElement, IUIValue<float> {
        [JsonIgnore]
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
        public float Value {
            get { return _value; }
            set { _value = MathHelper.Clamp(value, 0, 1); }
        }
        private float _value;

        public UIProgressBar() : base() {
            Name = "进度条";
            FrameTexture = UIEditor.Instance.SkinManager.GetTexture("ProgressBar_Default");
            FrameCornerSize = new Vector2(8f, 8f);
            BackgroundColor = new Color(90, 90, 112);
            FillColor = Color.White;
            Value = 0.5f;
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            float width = Width - FrameCornerSize.X * 2;
            Drawing.DrawAdvBox(sb, 0, 0, Width, Height, Color.White, FrameTexture, FrameCornerSize);
            sb.Draw(Main.magicPixel, new Rectangle((int)(FrameCornerSize.X), (int)(FrameCornerSize.Y),
                (int)width, (int)(Height - FrameCornerSize.Y * 2)), BackgroundColor);
            width *= Value;
            sb.Draw(Main.magicPixel, new Rectangle((int)(FrameCornerSize.X), (int)(FrameCornerSize.Y),
                (int)width, (int)(Height - FrameCornerSize.Y * 2)), FillColor);
        }
    }
}
