using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib.Events;

namespace UIEditor.UILib.Components.Composite {
    public class UIValueTextBox<T> : UITextBox where T : IFormattable, IConvertible {
        public event ValueChangeEvent<T> OnValueChanged;
        private T _value;
        public T Value {
            get {
                return _value;
            }
            set {
                T prev = _value;
                _value = value;
                if (!_value.Equals(prev)) {
                    OnValueChanged?.Invoke(new UIValueChangeEvent<T>(this, Main._drawInterfaceGameTime.TotalGameTime, _value), this);
                    Text = _value.ToString();
                }

            }
        }
        public UIValueTextBox(T value) {
            Text = value.ToString();
            _value = value;
        }

        private void tryParse(string obj) {
            T pre = _value;
            try {
                Value = (T)Convert.ChangeType(obj, typeof(T));
            } catch (FormatException ex) {
                // Ignore
                ex.ToString();
                _value = pre;
            }
        }

        public override void TextChange(UITextChangeEvent e) {
            tryParse(e.NewString);
            base.TextChange(e);
        }

    }
}
