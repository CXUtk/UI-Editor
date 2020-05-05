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

namespace UIEditor.UILib.Components {
    public class UIScrollBarH : UIElement {
        public Texture2D OuterTexture {
            get {
                return _outerBar.Texture;
            }
            set {
                _outerBar.Texture = value;
            }
        }
        public Texture2D InnerTexture {
            get {
                return _innerBar.Texture;
            }
            set {
                _innerBar.Texture = value;
            }
        }
        /// <summary>
        /// 滚动条两端的贴图高度
        /// </summary>
        public int EndSize {
            get {
                return _innerBar.EndSize;
            }
            set {
                _innerBar.EndSize = _outerBar.EndSize = value;
            }
        }


        public Color BackgroundColor {
            get {
                return _outerBar.Color;
            }
            set {
                _outerBar.Color = value;
            }
        }

        public Color DefaultInnerColor { get; set; }
        public Color MouseMoveInnerColor { get; set; }

        public float ViewSize { get; set; }
        /// <summary>
        /// 当前滚动条运动到的位置的比例，这个值处于0和1之间
        /// </summary>
        public float CurrentValue { get { return _currentValue; } set { _currentValue = MathHelper.Clamp(value, 0, 1); } }

        private float _currentValue;

        private UIBar _innerBar;
        private UIBar _outerBar;

        private bool _isMouseOver;
        private bool _isMouseDown;
        private float _offsetX;
        private float _timer;

        private const float PADDING = 5;
        public UIScrollBarH() : base() {
            Name = "水平滚动条";
            BlockPropagation = true;
            _currentValue = 0f;
            _isMouseOver = false;
            var tex = Main.magicPixel;
            _outerBar = new UIBar() {
                Texture = tex,
                SizeFactor = new Vector2(1f, 1f),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                EndSize = 6
            };
            ViewSize = 0.5f;
            _innerBar = new UIBar() {
                Name = "Inner",
                Texture = tex,
                SizeFactor = new Vector2(0.5f, 1f),
                Pivot = new Vector2(0f, 0.5f),
                AnchorPoint = new Vector2(0f, 0.5f),
                EndSize = 6
            };
            BackgroundColor = Color.Gray;
            DefaultInnerColor = Color.White * 0.5f;
            MouseMoveInnerColor = Color.White;
            _innerBar.OnMouseEnter += _innerBar_OnMouseOver;
            _innerBar.OnMouseOut += _innerBar_OnMouseOut;
            _innerBar.OnMouseDown += _innerBar_OnMouseDown;
            _innerBar.OnMouseUp += _innerBar_OnMouseUp;
            _timer = 0;
            AppendChild(_outerBar);
            _outerBar.AppendChild(_innerBar);

        }

        private void _innerBar_OnMouseUp(Events.UIMouseEvent e, UIElement sender) {
            _isMouseDown = false;
        }

        private void _innerBar_OnMouseDown(Events.UIMouseEvent e, UIElement sender) {
            _isMouseDown = true;
            _offsetX = _innerBar.PostionScreen.X - e.MouseScreen.X;
        }

        private void _innerBar_OnMouseOut(Events.UIMouseEvent e, UIElement sender) {
            _isMouseOver = false;
        }

        private void _innerBar_OnMouseOver(Events.UIMouseEvent e, UIElement sender) {
            _isMouseOver = true;
            Main.PlaySound(12);
        }



        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

            if (_isMouseOver || _isMouseDown) {
                if (_timer < 150) {
                    _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    _timer = 150;
                }
            } else {
                if (_timer > 0) {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    _timer = 0;
                }
            }
            _innerBar.Color = Color.Lerp(DefaultInnerColor, MouseMoveInnerColor, _timer / 150f);
            _innerBar.SizeFactor = new Vector2(MathHelper.Clamp(ViewSize, 0.01f, 1), 1f);
            _innerBar.RecalculateSelf();
            // 锚点和基准点都在顶部
            int topX = 0, bottomX = _outerBar.Width - _innerBar.Width;
            if (_isMouseDown) {
                var posLocal = _innerBar.ScreenPositionToParentAR(Main.MouseScreen + new Vector2(_offsetX, 0));
                float r = (posLocal.X - topX) / (bottomX - topX);
                if (float.IsNaN(r)) r = 0;
                CurrentValue = r;
            }



            var pos = new Vector2(MathHelper.Lerp(topX, bottomX, CurrentValue), 0);
            _innerBar.Position = pos;
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);

        }
    }
}
