using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Events {
    public class UIValueChangeEvent<T> : UIEvent {
        public T Value { get; }
        public UIValueChangeEvent(UIElement element, TimeSpan timestamp, T value)
            : base(element, timestamp) {
            Value = value;
        }
    }
}
