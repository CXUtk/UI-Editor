using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using UIEditor.UILib.Events;


namespace UIEditor.UILib
{
	public class UICheckBox : UIElement
	{

		private bool _checked;

		public delegate void CheckedChangeHandler(UICheckBoxEvent e, UIElement sender);

		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				if (_checked == value)
				{
					return;
				}
				var e = new UICheckBoxEvent(_checked, value, this, Terraria.Main._drawInterfaceGameTime.TotalGameTime);
				CheckedChange(e);
				_checked = e.NewValue;
			}
		}

		public event CheckedChangeHandler OnCheckedChange;

		public UICheckBox()
		{
			BlockPropagation = true;
			Size = new Vector2(30, 30);
			OnClick += UICheckBox_OnClick;
		}

		private void UICheckBox_OnClick(UIMouseEvent e, UIElement sender)
		{
				Checked ^= true;
		}

		public void CheckedChange(UICheckBoxEvent e)
		{
			OnCheckedChange?.Invoke(e, this);
		}

		public override void DrawSelf(SpriteBatch sb)
		{
			base.DrawSelf(sb);
			var texture = Checked ? Drawing.CheckBoxChecked : Drawing.CheckBox;
			sb.Draw(texture, Pivot * new Vector2(Width, Height), null, Color.White, 0, Pivot * texture.Size(),
					new Vector2(Width / (float)texture.Width, Height / (float)texture.Height), SpriteEffects.None, 0f);
		}
	}
}
