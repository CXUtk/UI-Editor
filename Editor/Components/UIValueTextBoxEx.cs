using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Components.Interface;

namespace UIEditor.Editor.Components {
    public class UIValueTextBoxEx<T> : UIValueTextBox<T>, IUIUpdateable where T : IFormattable, IConvertible {
        public PropertyInfo PropertyInfo { get; }
        public UIElement Target { get; }

        public UIValueTextBoxEx(UIElement target, PropertyInfo property) : base((T)property.GetValue(target)) {
            PropertyInfo = property;
            Target = target;
            UpdateValue();
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }

        public void UpdateValue() {
            Value = (T)PropertyInfo.GetValue(Target);
            Apply();
        }
    }
}
