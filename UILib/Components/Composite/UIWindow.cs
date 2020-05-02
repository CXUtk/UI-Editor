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

        public event ActionEvent OnClose;

        public UIWindow() : base() {
            this.OnMouseDown += UIWindow_OnMouseDown;
            this.OnMouseUp += UIWindow_OnMouseUp;
            BlockPropagation = true;

            _closeButton = new UIImageButton() {
                Pivot = new Vector2(1, 0),
                AnchorPoint = new Vector2(1, 0),
                Position = new Vector2(-10, 5),
                Texture = UIEditor.Instance.SkinManager.GetTexture("CloseButton"),
                DefaultColor = Color.White * 0.8f,
                WhiteTexture = UIEditor.Instance.SkinManager.GetTexture("CloseButton_Change"),
                Scale = new Vector2(0.9f, 0.9f),
                BlockPropagation = true,
            };
            _closeButton.OnClick += _closeButton_OnClick;
            AppendChild(_closeButton);
        }

        private void _closeButton_OnClick(UIMouseEvent e, UIElement sender) {
            OnClose?.Invoke(new UIActionEvent(this, e.TimeStamp), sender);
        }

        private void UIWindow_OnMouseUp(UIMouseEvent e, UIElement sender) {
            _isDragging = false;
        }

        private void UIWindow_OnMouseDown(UIMouseEvent e, UIElement sender) {
            _isDragging = true;
            _dragOffset = e.MouseScreen - PostionScreen;
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (_isDragging) {
                Position = ScreenPositionToNode(Main.MouseScreen - _dragOffset);
            }
        }
    }
}
