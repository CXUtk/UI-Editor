using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIEditor.UILib.Components.Composite {
    public class UIToolBarH : UIElement {
        public Texture2D CollapseButtonUpTexture { get; set; }
        public Texture2D CollapseButtonDownTexture { get; set; }
        public Texture2D PanelTexture {
            get { return _toolPanel.PanelTexture; }
            set { _toolPanel.PanelTexture = value; }
        }
        public Texture2D PanelBorderTexture {
            get { return _toolPanel.PanelTexture; }
            set { _toolPanel.PanelTexture = value; }
        }
        private UIPanel _toolPanel;
        private bool _collapseOn;
        private float _timer;
        private UIImageButton _openButton;

        public UIToolBarH() : base() {
            _timer = 0;
            _collapseOn = false;
            CollapseButtonUpTexture = Drawing.CollapseButtonUp;
            CollapseButtonDownTexture = Drawing.CollapseButtonDown;
            _openButton = new UIImageButton() {
                Texture = CollapseButtonUpTexture,
                AnchorPoint = new Vector2(0.5f, 0f),
                Pivot = new Vector2(0.5f, 0f),
            };
            _openButton.OnClick += _openButton_OnClick;
            _toolPanel = new UIPanel() {
                SizeFactor = new Vector2(1, 1),
                AnchorPoint = new Vector2(0.5f, 0f),
                Pivot = new Vector2(0.5f, 0f),
            };

            AppendChild(_toolPanel);
            AppendChild(_openButton);
        }
        public void AddToPanel(UIElement element) {
            _toolPanel.AppendChild(element);
        }
        private void _openButton_OnClick(Events.UIMouseEvent e, UIElement sender) {
            _collapseOn ^= true;
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            Vector2 upperPos = new Vector2(0, 0);
            Vector2 lowerPos = new Vector2(0, _toolPanel.Height);
            if (_collapseOn) {
                _openButton.Texture = CollapseButtonDownTexture;
                if (_timer < 300) {
                    _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    _timer = 300;
                }
            } else {
                _openButton.Texture = CollapseButtonUpTexture;
                if (_timer > 0) {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    _timer = 0;
                }
            }
            Position = Vector2.Lerp(lowerPos, upperPos, _timer / 300.0f);
            _toolPanel.Position = new Vector2(0, _openButton.Height);
            _toolPanel.Size = new Vector2(-20, -_openButton.Height);
        }
    }
}
