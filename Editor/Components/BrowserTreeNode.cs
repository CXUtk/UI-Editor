using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib;
using Microsoft.Xna.Framework;

namespace UIEditor.Editor.Components {
    public class BrowserTreeNode : UITreeNodeDisplay {
        public UIElement BindingElement { get; }
        public BrowserTreeNode(string text, UIElement element) : base(text) {
            BindingElement = element;
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            this.Text = BindingElement.Name;
        }
    }
}
