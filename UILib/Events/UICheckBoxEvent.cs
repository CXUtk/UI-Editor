using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components;

namespace UIEditor.UILib.Events {
    public class UICheckBoxEvent : UIEvent {
        public bool Value { get; set; }
        public UICheckBoxEvent(UIElement sender, TimeSpan timestamp, bool value) : base(sender, timestamp) {
            Value = value;
        }
    }
}
