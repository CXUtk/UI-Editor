using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Events {
    /// <summary>
    /// 储存鼠标事件
    /// </summary>
    public class UIDraggingEvent : UIEvent {
        public Vector2 Moved { get; }
        public UIDraggingEvent(UIElement source, Vector2 moved, TimeSpan timestamp)
            : base(source, timestamp) {
            Moved = moved;
        }
    }
}
