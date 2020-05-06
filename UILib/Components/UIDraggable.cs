using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib.Events;

namespace UIEditor.UILib.Components {
    public class UIDraggable : UIElement {
        public event ActionEvent OnUpdate;
        private Vector2 _dragOffset;
        public bool IsDragging { get; private set; }
        public UIDraggable() : base() {

        }
        public override void DragStart(UIMouseEvent e) {
            IsDragging = true;
            _dragOffset = e.MouseScreen - PositionScreen;
            base.DragStart(e);

        }
        public override void DragEnd(UIDragEndEvent e) {
            IsDragging = false;
            base.DragEnd(e);

        }
        public override void UpdateSelf(GameTime gameTime) {
            if (IsDragging)
                Position = ScreenPositionToParentAR(Main.MouseScreen - _dragOffset);
            if (IsMouseHover) {
                Main.cursorOverride = 17;
                Main.cursorColor = Color.White;
            }
            OnUpdate?.Invoke(new UIActionEvent(this, gameTime.TotalGameTime), this);
            base.UpdateSelf(gameTime);
        }
    }
}
