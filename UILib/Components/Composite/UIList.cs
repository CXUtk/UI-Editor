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
        public float ItemMargin {
            get { return _itemMargin; }
            set { base.CheckRecalculate(_itemMargin, value); _itemMargin = value; }
        }
        public float InnerContainerPadding {
            get { return _innerContainerPadding; }
            set { base.CheckRecalculate(_innerContainerPadding, value); _innerContainerPadding = value; }
        }
        private float _itemMargin;
        private float _innerContainerPadding;

        private UIScrollBarV _verticalScrollBar;
        // private UIScrollBarV _horizontalScrollBar;
        protected IList<UIElement> _elements;
        protected UIListViewPort _viewPort;
        protected float _totHeight;
        private int _listUpMost;
        private int _listBottomMost;


        public UIList() : base() {
            Overflow = OverflowType.Hidden;
            _viewPort = new UIListViewPort() {
                AnchorPoint = new Vector2(0f, 0f),
                SizeFactor = new Vector2(1, 1),
                Pivot = new Vector2(0f, 0f),
            };
            _elements = new List<UIElement>();
            ItemMargin = 5f;
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
            ShouldRecalculate = true;
        }

        private void UpdateScrollBarV() {
            if (_verticalScrollBar == null) return;
            _viewPort.Size = new Vector2(-_verticalScrollBar.Width - 5f, 0f);

            var totHeight = Math.Max(_totHeight - Height, 0);
            _verticalScrollBar.ViewSize = Height / totHeight;
            float offset = _verticalScrollBar.CurrentValue * totHeight;
            foreach (var element in _elements) {
                float y = element.Position.Y;
                element.Position = new Vector2(element.Position.X, y - offset);
            }

        }

        public virtual void UpdateElementPos(GameTime gameTime) {
            _totHeight = InnerContainerPadding;
            foreach (var element in _elements) {
                element.Position = new Vector2(0, _totHeight);
                _totHeight += element.Height + ItemMargin;
            }
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            UpdateElementPos(gameTime);
            UpdateScrollBarV();
            PlaceChildren();
        }

        private void PlaceChildren() {
            _viewPort.RemoveAll();
            if (_elements.Count == 0) return;
            int upper = 0;
            int lower = Height;
            int L = 0, R = _elements.Count - 1;
            _listUpMost = 0;
            _listBottomMost = 0;
            while (L <= R) {
                int mid = (L + R) / 2;
                var child = _elements[mid];
                if (upper <= child.Position.Y + child.Height) {
                    _listUpMost = mid;
                    R = mid - 1;
                } else {
                    L = mid + 1;
                }
            }
            L = 0;
            R = _elements.Count - 1;
            while (L <= R) {
                int mid = (L + R) / 2;
                var child = _elements[mid];
                if (lower >= child.Position.Y) {
                    _listBottomMost = mid;
                    L = mid + 1;
                } else {
                    R = mid - 1;
                }
            }
            for (int i = _listUpMost; i <= _listBottomMost; i++) {
                _viewPort.AppendChild(_elements[i]);
            }
            ShouldRecalculate = true;
        }
        public void Clear() {
            _elements.Clear();
        }
        public virtual void AddElement(UIElement element) {
            element.Pivot = new Vector2(0f, 0f);
            element.AnchorPoint = new Vector2(0f, 0f);
            _elements.Add(element);
        }


    }
}
