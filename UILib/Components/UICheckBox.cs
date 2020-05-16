using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using UIEditor.UILib.Components.Interface;
using UIEditor.UILib.Events;


namespace UIEditor.UILib.Components {
    public class UICheckBox : UIElement, IUIValue<bool> {
        public delegate void CheckedChangeHandler(UICheckBoxEvent e, UIElement sender);

        public bool Value { get; set; }

        public event CheckedChangeHandler OnCheckedChange;

        public UICheckBox() {
            Name = "复选框";
            Size = new Vector2(30, 30);
        }


        public override void MouseLeftClick(UIMouseEvent e) {
            Value ^= true;
            CheckedChange(new UICheckBoxEvent(this, Main._drawInterfaceGameTime.TotalGameTime, Value));
            base.MouseLeftClick(e);
        }

        public void CheckedChange(UICheckBoxEvent e) {
            OnCheckedChange?.Invoke(e, this);
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            var texture = UIEditor.Instance.SkinManager.GetTexture(Value ? "CheckBox_Checked" : "CheckBox_Default");
            sb.Draw(texture, Pivot * new Vector2(Width, Height), null, Color.White, 0, Pivot * texture.Size(),
                    new Vector2(1, 1), SpriteEffects.None, 0f);
            if (IsMouseHover) {
                sb.Draw(UIEditor.Instance.SkinManager.GetTexture("CheckBox_White"), Vector2.Zero, null, Color.White, 0, Vector2.Zero,
                    new Vector2(1, 1), SpriteEffects.None, 0f);
            }
        }
    }
}
