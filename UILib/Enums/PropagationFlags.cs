using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Enums {
    [Flags]
    public enum PropagationFlags : int {
        MouseLeftEvents = MouseLeftDown | MouseLeftClick | MouseLeftUp | MouseLeftDouble,
        FocusEvents = FocusOn | UnFocus,
        MouseMoveEvents = MouseEnter | MouseOut,
        MouseRightEvents = MouseRightDown | MouseRightDown | MouseRightClick,
        BLOCK_ALL = 0,
        PASS_ALL = -1,
        MouseLeftDown = 0x1,
        MouseLeftUp = 0x2,
        MouseLeftClick = 0x4,
        MouseEnter = 0x8,
        MouseOut = 0x10,
        MouseRightDown = 0x20,
        MouseRightUp = 0x40,
        MouseRightClick = 0x80,
        MouseLeftDouble = 0x100,
        ScrollWheel = 0x200,
        FocusOn = 0x400,
        UnFocus = 0x800,
        DragStart = 0x1000,
        DragEnd = 0x2000,
    }
}
