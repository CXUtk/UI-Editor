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
        public SizeStyle SizeStyle { get; set; }

        private string _displayString;

        public UILabel() : base() {
            Text = "文字";
            TextScale = 1f;
            TextColor = Color.White;
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
                Size = new Vector2(0, Size.Y);
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
