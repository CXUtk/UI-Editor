using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components
{
	public class UIMenu : UIElement
	{
		private List<UIMenuItem> items;

		public float ItemsPadding { get; set; }

		public UIMenu()
		{
			items = new List<UIMenuItem>();
			ItemsPadding = 6;
		}

		public void AddItem(UIMenuItem item)
		{
			items.Add(item);
			AppendChild(item);
		}

		public void RemoveItem(UIMenuItem item)
		{
			items.Remove(item);
			RemoveChild(item);
		}

		public override void UpdateChildren(GameTime gameTime)
		{
			foreach (var child in Children)
			{
				if (child.IsActive && !(child is UIMenuItem))
				{
					child.Update(gameTime);
				}
			}
			items.ForEach(item => item.UpdateExpand());
			UpdateItemsPosition();
			items.ForEach(item => item.Update(gameTime));
		}

		private void UpdateItemsPosition()
		{
			var start = PostionScreen - Pivot * new Vector2(Width, Height);
			foreach (var item in items)
			{
				item.PostionScreen = start;
				if (item.ShouldExpand)
				{
					item.UpdateSubItemsPosition(0);
				}
				start.X += item.Width;
			}
			Size = start - PostionScreen + Pivot * new Vector2(Width, Height);
			Size = new Vector2(Size.X, 20);
		}
	}
}
