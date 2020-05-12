using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;
using Terraria;

namespace UIEditor.UILib.Tests {
    public class TestState : UIState {
        public TestState(string name) : base(name) { }
        public override void Initialize() {
            base.Initialize();

        }

        private void Box1_OnClose(Events.UIActionEvent e, UIElement sender) {
            this.IsActive = false;
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            //var box1 = GetChildByName("a");
            //box1.Rotation -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //box1.GetChildByName("label").Rotation += 3.14f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
