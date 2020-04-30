using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using UIEditor;

namespace UIEditor.UILib {
    public static class Drawing {
        public static Texture2D PanelDefaultBackTexture;
        public static Texture2D ButtonDefaultBackTexture;
        public static Texture2D DefaultBoxTexture;
        public static Texture2D DefaultBox2Texture;
        public static Texture2D CloseButtonTexture;
        public static Texture2D CollapseButtonUp;
        public static Texture2D CollapseButtonDown;
        public static Texture2D CheckBox;
        public static Texture2D CheckBoxChecked;
        public static Mod CurrentMod;
        public static void Initialize(Mod mod) {
            CurrentMod = mod;
            PanelDefaultBackTexture = CurrentMod.GetTexture("Images/AdvInvBack1");
            ButtonDefaultBackTexture = CurrentMod.GetTexture("Images/AdvInvBack1");
            DefaultBoxTexture = CurrentMod.GetTexture("Images/Box");
            DefaultBox2Texture = CurrentMod.GetTexture("Images/Box2");
            CloseButtonTexture = CurrentMod.GetTexture("Images/CloseButton");
            CollapseButtonUp = CurrentMod.GetTexture("Images/CollapseButtonUp");
            CollapseButtonDown = CurrentMod.GetTexture("Images/CollapseButtonDown");
            CheckBox = CurrentMod.GetTexture("Images/CheckBox");
            CheckBoxChecked = CurrentMod.GetTexture("Images/CheckBox_Checked");
        }
        public static void Unload() {
            CurrentMod = null;
        }
        public static Color DefaultBoxColor {
            get {
                return new Color(0x3f, 0x41, 0x97);
            }
        }
        public static void DrawAdvBox(SpriteBatch sp, int x, int y, int w, int h, Color c, Texture2D img, Vector2 cornerSize) {
            Texture2D box = img;
            int width = (int)cornerSize.X;
            int height = (int)cornerSize.Y;
            if (w < cornerSize.X) {
                w = width;
            }
            if (h < cornerSize.Y) {
                h = width;
            }
            sp.Draw(box, new Rectangle(x, y, width, height), new Rectangle(0, 0, width, height), c);
            sp.Draw(box, new Rectangle(x + width, y, w - width * 2, height), new Rectangle(width, 0, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, y, width, height), new Rectangle(box.Width - width, 0, width, height), c);
            sp.Draw(box, new Rectangle(x, y + height, width, h - height * 2), new Rectangle(0, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x + width, y + height, w - width * 2, h - height * 2), new Rectangle(width, height, box.Width - width * 2, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle((x + w) - width, y + height, width, h - height * 2), new Rectangle(box.Width - width, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x, (y + h) - height, width, height), new Rectangle(0, box.Height - height, width, height), c);
            sp.Draw(box, new Rectangle(x + width, (y + h) - height, w - width * 2, height), new Rectangle(width, box.Height - height, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, (y + h) - height, width, height), new Rectangle(box.Width - width, box.Height - height, width, height), c);
        }
        public static void DrawStraightLine(SpriteBatch sb, Vector2 p1, Vector2 p2, int lineWidth = 1, Color color = default(Color)) {
            Vector2 dir = p2 - p1;
            float dis = dir.Length();
            dir.Normalize();
            for (int i = 0; i <= dis; i++) {
                sb.Draw(Main.magicPixel, p1, new Rectangle(0, 0, 1, 1), color, 0, new Vector2(0.5f, 0.5f), lineWidth, SpriteEffects.None, 0f);
                p1 += dir;
            }
        }
        public static void StrokeRect(SpriteBatch sb, Rectangle rect, int lineWidth = 1, Color color = default(Color)) {
            DrawStraightLine(sb, rect.TopLeft(), rect.TopRight(), lineWidth, color);
            DrawStraightLine(sb, rect.TopRight(), rect.BottomRight(), lineWidth, color);
            DrawStraightLine(sb, rect.BottomRight(), rect.BottomLeft(), lineWidth, color);
            DrawStraightLine(sb, rect.BottomLeft(), rect.TopLeft(), lineWidth, color);
        }
        public static void StrokePolygon(SpriteBatch sb, List<Vector2> points, int lineWidth = 1, Color color = default(Color)) {
            for (int i = 0; i < points.Count; i++) {
                DrawStraightLine(sb, points[i], points[(i + 1) % points.Count], lineWidth, color);
            }
        }
    }
}
