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
    public class UIDragEndEvent : UIEvent {
        public Vector2 MouseScreen { get; }
        public UIElement TargetElement { get; }
        public UIDragEndEvent(UIElement source, UIElement target, TimeSpan timestamp, Vector2 mouseScreen)
            : base(source, timestamp) {
            MouseScreen = mouseScreen;
            TargetElement = target;
        }
    }
}
