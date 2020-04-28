using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Xna.Framework.Input;

namespace UIEditor.UILib.Events
{
	public class UIKeyEvent : UIEvent
	{
		public Keys Key { get; }
		public bool Handled { get; set; }
		public UIKeyEvent(UIElement element, TimeSpan timestamp, Keys key) : base(element, timestamp)
		{
			Key = key;
		}
	}
}
