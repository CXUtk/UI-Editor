using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;

namespace UIEditor.Editor.Components
{
	public class UIPropertyView : UIElement
	{
		public PropertyInfo Property { get; }
		public UIElement Target { get; }

		public object Value
		{
			get => Property.GetValue(Target);
			set => Property.SetValue(Target, Value);
		}

		public UIPropertyView(PropertyInfo prop, UIElement target)
		{
			if (!prop.DeclaringType.IsAssignableFrom(target.GetType()))
			{
				throw new ArgumentException(nameof(prop));
			}
			Property = prop;
			Target = target;
		}

		public T GetValue<T>()
		{
			return (T)Value;
		}
	}
}
