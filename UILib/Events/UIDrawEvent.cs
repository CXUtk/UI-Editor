using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Events {
    public class UIDrawEvent : UIEvent {
        public SpriteBatch SpriteBatch { get; }
        public UIDrawEvent(UIElement element, TimeSpan timestamp, SpriteBatch sb)
            : base(element, timestamp) {
            SpriteBatch = sb;
        }
    }
}
