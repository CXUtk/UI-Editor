﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components {
    /// <summary>
    /// 自动适应大小的标签UI组件
    /// 只显示一行文本
    /// </summary>
    public class UILabel : UIElement {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public Color BackgroundColor { get; set; }
        public float TextScale { get { return _textScale; } set { CheckRecalculate(_textScale, value); _textScale = value; } }
        private float _textScale;
        public bool IsLargeText { get; set; }
        public SizeStyle SizeStyle { get; set; }

        private string _displayString;

        public UILabel() : base() {
            Text = "文字";
            TextScale = 1f;
            TextColor = Color.White;
            BackgroundColor = Color.Transparent;
            IsLargeText = false;
            SizeStyle = SizeStyle.Inline;
        }
        public Vector2 MeasureSize(string str) {
            var font = IsLargeText ? Main.fontDeathText : Main.fontMouseText;
            return new Vector2(font.MeasureString(str).X, IsLargeText ? 42f : 18f) * TextScale;

        }
        public void CalculateSize() {
            _displayString = Text;
            var font = IsLargeText ? Main.fontDeathText : Main.fontMouseText;
            if (SizeStyle == SizeStyle.Inline)
                Size = MeasureSize(Text);
            else {
                _displayString = StringProcess.GetClampStringWithEllipses(font, _displayString, TextScale, Width);
                Size = new Vector2(Size.X, MeasureSize(_displayString).Y);
            }
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            CalculateSize();
            RecalculateSelf();
        }

        public override void DrawSelf(SpriteBatch sb) {
            var font = IsLargeText ? Main.fontDeathText : Main.fontMouseText;
            sb.Draw(Main.magicPixel, new Rectangle(0, 0, Width, Height), BackgroundColor);
            if (IsLargeText)
                Utils.DrawBorderStringBig(sb, _displayString, Vector2.Zero, TextColor, TextScale);
            else
                Utils.DrawBorderString(sb, _displayString, Vector2.Zero, TextColor, TextScale);
            base.DrawSelf(sb);
        }
    }
}
