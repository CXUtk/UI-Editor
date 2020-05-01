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
    /// 支持多行输入的文本框
    /// </summary>
    public class UITextArea : UIElement {
        public delegate void KeyEvent(UIKeyEvent e, UIElement sender);
        public delegate void TextChangeEvent(UITextChangeEvent e, UIElement sender);

        // private UILabel _label;
        private string _text;
        private int caret;
        private float _timer;
        private bool _shouldBlink;

        /// <summary>
        /// 行间距
        /// </summary>
        public float LinePadding
        {
            get;
            set;
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    var e = new UITextChangeEvent(_text, value, this, Main._drawInterfaceGameTime.TotalGameTime);
                    TextChange(e);
                    if (!e.Cancel)
                    {
                        _text = e.NewString;
                        caret = _text.Length;
                    }
                }
            }
        }

        private static UIStateMachine StateMachine => UIEditor.Instance.UIStateMachine;

        public float TextScale {
            get;
            set;
        }

        public Color TextColor {
            get;
            set;
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

        public UITextArea() {
            BlockPropagation = true;
            TextColor = Color.White;
            TextScale = 1;
            _text = string.Empty;

        }

        public override void DrawSelf(SpriteBatch sb) {
            string displayText = Text;
            if (IsFocused) {
                InputText();
                DrawIME();
                displayText = Text;
                caret = (int)MathHelper.Clamp(caret, 0, displayText.Length);
                displayText = displayText.Insert(caret, _shouldBlink ? "|" : " ");
            }
            DrawTexts(displayText);
            base.DrawSelf(sb);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            // SyncLabel(); 
            if (IsFocused)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                // 显示文本光标，500ms闪烁一次
                if (_timer > 500)
                {
                    _shouldBlink ^= true;
                    _timer = 0;
                }
            }
        }

        public void TextChange(UITextChangeEvent e) {
            OnTextChange?.Invoke(e, this);
        }
        public void KeyInput(UIKeyEvent e) {
            OnKeyInput?.Invoke(e, this);
        }
		#region InputText
		private void InputText()
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            Main.editSign = true;
            Main.blockInput = false;
            #region KeyDown
            bool KeyDown(Keys key)
            {
                return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
            }
            #endregion
            if (KeyDown(Keys.End))
            {
                caret = Text.Length;
            }
            else if (KeyDown(Keys.Home))
            {
                caret = 0;
            }
            else if (KeyDown(Keys.Enter))
            {
                _text = _text.Insert(caret, "\n");
                caret++;
            }
            else if ((KeyDown(Keys.LeftControl) || KeyDown(Keys.RightControl)) && KeyDown(Keys.X))
            {
                Platform.Current.Clipboard = Text;
                Text = string.Empty;
            }
            else if (KeyDown(Keys.Left))
            {
                caret = Math.Max(0, caret - 1);
            }
            else if (KeyDown(Keys.Right))
            {
                caret = Math.Min(Text.Length, caret + 1);
            }
            else
            {
                if (caret == Text.Length)
                {
                    Text = Main.GetInputText(Text);
                }
                else
                {
                    var front = Text.Substring(0, caret);
                    var back = Text.Substring(caret);
                    var newString = Main.GetInputText(front);
                    if (front != newString)
                    {
                        Text = newString + back;
                        caret = newString.Length;
                    }
                }
            }
        }
		#endregion
		#region SyncLabel
#if false
        private void SyncLabel()
        {
            Vector2 anchor = new Vector2(0.5f, 0.5f);
            if (TextAlign.HasFlag(Align.Top))
            {
                anchor.Y = 0 + (float)_label.Height / Height / 2;
            }
            else if (TextAlign.HasFlag(Align.Bottom))
            {
                anchor.Y = 1 - (float)_label.Height / Height / 2;
            }
            if (TextAlign.HasFlag(Align.Left))
            {
                anchor.X = 0 + (float)_label.Width / Width / 2;
            }
            else if (TextAlign.HasFlag(Align.Right))
            {
                anchor.X = 1 - (float)_label.Width / Width / 2;
            }
            _label.AnchorPoint = anchor;
        }
#endif
		#endregion
		#region DrawTexts
        private void DrawTexts(string text)
        {
            var (texts, amount) = WrapWord(text);
            var lineHeight = 18 * TextScale;
            var height = amount * LinePadding + (amount + 1) * lineHeight;
            Vector2 pos = default;
            if (TextAlign.HasFlag(Align.Top))
            {
                pos.Y = 0;
            }
            else if(TextAlign.HasFlag(Align.Bottom))
            {
                pos.Y = Height - height;
            }
            else
            {
                pos.Y = Height / 2 - height / 2;
            }
            for (int i = 0; i <= amount; i++)
            {
                if (TextAlign.HasFlag(Align.Left))
                {
                    pos.X = 0;
                }
                else if (TextAlign.HasFlag(Align.Right))
                {
                    pos.X = Width - Main.fontMouseText.MeasureString(texts[i]).X * TextScale;
                }
                else
                {
                    pos.X = Width / 2 - Main.fontMouseText.MeasureString(texts[i]).X * TextScale / 2;
                }
                Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, texts[i], pos.X, pos.Y, TextColor, Color.Black, Vector2.Zero, TextScale);
                pos.Y += lineHeight + LinePadding;
            }
        }
		#endregion
		private void DrawIME() {
            var pos = PostionScreen;
            var size = GetIMESize();
            if (pos.Y + Height + size.Y > Main.screenHeight) {
                pos.Y -= 6 + size.Y;
            } else {
                pos.Y += Height + 6 + size.Y;
            }
            pos.X += size.X + 10;
            StateMachine.SetIME(pos);
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

        private (string[] texts, int amount) WrapWord(string text)
        {
            var height = Main.fontMouseText.MeasureString("H").Y;
            var lines = Utils.WordwrapString(text, Main.fontMouseText, Width, (int)Math.Ceiling(Height / height), out int amount);
            return (lines, amount);
        }
    }
}
