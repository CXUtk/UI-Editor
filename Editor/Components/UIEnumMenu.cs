using IL.Terraria;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components.Composite;

namespace UIEditor.Editor.Components
{
	public class UIEnumMenu<T> : UIPropertyView where T: Enum
	{
		private T value;
		public Type EnumType => typeof(T);
		public UIEnumMenu(PropertyInfo prop, UIElement target) : base(prop, target)
		{
			if (prop.PropertyType != typeof(T))
			{
				throw new ArgumentException(nameof(prop));
			}


		}
	}
}
