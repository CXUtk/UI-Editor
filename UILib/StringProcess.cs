using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using UIEditor;

namespace UIEditor.UILib {
    public static class StringProcess {
        public static string GetClampStringWithEllipses(DynamicSpriteFont font, string text, float scale, float maxLength) {
            StringBuilder sb = new StringBuilder();
            foreach (var c in text) {
                sb.Append(c);
                if (font.MeasureString(sb.ToString()).X * scale > maxLength && sb.Length >= 2) {
                    sb.Remove(sb.Length - 2, 2);
                    sb.Append("...");
                    break;
                }
            }
            return sb.ToString();
        }

        public static string GetClampString(DynamicSpriteFont font, string text, float scale, float maxLength) {
            StringBuilder sb = new StringBuilder();
            foreach (var c in text) {
                sb.Append(c);
                if (font.MeasureString(sb.ToString()).X * scale > maxLength && sb.Length >= 1) {
                    sb.Remove(sb.Length - 1, 1);
                    break;
                }
            }
            return sb.ToString();
        }
    }
}
