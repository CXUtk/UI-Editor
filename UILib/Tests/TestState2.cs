using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UIEditor.Editor.Components;

namespace UIEditor.UILib.Tests {
    public class TestState2 : UIState {
        public TestState2(string name) : base(name) { }
        public override void Initialize() {
            base.Initialize();
            var button = new UIButton() {
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
            var textbox = new UITextArea {
                Name = "emmm",
                Size = new Vector2(20 * 16, 6.5f * 16),
                Position = new Vector2(0, 300),
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0),
                TextAlign = Enums.Align.TopLeft
            };
            var menu = new UIMenu() {
                Name = "text menu",
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0),
            };
            var checkbox = new UICheckBox {
                Tooltip = "Debug模式",
                Name = "test checkbox",
                Position = new Vector2(90, 90)
            };
            var colorwheel = new UIColorBar() {
                Position = new Vector2(200, 200),
                Size = new Vector2(128, 128),
            };
            var selector = new UIEnumSelector<TypeCode>() {
                Position = new Vector2(140, 280),
                Size = new Vector2(100, 30),
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0)
            };
            selector.HangElement = box13;

            checkbox.OnCheckedChange += Checkbox_OnCheckedChange;
            button.OnClick += (e, sender) => {
                switch (textbox.TextAlign) {
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
            {
                var item1 = new UIMenuItem();
                item1.Text = nameof(item1);
                item1.AddItem(new UIMenuItem { Text = "item1.item1" });
                item1.AddItem(new UIMenuItem { Text = "item1.item2" });
                item1.AddItem(new UIMenuItem { Text = "item1.item3" });
                menu.AddItem(item1);
                var item2 = new UIMenuItem();
                item2.Text = nameof(item2);
                item2.AddItem(new UIMenuItem { Text = "item2.item1" });
                item2.AddItem(new UIMenuItem { Text = "item2.item2" });
                menu.AddItem(item2);
                {
                    var item2_3 = new UIMenuItem();
                    item2_3.Text = "item2.item3";
                    item2_3.AddItem(new UIMenuItem { Text = "item231" });
                    item2_3.AddItem(new UIMenuItem { Text = "item232" });
                    item2_3.AddItem(new UIMenuItem { Text = "item233" });
                    item2_3.AddItem(new UIMenuItem { Text = "item234" });
                    item2.AddItem(item2_3);
                }
            }

            var scrollBarH = new UIScrollBarH() {
                AnchorPoint = new Vector2(0.5f, 1),
                Pivot = new Vector2(0.5f, 1),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 20),
            };
            box13.AppendChild(scrollBarH);
            box13.AppendChild(button);
            box13.AppendChild(checkbox);
            //box13.AppendChild(button2);
            box13.AppendChild(menu);
            box13.AppendChild(selector);
            //box13.AppendChild(colorwheel);
            //UIValueSlider slider = new UIValueSlider() {
            //    Position = new Vector2(400, 300),
            //    Size = new Vector2(230, 30),
            //    Min = 0,
            //    Max = 255,
            //};
            //box13.AppendChild(slider);
        }

        private void Checkbox_OnCheckedChange(Events.UICheckBoxEvent e, UIElement sender) {
            UIElement.DEBUG_MODE = !e.Value;

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
