using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIEditor.UILib.Components.Composite {
    public class UIToolBarV : UIElement {
        public Texture2D CollapseButtonLeftTexture { get; set; }
        public Texture2D CollapseButtonRightTexture { get; set; }
        public string ButtonTooltip { get { return _openButton.Tooltip; } set { _openButton.Tooltip = value; } }
        public Texture2D PanelTexture {
            get { return _toolPanel.PanelTexture; }
            set { _toolPanel.PanelTexture = value; }
        }
        public Texture2D PanelBorderTexture {
            get { return _toolPanel.PanelTexture; }
            set { _toolPanel.PanelTexture = value; }
        }
        private readonly UIPanel _toolPanel;
        private bool _collapseOn;
        private float _timer;
        private readonly UIImageButton _openButton;

        public UIToolBarV() : base() {
            Name = "垂直工具栏";
            _timer = 0;
            _collapseOn = false;
            CollapseButtonLeftTexture = UIEditor.Instance.SkinManager.GetTexture("MoveLeftButton");
            CollapseButtonRightTexture = UIEditor.Instance.SkinManager.GetTexture("MoveRightButton");
            _openButton = new UIImageButton() {
                Texture = CollapseButtonRightTexture,
                AnchorPoint = new Vector2(1f, 0.5f),
                Pivot = new Vector2(1f, 0.5f),
                DefaultColor = new Color(150, 150, 150),
            };
            _openButton.OnClick += _openButton_OnClick;
            _toolPanel = new UIPanel() {
                SizeFactor = new Vector2(1, 1),
                AnchorPoint = new Vector2(1f, 0.5f),
                Pivot = new Vector2(1f, 0.5f),
                PanelTexture = UIEditor.Instance.SkinManager.GetTexture("ToolBar_Default"),
                CornerSize = new Vector2(6f, 6f),
                BlockPropagation = true,
            };
            NoEvent = true;
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
            Vector2 leftPos = new Vector2(-_toolPanel.Width, 0);
            Vector2 rightPos = new Vector2(0, 0);
            if (_collapseOn) {
                _openButton.Texture = CollapseButtonLeftTexture;
                if (_timer < 300) {
                    _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    _timer = 300;
                }
            } else {
                _openButton.Texture = CollapseButtonRightTexture;
                if (_timer > 0) {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    _timer = 0;
                }
            }
            Position = Vector2.Lerp(leftPos, rightPos, _timer / 300.0f);
            _toolPanel.Position = new Vector2(-_openButton.Width + 2, 0);
            _toolPanel.Size = new Vector2(-_openButton.Width + 2, 0);
        }
    }
}
