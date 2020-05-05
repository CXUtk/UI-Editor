using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components.Composite {
    public class UISizer : UIElement {
        private UIDraggable[] _dragCorner;
        private UIElement _targetElement;
        public UISizer() : base() {
            _dragCorner = new UIDraggable[4];
        }

        public void AttachTo(UIElement element) {
            _targetElement = element;
            Size = new Vector2(_targetElement.Width + 12, _targetElement.Height + 12);
            Pivot = new Vector2(0, 0);
            Position = ScreenPositionToParentAR(_targetElement.InnerRectangleScreen.TopLeft());
        }
    }
}
