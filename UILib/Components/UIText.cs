//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;

//namespace UIEditor.UILib.Components {
//    /// <summary>
//    /// 可以用于显示一大段文字
//    /// </summary>
//    public class UIText : UIElement {
//        public string Text { get; set; }
//        public Color TextColor { get; set; }
//        public float TextScale { get; set; }
//        public bool IsLargeText { get; set; }


//        public UIText() : base() {
//            Text = "文字";
//            TextScale = 1f;
//            TextColor = Color.White;
//            IsLargeText = false;
//            MaxWidth = -1;
//        }

//        public override void UpdateSelf(GameTime gameTime) {
//            _displayString = Text;
//            var font = IsLargeText ? Main.fontDeathText : Main.fontMouseText;
//            Size = new Vector2(font.MeasureString(Text).X, IsLargeText ? 42f : 18f) * TextScale;

//            if (MaxWidth != -1) {
//                _displayString = StringProcess.GetClampStringWithEllipses(font, _displayString, TextScale, MaxWidth);
//                Size = new Vector2(font.MeasureString(_displayString).X, Size.Y);
//            }
//            Recalculate();
//            base.UpdateSelf(gameTime);
//        }

//        public override void DrawSelf(SpriteBatch sb) {
//            var font = IsLargeText ? Main.fontDeathText : Main.fontMouseText;
//            if (IsLargeText)
//                Terraria.Utils.DrawBorderStringBig(sb, _displayString, Vector2.Zero, TextColor, TextScale);
//            else
//                Terraria.Utils.DrawBorderString(sb, _displayString, Vector2.Zero, TextColor, TextScale);
//            base.DrawSelf(sb);
//        }
//    }
//}
