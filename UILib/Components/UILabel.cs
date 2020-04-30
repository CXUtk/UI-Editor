using Microsoft.Xna.Framework;
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
        public float TextScale { get; set; }
        public bool IsLargeText { get; set; }
        /// <summary>
        /// 最大显示宽度，如果文本超过这个宽度就会被省略
        /// 如果设为-1就是不限制
        /// </summary>
        public float MaxWidth { get; set; }

        private string _displayString;

        public UILabel() : base() {
            Text = "文字";
            TextScale = 1f;
            TextColor = Color.White;
            IsLargeText = false;
            MaxWidth = -1;
        }

        public void CalculateSize() {
            _displayString = Text;
            var font = IsLargeText ? Main.fontDeathText : Main.fontMouseText;
            Size = new Vector2(font.MeasureString(Text).X, IsLargeText ? 42f : 18f) * TextScale;

            if (MaxWidth != -1) {
                _displayString = StringProcess.GetClampStringWithEllipses(font, _displayString, TextScale, MaxWidth);
                Size = new Vector2(font.MeasureString(_displayString).X, Size.Y);
            }
        }
        public override void UpdateSelf(GameTime gameTime) {
            CalculateSize();
            Recalculate();
            base.UpdateSelf(gameTime);
        }

        public override void DrawSelf(SpriteBatch sb) {
            var font = IsLargeText ? Main.fontDeathText : Main.fontMouseText;
            if (IsLargeText)
                Terraria.Utils.DrawBorderStringBig(sb, _displayString, Vector2.Zero, TextColor, TextScale);
            else
                Terraria.Utils.DrawBorderString(sb, _displayString, Vector2.Zero, TextColor, TextScale);
            base.DrawSelf(sb);
        }
    }
}
