using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static UIEditor.UILib.UIElement;

namespace UIEditor.UILib.Components.Composite {
    public class UISelectableList : UIList {
        public event ActionEvent OnSelect;
        public UISelectableList() : base() {
            Name = "可选列表";
        }

        public override void AddElement(UIElement element) {
            base.AddElement(element);
            element.OnMouseDown += Element_OnMouseDown;
        }

        private void Element_OnMouseDown(Events.UIMouseEvent e, UIElement sender) {
            SelectedElement = sender;
            OnSelect?.Invoke(new Events.UIActionEvent(this, Main._drawInterfaceGameTime.TotalGameTime), this);
        }
        public override void UpdateElementPos(GameTime gameTime) {
            base.UpdateElementPos(gameTime);
            foreach (var element in _elements) {
                element.IsSelected = (SelectedElement == element);
            }
        }
    }
}
