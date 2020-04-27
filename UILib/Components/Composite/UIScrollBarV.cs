using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;

namespace UIEditor.UILib.Components {
    public class UIScrollBarV : UIElement {
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
        private float _offsetY;
        private float _timer;

        private const float PADDING = 5;
        public UIScrollBarV() : base() {
            BlockPropagation = true;
            SizeFactor = new Vector2(0, 1);
            Size = new Vector2(20, -PADDING * 2);
            _currentValue = 0;
            _isMouseOver = false;
            _outerBar = new UIBar() {
                Texture = TextureManager.Load("Images/UI/ScrollbarInner"),
                SizeFactor = new Vector2(1f, 1f),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                EndSize = 6
            };
            _innerBar = new UIBar() {
                Name = "Inner",
                Texture = TextureManager.Load("Images/UI/ScrollbarInner"),
                SizeFactor = new Vector2(1f, 0.5f),
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0),
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
            _offsetY = _innerBar.PostionScreen.Y - e.MouseScreen.Y;
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
            // 锚点和基准点都在顶部
            int topY = 0, bottomY = _outerBar.Height - _innerBar.Height;
            if (_isMouseDown) {
                var posLocal = _innerBar.ScreenPositionToNode(Main.MouseScreen + new Vector2(0, _offsetY));
                CurrentValue = (posLocal.Y - topY) / (float)(bottomY - topY);
            }

            _innerBar.SizeFactor = new Vector2(1f, ViewSize);
            _innerBar.RecalculateSelf();
            _outerBar.RecalculateSelf();


            var pos = new Vector2(0, MathHelper.Lerp(topY, bottomY, CurrentValue));
            _innerBar.Position = pos;
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);

        }
    }
}
