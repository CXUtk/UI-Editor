﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Events {
    /// <summary>
    /// 储存UI事件的信息
    /// </summary>
    public abstract class UIEvent {
        public UIElement Target { get; }
        public TimeSpan TimeStamp { get; }
        public UIEvent(UIElement target, TimeSpan timestamp) {
            Target = target;
            TimeStamp = timestamp;
        }
    }
}
