using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.Components
{
	public class UIEnumSelector : UIElement
	{
		private class EnumValueButton : ToolBarButton
		{
			public object Value { get; }
			public EnumValueButton(object enumValue)
			{
				Value = enumValue;
				Name = enumValue.ToString();
				Text = enumValue.ToString();
			}
			public override void UpdateSelf(GameTime gameTime)
			{
				base.UpdateSelf(gameTime);
				var size = Parent.Size;
				size.Y = 18;
				Size = size;
				if(IsMouseHover)
				{
					_label.TextColor = Color.Blue;
				}
				else
				{
					_label.TextColor = Color.White;
				}
			}
		}

		private UISelectableList list;
		private UILabel label;
		private UIImageButton expandButton;

		private object currentValue;

		public Type EnumType { get; }
		public object CurrentValue
		{
			get
			{
				return currentValue;
			}
			set
			{
				if (value.GetType() == EnumType)
				{
					currentValue = value;
					label.Text = value.ToString();
					OnValueChange?.Invoke(new UIValueChangeEvent<object>(this, Main._drawInterfaceGameTime.TotalGameTime, value), this);
					return;
				}
				throw new ArgumentException(nameof(value));
			}
		}

		public event ValueChangeEvent<object> OnValueChange;

		public UIEnumSelector(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(nameof(enumType));
			}
			label = new UILabel()
			{
				NoEvent = true,
				Text = string.Empty
			};
			expandButton = new UIImageButton()
			{
				Texture = UIEditor.Instance.SkinManager.GetTexture("Down"),
				WhiteTexture = UIEditor.Instance.SkinManager.GetTexture("Down_Border"),
				Pivot = new Vector2(1, 0),
				AnchorPoint = new Vector2(1, 0),
				Position = new Vector2(-1, 1),
				Size = new Vector2(18, 18),
				SizeStyle = SizeStyle.Block
			};
			expandButton.OnClick += (e, sender) => list.IsActive ^= true;
			#region LoadList
			EnumType = enumType;
			var values = enumType.GetEnumValues();
			list = new UISelectableList
			{
				Name = "Values",
				InnerContainerPadding = 2,
				AnchorPoint = new Vector2(0.5f, 0),
				Pivot = new Vector2(0.5f, 0),
				Position = new Vector2(0, 18 + 2)
			};

			foreach (var value in values)
			{
				list.AddElement(new EnumValueButton(value));
			}
			list.OnSelect += List_OnSelect;
			list.IsActive = false;
			list.Size = new Vector2(80, 100);
			#endregion
			AppendChild(label);
			AppendChild(list);
			AppendChild(expandButton);
		}

		public override void UpdateSelf(GameTime gameTime)
		{
			base.UpdateSelf(gameTime);
			var size = list.Size;
			size.X = Size.X;
			list.Size = size;
			size.Y = 18 + 2;
			if (list.IsActive)
			{
				size.Y += list.Height;
			}
			Size = size;
		}

		private void List_OnSelect(UILib.Events.UIActionEvent e, UIElement sender)
		{
			CurrentValue = ((EnumValueButton)e.Target).Value;
			list.IsActive = false;
		}
	}
}
