using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Events {
    public class UIScrollWheelEvent : UIEvent {
        public int ScrollValue;
        public UIScrollWheelEvent(UIElement element, TimeSpan timestamp, int scrollValue)
            : base(element, timestamp) {
            ScrollValue = scrollValue;
        }
    }
}
