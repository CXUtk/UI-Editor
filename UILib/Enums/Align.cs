using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIEditor.UILib.Enums {
    [Flags]
    public enum Align : byte {
        Center = 0,
        Top = 0b0000_0001,
        Bottom = 0b0000_0010,
        Left = 0b0000_0100,
        Right = 0b0000_1000,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
    }
}
