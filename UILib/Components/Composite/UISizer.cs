using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components.Composite {
    public class UISizer : UIDraggable {
        private class UICornerDragger : UIDraggable {
            public UICornerDragger() : base() {

            }
            public override void DrawSelf(SpriteBatch sb) {
                sb.Draw(Main.magicPixel, new Rectangle(0, 0, Width, Height), Color.White);
                Drawing.DrawAdvBox(sb, new Rectangle(0, 0, Width, Height), Color.Black, UIEditor.Instance.SkinManager.GetTexture("BoxFrame_Default"), new Vector2(4, 4));
                base.DrawSelf(sb);
            }
        };
        private class UIBarDraggerH : UIDraggable {
            public UIBarDraggerH() : base() {

            }
            public override void CursorChange() {
                Main.cursorOverride = 18;
                Main.cursorColor = Color.White;
            }
            public override void DrawSelf(SpriteBatch sb) {
                sb.Draw(Main.magicPixel, new Rectangle(0, Height / 2 - 1, Width, 1), Color.White);
                base.DrawSelf(sb);
            }
        };
        private class UIBarDraggerV : UIDraggable {
            public UIBarDraggerV() : base() {

            }
            public override void CursorChange() {
                base.CursorChange();
                Main.cursorOverride = 19;
                Main.cursorColor = Color.White;
            }
            public override void DrawSelf(SpriteBatch sb) {
                sb.Draw(Main.magicPixel, new Rectangle(Width / 2 - 1, 0, 1, Height), Color.White);
                base.DrawSelf(sb);
            }
        };
        public event ActionEvent OnSizerChanged;
        public UIElement TargetElement { get; private set; }
        private UIDraggable[] _dragCorner;
        private UIDraggable[] _dragBar;
        private Vector2 _lastBottomRight;
        private Vector2 _lastPos;
        private Vector2 _lastSize;


        public UISizer() : base() {
            TargetElement = null;
            Pivot = new Vector2(0, 0);
            OnDragging += UISizer_OnDragging1;
            _dragBar = new UIDraggable[4];
            _dragBar[0] = new UIBarDraggerH() {
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(-24, 12),
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0f),
            };
            _dragBar[1] = new UIBarDraggerH() {
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(-24, 12),
                Pivot = new Vector2(0.5f, 1),
                AnchorPoint = new Vector2(0.5f, 1f),
            };
            _dragBar[2] = new UIBarDraggerV() {
                SizeFactor = new Vector2(0, 1),
                Size = new Vector2(12, -24),
                Pivot = new Vector2(0f, 0.5f),
                AnchorPoint = new Vector2(0f, 0.5f),
            };
            _dragBar[3] = new UIBarDraggerV() {
                SizeFactor = new Vector2(0, 1),
                Size = new Vector2(12, -24),
                Pivot = new Vector2(1f, 0.5f),
                AnchorPoint = new Vector2(1f, 0.5f),
            };


            _dragBar[0].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize - new Vector2(0, e.Moved.Y);
                    this.Position = _lastPos + new Vector2(0, e.Moved.Y);
                    _check();
                }
            };
            _dragBar[1].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize + new Vector2(0, e.Moved.Y);
                    _check();
                }
            };
            _dragBar[2].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize - new Vector2(e.Moved.X, 0);
                    this.Position = _lastPos + new Vector2(e.Moved.X, 0);
                    _check();
                }
            };
            _dragBar[3].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize + new Vector2(e.Moved.X, 0);

                    _check();
                }
            };


            for (int i = 0; i < 4; i++) AppendChild(_dragBar[i]);
            _dragCorner = new UIDraggable[4];
            for (int i = 0; i < 4; i++) {
                _dragBar[i].PropagationRule = Enums.PropagationFlags.FocusEvents | Enums.PropagationFlags.ScrollWheel;
                _dragBar[i].OnDragging += UISizer_OnDragging1;
                _dragCorner[i] = new UICornerDragger() {
                    Size = new Vector2(12, 12),
                    AnchorPoint = new Vector2(i & 1, (i >> 1) & 1),
                    Pivot = new Vector2(i & 1, (i >> 1) & 1),
                    PropagationRule = Enums.PropagationFlags.FocusEvents | Enums.PropagationFlags.ScrollWheel,
                };
                _dragCorner[i].OnDragging += UISizer_OnDragging1;
                AppendChild(_dragCorner[i]);
            }

            _dragCorner[0].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize - e.Moved;
                    this.Position = _lastPos + e.Moved;
                    _check();
                }
            };
            _dragCorner[1].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize + e.Moved * new Vector2(1, -1);
                    this.Position = _lastPos + new Vector2(0, e.Moved.Y);
                    _check();
                }
            };
            _dragCorner[2].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize + e.Moved * new Vector2(-1, 1);
                    this.Position = _lastPos + new Vector2(e.Moved.X, 0);
                    _check();
                }
            };
            _dragCorner[3].OnDragging += (e, sender) => {
                if (((UIDraggable)sender).IsDragging) {
                    sender.Position = new Vector2(0, 0);
                    this.Size = _lastSize + e.Moved;
                    _check();
                }
            };
        }
        private void UISizer_OnDragging1(Events.UIDraggingEvent e, UIElement sender) {
            if (TargetElement != null) {
                TargetElement.Size = Size - new Vector2(12, 12);
                TargetElement.RecalculateSelf();
                TargetElement.TopLeft = TargetElement.ScreenPositionToParentAR(ParentNodePositionToScreenAR(Position)) + new Vector2(6, 6);
            }
            OnSizerChanged?.Invoke(new Events.UIActionEvent(this, e.TimeStamp), this);
        }


        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _lastSize = Size;
            _lastPos = Position;
            _lastBottomRight = Position + new Vector2(Width, Height);
        }
        private void _check() {
            if (Size.X < 24) Size = new Vector2(24, Size.Y);
            if (Size.Y < 24) Size = new Vector2(Size.X, 24);
            if (Position.X >= _lastBottomRight.X - 24) Position = new Vector2(_lastBottomRight.X - 24, Position.Y);
            if (Position.Y >= _lastBottomRight.Y - 24) Position = new Vector2(Position.X, _lastBottomRight.Y - 24);

        }
        public void AttachTo(UIElement element) {
            element.Recalculate();
            TargetElement = element;
            Position = ScreenPositionToParentAR(element.InnerRectangleScreen.TopLeft()) - new Vector2(6, 6);
            Size = new Vector2(element.Width + 12f, element.Height + 12f);
            Recalculate();
        }


        public void UnAttach() {
            TargetElement = null;
        }
    }
}
