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
using static UIEditor.UILib.Components.UITextArea;

namespace UIEditor.UILib.Components {

    /// <summary>
    /// 单行输入的文本框
    /// </summary>
    public class UITextBox : UIElement {
        private UILabel _label;
        private string _text;
        private int caret;
        private float _timer;
        private bool _shouldBlink;
        private float _offsetX;

        public string Text {
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
        public float TextScale {
            get => _label.TextScale;
            set => _label.TextScale = value;
        }
        public Color TextColor {
            get => _label.TextColor;
            set => _label.TextColor = value;
        }
        private static UIStateMachine StateMachine => UIEditor.Instance.UIStateMachine;
        public event TextChangeEvent OnTextChange;
        public UITextBox() : base() {
            Overflow = OverflowType.Hidden;
            BlockPropagation = true;
            _text = string.Empty;
            _shouldBlink = false;
            _label = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                NoEvent = true,
            };
            AppendChild(_label);
        }
        public override void FocusOn(UIActionEvent e) {
            _shouldBlink = true;
            _timer = 0;
            base.FocusOn(e);
        }
        public override void UnFocus(UIActionEvent e) {
            _shouldBlink = false;
            _timer = 0;
            caret = Text.Length;
            base.UnFocus(e);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            caret = (int)MathHelper.Clamp(caret, 0, Text.Length);
            if (IsFocused)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                // 显示文本光标，500ms闪烁一次
                if (_timer > 500)
                {
                    _shouldBlink ^= true;
                    _timer = 0;
                }
                _label.Text = Text.Insert(caret, _shouldBlink ? "|" : " ");
                _label.CalculateSize();
                //// 5像素的偏移是留给光标的
                //_offsetX = Math.Min(0, Width - 5f - _label.Width);
                //_label.Position = new Vector2(_offsetX, 0);
                _label.Recalculate();
            }
            else
            {
                if (_label.Text.Length != Text.Length)
                {
                    _label.Text = Text;
                }
            }
        }


        public override void DrawSelf(SpriteBatch sb) {
            if (IsFocused) {
                InputText();
                DrawIME();
            }
            base.DrawSelf(sb);
        }

        public void TextChange(UITextChangeEvent e) {

            OnTextChange?.Invoke(e, this);
        }

        private void InputText()
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
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
            else if (KeyDown(Keys.LeftControl) && KeyDown(Keys.X))
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
    }
}
