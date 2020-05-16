
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.Editor.Attributes;
using UIEditor.UILib;
using UIEditor.UILib.Events;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;

namespace UIEditor.Editor.States {
    public class Canvas : UIElement {
        public UIElement Root { get; }
        public EditorState Editor { get; }
        public Canvas(EditorState editor) : base() {
            Editor = editor;

            Root = new UIElement() {
                SizeFactor = new Vector2(1f, 1f),
                Pivot = new Vector2(0, 0),
            };
            AppendChild(Root);
            Root.AppendChild(new UIButton() {
                Name = "测试按钮",
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Size = new Vector2(50, 50),
            });

        }


        private float _scale = 0f;
        public override void ScrollWheel(UIScrollWheelEvent e) {
            base.ScrollWheel(e);
            _scale += e.ScrollValue / 120f * 0.1f;
            _scale = MathHelper.Clamp(_scale, -1f, 1f);
            float s = (float)Math.Exp(_scale);
            Scale = new Vector2(s, s);
        }

        private bool _isRightDragging;
        private Vector2 _startPos;
        private Vector2 _startMousePos;
        public override void MouseRightDown(UIMouseEvent e) {
            base.MouseRightDown(e);
            _isRightDragging = true;
            _startPos = Position;
            _startMousePos = e.MouseScreen;
        }
        public override void MouseRightUp(UIMouseEvent e) {
            base.MouseRightUp(e);
            _isRightDragging = false;
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (_isRightDragging) {
                Vector2 offset = Main.MouseScreen - _startMousePos;
                Vector2 pos = _startPos + offset;
                var tmp = pos - PivotOffset;
                tmp.X = MathHelper.Clamp(tmp.X, -Width + Parent.Width / 2, -Parent.Width / 2);
                tmp.Y = MathHelper.Clamp(tmp.Y, -Height + Parent.Height / 2, -Parent.Height / 2);
                Main.NewText(tmp);
                Position = tmp + PivotOffset;
            }

        }
        public void PlaceElement(Vector2 pos, UIElement prefab) {
            var element = (UIElement)prefab.Clone();
            Root.AppendChild(element);
            element.Recalculate();
            element.Position = element.ScreenPositionToParentAR(pos);
            Editor.NotifyPlaceElement(element);
        }

    }
}
