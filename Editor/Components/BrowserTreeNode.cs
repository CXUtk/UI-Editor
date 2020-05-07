using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib;

namespace UIEditor.Editor.Components {
    public class BrowserTreeNode : UITreeNodeDisplay {
        public UIElement BindingElement { get; }
        public BrowserTreeNode(string text, UIElement element) : base(text) {
            BindingElement = element;
        }
    }
}
