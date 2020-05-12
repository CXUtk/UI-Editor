using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;
using UIEditor.UILib.Events;
using UIEditor.UILib.Components.Interface;
using UIEditor.UILib.Components.Composite;

namespace UIEditor.UILib.Components {
    public class UIValueSlider : UIElement, IUIValue<float> {
        public Color SliderColor { get { return _slider._innerLine.Color; } set { _slider._innerLine.Color = value; } }
        private class UIInnerSlider : UIElement {
            private bool _isMouseDown;
            private float _offsetX;
            public float _currentValue;
            public UIBar _innerLine;
            public UIButton _sliderButton;
            public event ActionEvent OnValueChanged;

            public UIInnerSlider() : base() {
                _innerLine = new UIBar() {
                    AnchorPoint = new Vector2(0.5f, 0.5f),
                    SizeFactor = new Vector2(1f, 0f),
                    Size = new Vector2(-10, 3),
                    Position = new Vector2(0f, 0),
                    Texture = Main.magicPixel,
                };

                _sliderButton = new UIButton() {
                    AnchorPoint = new Vector2(0f, 0.5f),
                    Pivot = new Vector2(0.5f, 0.5f),
                    Size = new Vector2(12, 30),
                    PanelTexture = UIEditor.Instance.SkinManager.GetTexture("BoxTR_Default"),
                    CornerSize = new Vector2(6f, 6f),
                    DrawPanel = true,
                    Text = "",
                    Position = new Vector2(5f, 0),
                };
                AppendChild(_innerLine);
                AppendChild(_sliderButton);
            }

            public override void MouseLeftDown(UIMouseEvent e) {
                _isMouseDown = true;
                _offsetX = 0;
                base.MouseLeftDown(e);
            }

            public override void MouseLeftUp(UIMouseEvent e) {
                _isMouseDown = false;
                base.MouseLeftUp(e);
            }

            public override void UpdateSelf(GameTime gameTime) {
                base.UpdateSelf(gameTime);
                int topX = 5, bottomX = Width - 10;
                if (_isMouseDown) {
                    var posLocal = _sliderButton.ScreenPositionToParentAR(Main.MouseScreen + new Vector2(_offsetX, 0));
                    float r = (posLocal.X - topX) / (bottomX - topX);
                    if (float.IsNaN(r)) r = 0;
                    _currentValue = MathHelper.Clamp(r, 0, 1);
                    OnValueChanged?.Invoke(new UIActionEvent(this, gameTime.TotalGameTime), this);
                }
                var pos = new Vector2(MathHelper.Lerp(topX, bottomX, _currentValue), 0);
                _sliderButton.Position = pos;
            }
            public void UpdateValue() {
                int topX = 5, bottomX = Width - 10;
                var pos = new Vector2(MathHelper.Lerp(topX, bottomX, _currentValue), 0);
                _sliderButton.Position = pos;
            }

        }

        /// <summary>
        /// 当前滚动条运动到的位置的比例，这个值处于Min和Max之间
        /// </summary>
        public float Value { get { return _slider._currentValue * (Max - Min) + Min; } set { _slider._currentValue = MathHelper.Clamp((value - Min) / (float)(Max - Min), 0, 1); _slider.UpdateValue(); } }
        public int Min { get; set; }
        public int Max { get; set; }
        private UIInnerSlider _slider;
        private UIValueTextBox<int> _display;

        public UIValueSlider() : base() {
            Overflow = OverflowType.Hidden;
            Name = "滑动值域";
            PropagationRule = Enums.PropagationFlags.FocusEvents | Enums.PropagationFlags.ScrollWheel;
            Min = 0;
            Max = 1;
            _slider = new UIInnerSlider() {
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.7f, 1f),
            };
            _display = new UIValueTextBox<int>(Min) {
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.3f, 1f),
                Size = new Vector2(-7, 0),
            };
            _display.Text = Min.ToString();
            _display.OnValueChanged += _display_OnValueChanged;
            _slider.OnValueChanged += _slider_OnValueChanged;
            SliderColor = UIEditor.Instance.SkinManager.GetColor("Background2");
            AppendChild(_slider);
            AppendChild(_display);
        }

        private void _display_OnValueChanged(UIValueChangeEvent<int> e, UIElement sender) {
            Value = e.Value;
            _display.Value = (int)Value;
            _display.Apply();
        }

        private void _slider_OnValueChanged(Events.UIActionEvent e, UIElement sender) {
            _display.Value = (int)Value;
            _display.Apply();
            _display.Recalculate();
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _display.Position = new Vector2(_slider.Width + 7f, 0);

        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);

        }
    }
}
