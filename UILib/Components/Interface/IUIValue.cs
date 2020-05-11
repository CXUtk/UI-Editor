using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Components.Interface {
    interface IUIValue<T> {
        T Value { get; set; }
    }
}
