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
        public delegate void DraggingEvent(UIDraggingEvent e, UIElement sender);
        public event DraggingEvent OnDragging;
        private Vector2 _dragOffset;
        private Vector2 _dragStartPos;
        public bool IsDragging { get; private set; }
        public UIDraggable() : base() {

        }
        public override void DragStart(UIMouseEvent e) {
            IsDragging = true;
            _dragOffset = e.MouseScreen - PositionScreen;
            _dragStartPos = Position;
            base.DragStart(e);

        }
        public override void DragEnd(UIDragEndEvent e) {
            IsDragging = false;
            base.DragEnd(e);

        }

        public virtual void CursorChange() {
            Main.cursorOverride = 17;
            Main.cursorColor = Color.White;
        }
        public override void UpdateSelf(GameTime gameTime) {
            if (IsDragging) {
                Position = ScreenPositionToParentAR(Main.MouseScreen - _dragOffset);
                OnDragging?.Invoke(new UIDraggingEvent(this, Position - _dragStartPos, gameTime.TotalGameTime), this);
            }
            if (IsMouseHover || IsDragging) {
                CursorChange();
            }

            base.UpdateSelf(gameTime);
        }
    }
}
