﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib {
    public class UIState : UIElement, IComparable {
        internal long TimeGetFocus { get; set; }
        internal float ZIndex { get; set; }
        internal UIStateMachine UIStateMachine { get; set; }
        public UIState(string name) : base() {
            Pivot = new Vector2(0, 0);
            SizeFactor = new Vector2(1, 1);
            IsVisible = false;
            ZIndex = 0f;
            Name = name;

            Initialize();
            Recalculate();
        }

        public int CompareTo(object obj) {
            var other = obj as UIState;
            if (ZIndex != other.ZIndex) return ZIndex.CompareTo(other.ZIndex);
            return TimeGetFocus.CompareTo(other.TimeGetFocus);
        }

        public virtual void Initialize() { }
    }
}
