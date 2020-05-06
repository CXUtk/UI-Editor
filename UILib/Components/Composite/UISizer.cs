using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components.Composite {
    public class UISizer : UIElement {
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
            public override void DrawSelf(SpriteBatch sb) {
                sb.Draw(Main.magicPixel, new Rectangle(0, Height / 2 - 1, Width, 1), Color.White);
                base.DrawSelf(sb);
            }
        };
        private class UIBarDraggerV : UIDraggable {
            public UIBarDraggerV() : base() {

            }
            public override void DrawSelf(SpriteBatch sb) {
                sb.Draw(Main.magicPixel, new Rectangle(Width / 2 - 1, 0, 1, Height), Color.White);
                base.DrawSelf(sb);
            }
        };

        private UIDraggable[] _dragCorner;
        private UIDraggable[] _dragBar;
        private UIElement _targetElement;
        public UISizer() : base() {
            Pivot = new Vector2(0, 0);
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
            for (int i = 0; i < 4; i++) AppendChild(_dragBar[i]);
            _dragCorner = new UIDraggable[4];
            for (int i = 0; i < 4; i++) {
                _dragCorner[i] = new UICornerDragger() {
                    Size = new Vector2(12, 12),
                    AnchorPoint = new Vector2(i & 1, (i >> 1) & 1),
                    Pivot = new Vector2(i & 1, (i >> 1) & 1)
                };
                AppendChild(_dragCorner[i]);
            }

        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

            var pos = _dragCorner[0].InnerRectangleScreen.TopLeft();
            var br = _dragCorner[3].InnerRectangleScreen.BottomRight();
            Main.NewText(ScreenPositionToParentAR(pos));
            Main.NewText(Position);
            Position = ScreenPositionToParentAR(pos);
            Size = new Vector2(br.X - pos.X, br.Y - pos.Y);
            for (int i = 0; i < 4; i++) {
                _dragCorner[i].Position = new Vector2(0, 0);
            }
        }
        public override void UpdateChildren(GameTime gameTime) {
            base.UpdateChildren(gameTime);

        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);


        }

        public void AttachTo(UIElement element) {
            _targetElement = element;
            Size = new Vector2(_targetElement.Width + 12, _targetElement.Height + 12);
            Pivot = new Vector2(0, 0);
            Position = ScreenPositionToParentAR(_targetElement.InnerRectangleScreen.TopLeft());
        }
    }
}
