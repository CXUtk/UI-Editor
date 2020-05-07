
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;

namespace UIEditor.Editor.States {
    public class Canvas : UIElement {
        public Canvas() : base() {
            var button = new UIButton() {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(50, 50),
            };
            var test = new UISizer() {
                Size = new Vector2(50, 50),
                Pivot = new Vector2(0, 0),
                Position = new Vector2(100, 100),
            };
            AppendChild(button);
            AppendChild(test);

            test.AttachTo(button);
        }
    }
}
