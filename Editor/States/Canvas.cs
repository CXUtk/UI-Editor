
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
        public UISizer Sizer { get; }
        private EditorState _editor;

        public Canvas(EditorState editor) : base() {
            _editor = editor;
            Sizer = new UISizer() {
                IsActive = false,
            };
            Sizer.OnSizerChanged += _sizer_OnSizerChanged;
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
            AppendChild(Sizer);
        }

        private void _sizer_OnSizerChanged(UIActionEvent e, UIElement sender) {
            _editor.NotifySizerChanged(Sizer);
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
            if (Sizer.TargetElement != null && Sizer.TargetElement.ShouldRecalculate) {
                Sizer.AttachTo(Sizer.TargetElement);
            }
        }
        public void PlaceElement(Vector2 pos, UIElement prefab) {
            var element = (UIElement)prefab.Clone();
            Root.AppendChild(element);
            element.RecalculateSelf();
            element.Position = element.ScreenPositionToParentAR(pos);
            _editor.NotifyPlaceElement(element);
        }

        public void PlaceSizer(UIElement element) {
            if (element == null) {
                Sizer.UnAttach();
                Sizer.IsActive = false;
                return;
            }
            Sizer.IsActive = true;
            Sizer.AttachTo(element);
        }
    }
}
