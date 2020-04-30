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
    public class TestState2 : UIState {
        public TestState2(string name) : base(name) { }
        public override void Initialize() {
            base.Initialize();
            var button = new UIButton()
            {
                Name = "tbDebug",
                Size = new Vector2(10 * 16, 3 * 16),
                Position = new Vector2(10 * 16, 3 * 16) / 2
            };
            var box13 = new UIWindow() {
                Name = "a",
                Size = new Vector2(800, 640),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Position = new Vector2(100, 100)
            };
            var textbox = new UITextBox
            {
                Name = "emmm",
                Size = new Vector2(20 * 16, 3 * 16),
                Position = new Vector2(0, 300),
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0),
                TextAlign = Enums.Align.TopLeft
            };
            var checkbox = new UICheckBox
            {
                Name = "test checkbox",
                Position = new Vector2(70, 70)
            };
            button.OnClick += (e, sender) =>
            {
                switch(textbox.TextAlign)
                {
                    case Enums.Align.Top:
                        textbox.TextAlign = Enums.Align.TopRight;
                        break;
                    case Enums.Align.TopRight:
                        textbox.TextAlign = Enums.Align.Right;
                        break;
                    case Enums.Align.Right:
                        textbox.TextAlign = Enums.Align.BottomRight;
                        break;
                    case Enums.Align.BottomRight:
                        textbox.TextAlign = Enums.Align.Bottom;
                        break;
                    case Enums.Align.Bottom:
                        textbox.TextAlign = Enums.Align.BottomLeft;
                        break;
                    case Enums.Align.BottomLeft:
                        textbox.TextAlign = Enums.Align.Left;
                        break;
                    case Enums.Align.Left:
                        textbox.TextAlign = Enums.Align.TopLeft;
                        break;
                    case Enums.Align.TopLeft:
                        textbox.TextAlign = Enums.Align.Top;
                        break;
                }
                button.Text = textbox.TextAlign.ToString();
            };
            box13.OnClose += Box1_OnClose;
            AppendChild(box13);
            box13.AppendChild(textbox);
            box13.AppendChild(button);
            box13.AppendChild(checkbox);
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
