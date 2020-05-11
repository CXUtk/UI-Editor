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
        public T Value { get { return _value; } set { _value = value; } }
        public UIValueTextBox(T value) {
            Text = value.ToString();
            Value = value;
        }

        private void tryParse(string obj) {
            T pre = Value;
            try {
                Value = (T)Convert.ChangeType(obj, typeof(T));
                if (!Value.Equals(pre)) {
                    OnValueChanged?.Invoke(new UIValueChangeEvent<T>(this, Main._drawInterfaceGameTime.TotalGameTime, Value), this);
                    _text = Value.ToString();
                }
            } catch (FormatException ex) {
                // Ignore
                Value = pre;
            }
        }
        public override void UnFocus(UIActionEvent e) {
            base.UnFocus(e);
            _text = Value.ToString();
        }

        public override void TextChange(UITextChangeEvent e) {
            tryParse(e.NewString);
            base.TextChange(e);
        }

        public void Apply() {
            _text = Value.ToString();
        }

    }
}
