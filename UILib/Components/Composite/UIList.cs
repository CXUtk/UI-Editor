using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;

namespace UIEditor.UILib.Components.Composite {
    public class UIList : UIElement {
        public float MarginBetweenItems { get; set; }

        private UIScrollBarV _verticalScrollBar;
        // private UIScrollBarV _horizontalScrollBar;
        private List<UIElement> _elements;
        private UIViewPort _viewPort;
        private float _totHeight;


        private const float PADDING = 10f;
        public UIList() : base() {
            Overflow = OverflowType.Hidden;
            _viewPort = new UIViewPort() {
                AnchorPoint = new Vector2(0f, 0f),
                SizeFactor = new Vector2(1, 1),
                Pivot = new Vector2(0f, 0f),
            };
            _elements = new List<UIElement>();
            MarginBetweenItems = 5f;
            this.AppendChild(_viewPort);
            OnScrollWheel += UIList_OnScrollWheel;

        }

        private void UIList_OnScrollWheel(Events.UIScrollWheelEvent e, UIElement sender) {
            _verticalScrollBar.CurrentValue -= e.ScrollValue / (_totHeight - Height);
        }

        public void SetScrollBarV(UIScrollBarV scrollBarV) {
            if (_verticalScrollBar != null) {
                _verticalScrollBar.Parent = null;
                Children.Remove(_verticalScrollBar);
            }
            _verticalScrollBar = scrollBarV;
            _verticalScrollBar.AnchorPoint = new Vector2(1, 0.5f);
            _verticalScrollBar.Pivot = new Vector2(1, 0.5f);
            _verticalScrollBar.Position = new Vector2(-5, 0);
            AppendChild(_verticalScrollBar);
        }

        private void UpdateScrollBarV() {
            if (_verticalScrollBar == null) return;
            _viewPort.Size = new Vector2(-_verticalScrollBar.Width - 5f, 0f);
            _verticalScrollBar.ViewSize = Height / (_totHeight - Height);
            float offset = _verticalScrollBar.CurrentValue * (_totHeight - Height);
            foreach (var element in _elements) {
                float y = element.Position.Y;
                element.Position = new Vector2(element.Position.X, y - offset);
            }
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _totHeight = PADDING;
            foreach (var element in _elements) {
                element.Position = new Vector2(0, _totHeight);
                _totHeight += element.Height + MarginBetweenItems;
            }
            UpdateScrollBarV();
        }

        public void AddElement(UIElement element) {
            element.Pivot = new Vector2(0f, 0f);
            element.AnchorPoint = new Vector2(0f, 0f);
            _elements.Add(element);
            _viewPort.AppendChild(element);
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
        }


    }
}
