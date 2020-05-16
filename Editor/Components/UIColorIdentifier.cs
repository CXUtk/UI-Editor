using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using UIEditor.UILib.Components.Interface;
using System.Reflection;

namespace UIEditor.Editor.Components {
    public class UIColorIdentifier : UIElement, IUIUpdateable {
        public Color Color { get; set; }
        public PropertyInfo PropertyInfo { get; }
        public UIElement Target { get; }

        public UIColorIdentifier(PropertyInfo info, UIElement target) : base() {
            PropertyInfo = info;
            Target = target;
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            int div = (int)(Height * 0.7f);
            Drawing.DrawAdvBox(sb, new Rectangle(0, 0, Width, Height), Color.White, UIEditor.Instance.SkinManager.GetTexture("Box_Default"), new Vector2(4f, 4f));
            sb.Draw(Main.magicPixel, new Rectangle(0, 0, Width, div), Color);
            sb.Draw(Main.magicPixel, new Rectangle(0, div, Width, 1), Color.White);
        }

        public void UpdateValue() {
            Color = (Color)PropertyInfo.GetValue(Target);
        }
    }
}
