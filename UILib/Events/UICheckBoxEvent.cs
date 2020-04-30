using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIEditor.UILib.Events
{
	public class UICheckBoxEvent : UIEvent
	{
		public bool OldValue { get; }
		public bool NewValue { get; set; }
		public UICheckBoxEvent(bool old, bool newValue, UICheckBox sender,TimeSpan timestamp) : base(sender, timestamp)
		{
			OldValue = old;
			NewValue = newValue;
		}
	}
}
