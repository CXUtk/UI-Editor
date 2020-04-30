using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UIEditor.UILib.Events;
using UIEditor.UILib.Enums;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;
using ReLogic.OS;

namespace UIEditor.UILib.Components {

    /// <summary>
    /// 只支持单行的输入框
    /// </summary>
    public class UITextBox : UIElement {
        public delegate void KeyEvent(UIKeyEvent e, UIElement sender);
        public delegate void TextChangeEvent(UITextChangeEvent e, UIElement sender);


        private UILabel _label;

        private static UIStateMachine StateMachine => UIEditor.Instance.UIStateMachine;

        public string Text {
            get => _label.Text;
            set => _label.Text = value;
        }

        public float TextScale {
            get => _label.TextScale;
            set => _label.TextScale = value;
        }

        public Color TextColor {
            get => _label.TextColor;
            set => _label.TextColor = value;
        }

        public Align TextAlign {
            get;
            set;
        }

        //public event KeyEvent OnKeyDown;
        //public event KeyEvent OnKeyUp;
        //public event KeyEvent OnKeyPress;
        public event KeyEvent OnKeyInput;
        public event TextChangeEvent OnTextChange;

        public UITextBox() {
            BlockPropagation = true;
            _label = new UILabel() {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Pivot = new Vector2(0.5f, 0.5f),
                NoEvent = true,
            };

            this.AppendChild(_label);
        }

        public override void DrawSelf(SpriteBatch sb) {
            if (IsFocused) {
                PlayerInput.WritingText = true;
                //Main.blockInput = true;
                Main.instance.HandleIME();
                string oldString = Text;
                var newString = Main.GetInputText(oldString);
                if (oldString != newString) {
                    var e = new UITextChangeEvent(oldString, newString, this, Main._drawInterfaceGameTime.TotalGameTime);
                    TextChange(e);
                    if (!e.Cancel) {
                        Text = newString;
                    }
                }
            }
            base.DrawSelf(sb);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            SyncLabel();
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            if (IsFocused) {
                DrawIME();
            }
        }

        public void TextChange(UITextChangeEvent e) {
            OnTextChange?.Invoke(e, this);
        }
        public void KeyInput(UIKeyEvent e) {
            OnKeyInput?.Invoke(e, this);
        }

        private void SyncLabel() {
            Vector2 anchor = new Vector2(0.5f, 0.5f);
            if (TextAlign.HasFlag(Align.Top)) {
                anchor.Y = 0 + (float)_label.Height / Height / 2;
            } else if (TextAlign.HasFlag(Align.Bottom)) {
                anchor.Y = 1 - (float)_label.Height / Height / 2;
            }
            if (TextAlign.HasFlag(Align.Left)) {
                anchor.X = 0 + (float)_label.Width / Width / 2;
            } else if (TextAlign.HasFlag(Align.Right)) {
                anchor.X = 1 - (float)_label.Width / Width / 2;
            }
            _label.AnchorPoint = anchor;
        }

        private void DrawIME() {
            var pos = PostionScreen;
            var size = GetIMESize();
            if (pos.Y + Height + size.Y > Main.screenHeight) {
                pos.Y -= 6 + size.Y;
            } else {
                pos.Y += Height + 6 + size.Y;
            }
            pos.X += size.X + 10;
            Main.instance.DrawWindowsIMEPanel(pos);
        }

        private Vector2 GetIMESize() {
            List<TextSnippet> list = ChatManager.ParseMessage(Text, Color.White);
            string compositionString = Platform.Current.Ime.CompositionString;
            if (compositionString != null && compositionString.Length > 0) {
                list.Add(new TextSnippet(compositionString, new Color(255, 240, 20)));
            }
            if (Main.instance.textBlinkerState == 1) {
                list.Add(new TextSnippet("|", Color.White));
            }
            return ChatManager.GetStringSize(Main.fontMouseText, list.ToArray(), Vector2.Zero);
        }

    }
}
