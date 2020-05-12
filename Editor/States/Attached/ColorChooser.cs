using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using Microsoft.Xna.Framework;
using UIEditor.UILib.Components.Composite;

namespace UIEditor.Editor.States.Attached {
    public class ColorChooser : UIState {
        private UIWindow _window;
        public ColorChooser(string name) : base(name) {
            _window = new UIWindow() {
                Size = new Vector2(280, 400),
                AnchorPoint = new Vector2(0.5f, 0.5f),
            };
            var chooser = new UIElement() {
                SizeFactor = new Vector2(1f, 0.5f),

            };

            _window.AppendChild(chooser);
            AppendChild(_window);
        }
    }
}
