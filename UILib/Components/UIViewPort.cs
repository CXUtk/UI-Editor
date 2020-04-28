using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Components {
    public class UIListViewPort : UIElement {
        private int _listUpMost;
        private int _listBottomMost;
        public UIListViewPort() : base() {
            Overflow = OverflowType.Hidden;
        }

        public override void UpdateChildren(GameTime gameTime) {
            if (Children.Count == 0) return;
            int upper = InnerRectangleScreen.Y;
            int lower = InnerRectangleScreen.Y + InnerRectangleScreen.Height;
            int L = 0, R = Children.Count - 1;
            _listUpMost = 0;
            _listBottomMost = 0;
            while (L <= R) {
                int mid = (L + R) / 2;
                var child = Children[mid];
                if (upper <= child.InnerRectangleScreen.Bottom) {
                    _listUpMost = mid;
                    R = mid - 1;
                } else {
                    L = mid + 1;
                }
            }
            L = 0;
            R = Children.Count - 1;
            while (L <= R) {
                int mid = (L + R) / 2;
                var child = Children[mid];
                if (lower >= child.InnerRectangleScreen.Top) {
                    _listBottomMost = mid;
                    L = mid + 1;
                } else {
                    R = mid - 1;
                }
            }
            // 不绘制出界元素来减少性能损耗
            for (int i = _listUpMost; i <= _listBottomMost; i++) {
                var child = Children[i];
                child.Update(gameTime);
            }
        }

        public override void DrawChildren(SpriteBatch sb) {
            if (Children.Count == 0) return;

            // 不绘制出界元素来减少性能损耗
            for (int i = _listUpMost; i <= _listBottomMost; i++) {
                var child = Children[i];
                child.Draw(sb);
            }
        }
    }
}
