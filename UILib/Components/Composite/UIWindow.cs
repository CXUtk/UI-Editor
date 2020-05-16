using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Events;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace UIEditor.UILib.Components.Composite {
    public class UIWindow : UIPanel {
        private readonly UIImageButton _closeButton;
        private bool _isDragging;
        private Vector2 _dragOffset;
        public Vector2 CloseButtonOffset { get { return _closeButton.Position; } set { _closeButton.Position = value; } }

        public event ActionEvent OnClose;

        public UIWindow() : base() {
            Name = "窗口";
            PropagationRule = Enums.PropagationFlags.FocusEvents;

            _closeButton = new UIImageButton() {
                Pivot = new Vector2(1, 0),
                AnchorPoint = new Vector2(1, 0),
                Texture = UIEditor.Instance.SkinManager.GetTexture("CloseButton"),
                DefaultColor = Color.White * 0.8f,
                WhiteTexture = UIEditor.Instance.SkinManager.GetTexture("CloseButton_Change"),
                Scale = new Vector2(0.9f, 0.9f),
                PropagationRule = Enums.PropagationFlags.BLOCK_ALL,
            };
            CloseButtonOffset = new Vector2(-10, 5);
            _closeButton.OnClick += _closeButton_OnClick;
            AppendChild(_closeButton);
        }

        private void _closeButton_OnClick(UIMouseEvent e, UIElement sender) {
            OnClose?.Invoke(new UIActionEvent(this, e.TimeStamp), sender);
        }

        public override void DragStart(UIMouseEvent e) {
            _isDragging = true;
            _dragOffset = e.MouseScreen - PositionScreen;
            base.DragStart(e);
        }

        public override void DragEnd(UIDragEndEvent e) {
            _isDragging = false;
            base.DragEnd(e);
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (_isDragging) {
                Position = ScreenPositionToParentAR(Main.MouseScreen - _dragOffset);
            }
        }
    }
}
