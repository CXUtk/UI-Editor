using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Components {
    public class UIViewPort : UIElement {
        public UIViewPort() : base() {
            Overflow = OverflowType.Hidden;
        }
        public override void DrawChildren(SpriteBatch sb) {
            // 不绘制出界元素来减少性能损耗
            foreach (var child in Children) {
                if (child.IsActive && child.InnerRectangleScreen.Intersects(InnerRectangleScreen)) {
                    child.Draw(sb);
                }
            }
        }
    }
}
