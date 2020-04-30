using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIEditor.UILib.Events
{
	public class UITextChangeEvent : UIEvent
	{
		public bool Cancel { get; set; }
		public string OldString { get; }
		public string NewString { get; set; }
		public UITextChangeEvent(string oldString,string newString, UIElement target, TimeSpan timestamp) : base(target, timestamp)
		{
			OldString = oldString;
			NewString = newString;
		}
	}
}
