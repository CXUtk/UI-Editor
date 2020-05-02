using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Components {
    public class UIListViewPort : UIPanel {
        public UIListViewPort() : base() {
            Overflow = OverflowType.Hidden;
            PanelTexture = Drawing.DefaultBoxTexture;
        }
        public void RemoveAll() {
            Children.Clear();
        }
    }
}
