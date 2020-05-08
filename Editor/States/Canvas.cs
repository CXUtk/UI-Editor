
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
        private UISizer _sizer;
        private EditorState _editor;

        public Canvas(EditorState editor) : base() {
            _editor = editor;
            _sizer = new UISizer() {
                IsActive = false,
            };
            Root = new UIElement() {
                SizeFactor = new Vector2(1f, 1f),
                Pivot = new Vector2(0, 0),
            };
            AppendChild(Root);
            Root.AppendChild(new UIButton() {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Size = new Vector2(50, 50),
            });
            AppendChild(_sizer);
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
                Position = _startPos + offset;
            }
        }
        //public UIElement GrabElement(Vector2 pos) {
        //    UIElement target = null;
        //    int sz = Children.Count;
        //    for (int i = sz - 1; i >= 0; i--) {
        //        var child = Children[i];
        //        if (child.IsActive && child.GetType() != typeof(UISizer) && child.ScreenHitBox.Contains(pos)) {
        //            var tmp = child.ElementAt(pos);
        //            if (tmp != null) {
        //                target = tmp;
        //                break;
        //            }
        //        }
        //    }
        //    return target;
        //}
        public void PlaceElement(Vector2 pos, UIElement prefab) {
            var element = (UIElement)prefab.Clone();
            Root.AppendChild(element);
            element.RecalculateSelf();
            element.Position = element.ScreenPositionToParentAR(pos);
            _editor.Browser.AddNode(element);
        }

        public void PlaceSizer(UIElement element) {
            if (element == null) {
                _sizer.UnAttach();
                _sizer.IsActive = false;
                return;
            }
            _sizer.IsActive = true;
            _sizer.AttachTo(element);
        }
    }
}
