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

        private const float FontHeight = 18;

        // private UILabel _label;
        private string _text;
        private int caret;
        private float _timer;
        private bool _shouldBlink;
        private int caretMove;


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
            OnClick += UITextArea_OnClick;
        }

        public override void DrawSelf(SpriteBatch sb) {
            string displayText = Text;
            if (IsFocused) {
                InputText();
                DrawIME();
                displayText = Text;
                caret = (int)MathHelper.Clamp(caret, 0, displayText.Length);
                // displayText = displayText.Insert(caret, _shouldBlink ? "|" : " ");
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
            #region CaretMove
            if (KeyDown(Keys.Left))
            {
                caret = Math.Max(0, caret - 1);
                caretMove++;
            }
            else if (KeyDown(Keys.Right))
            {
                caret = Math.Min(Text.Length, caret + 1);
                caretMove++;
            }
            else if (Main.keyState.IsKeyDown(Keys.Left) ^ Main.keyState.IsKeyDown(Keys.Right))
            {
                caretMove++;
                if (caretMove > 5 * 60)
                {
                    if (Main.keyState.IsKeyDown(Keys.Left))
                    {
                        caret = Math.Max(0, caret - 1);
                    }
                    else if (Main.keyState.IsKeyDown(Keys.Right))
                    {
                        caret = Math.Min(Text.Length, caret + 1);
                    }
                }
                else if (caretMove > 4 * 60)
                {
                    if (caretMove % 3 == 0)
                    {
                        if (Main.keyState.IsKeyDown(Keys.Left))
                        {
                            caret = Math.Max(0, caret - 1);
                        }
                        else if (Main.keyState.IsKeyDown(Keys.Right))
                        {
                            caret = Math.Min(Text.Length, caret + 1);
                        }
                    }
                }
                else if (caretMove > 2 * 60)
                {
                    if (caretMove % 6 == 0)
                    {
                        if (Main.keyState.IsKeyDown(Keys.Left))
                        {
                            caret = Math.Max(0, caret - 1);
                        }
                        else if (Main.keyState.IsKeyDown(Keys.Right))
                        {
                            caret = Math.Min(Text.Length, caret + 1);
                        }
                    }
                }
                else if (caretMove % 10 == 0)
                {
                    if (Main.keyState.IsKeyDown(Keys.Left))
                    {
                        caret = Math.Max(0, caret - 1);
                    }
                    else if (Main.keyState.IsKeyDown(Keys.Right))
                    {
                        caret = Math.Min(Text.Length, caret + 1);
                    }
                }
                return;
            }
            else
            {
                caretMove = 0;
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
        #region DrawTexts
        private void DrawTexts(string text)
        {
            var (texts, amount) = WrapWord(text);
            var lineHeight = FontHeight * TextScale;
            var height = amount * LinePadding + (amount + 1) * lineHeight;
            var fakeCaret = caret;
            Vector2 pos = default;
            if (TextAlign.HasFlag(Align.Top))
            {
                pos.Y = 0;
            }
            else if (TextAlign.HasFlag(Align.Bottom))
            {
                pos.Y = Height - height;
            }
            else
            {
                pos.Y = Height / 2 - height / 2;
            }
            int totalLen = 0;
            if (text.Length == 0)
            {
                texts[0] = _shouldBlink ? "|" : " ";
            }
            else if (text.Length - fakeCaret <= 1)
            {
                texts[amount] = texts[amount].Insert(texts[amount].Length - text.Length + fakeCaret, _shouldBlink ? "|" : " ");
            }
            else
            {
                for (int i = 0; i <= amount; i++)
                {
                    if (texts[i] == null)
                    {
                        continue;
                    }
                    if (fakeCaret <= texts[i].Length - 2)
                    {
                        texts[i] = texts[i].Insert(fakeCaret, _shouldBlink ? "|" : " ");
                        break;
                    }
                    totalLen += texts[i].Length - 2;
                    fakeCaret -= texts[i].Length - 2;
                    if (0 <= totalLen && totalLen < text.Length && text[totalLen] == '\n')
                    {
                        totalLen++;
                        fakeCaret--;
                    }
                }
            }
            for (int i = 0; i <= amount; i++)
            {
                if (string.IsNullOrEmpty(texts[i]))
                {
                    continue;
                }
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
        #region FindCaret
        private (int row, int column, int index) FindCaret(Vector2 mouseScreen)
        {
            if (Text.Length == 0)
            {
                return (0, 0, 0);
            }
            var (lines, amount) = WrapWord(Text);
            var relative = mouseScreen - PostionScreen + new Vector2(Width, Height) * Pivot;
            var lineHeight = FontHeight * TextScale;
            int row;
            if (amount > 0)
            {
                float top;
                #region Top
                switch (TextAlign)
                {
                    case Align.Top:
                    case Align.TopLeft:
                    case Align.TopRight:
                        top = 0f;
                        break;
                    case Align.Bottom:
                    case Align.BottomLeft:
                    case Align.BottomRight:
                        top = Height - amount * LinePadding + (amount + 1) * lineHeight;
                        break;
                    default:
                        top = Height / 2 - amount * LinePadding + (amount + 1) * lineHeight;
                        break;
                };
                #endregion
                row = (int)((relative.Y - top) / (lineHeight + LinePadding));
                row = (int)MathHelper.Clamp(row, 0, amount);
            }
            else
            {
                row = 0;
            }
            #region 列数一时半会儿还找不来
#if false
            int column;
            #region Search
            {
                float left;
                var Text = lines[row];
                int l = 0, r = Text.Length;
                int ans = r;
                switch (TextAlign)
                {
                    case Align.TopLeft:
                    case Align.Left:
                    case Align.BottomLeft:
                        left = 0;
                        break;
                    case Align.TopRight:
                    case Align.Right:
                    case Align.BottomRight:
                        left = Width - Main.fontMouseText.MeasureString(Text).X * TextScale;
                        break;
                    default:
                        left = Width / 2 - Main.fontMouseText.MeasureString(Text).X * TextScale / 2;
                        break;
                }
                Main.NewText(left);
                if (left + Main.fontMouseText.MeasureString(Text).X * TextScale < relative.X)
                {
                    column = Text.Length;
                }
                else
                {
                    while (l <= r)
                    {
                        int mid = (l + r) / 2;
                        if (left + Main.fontMouseText.MeasureString(Text.Substring(0, mid)).X * TextScale < relative.X)
                        {
                            l = mid + 1;
                            ans = mid;
                        }
                        else
                        {
                            r = mid - 1;
                        }
                    }
                    column = (int)MathHelper.Clamp(ans, 0, Text.Length);
                }
            }
            #endregion
            int index;
            if (row == 0)
            {
                index = inserted.IndexOf(lines[row]) + column;
            }
            else
            {
                int startIndex = 0;
                for (int i = 0; i < row; i++)
                {
                    startIndex += lines[i].Length;
                }
                index = inserted.IndexOf(lines[row], startIndex - row);
            }
            Main.NewText($"{row}, {column}, {index}");
            index = (int)MathHelper.Clamp(index, 0, Text.Length);
            return (row, column, index);
#endif
            #endregion
            int index;
            if (row == 0)
            {
                index = Text.IndexOf(lines[row]);
            }
            else
            {
                index = 0;
                for (int i = 0; i < row; i++)
                {
                    index += lines[i].Length - 2;
                    if (Text[index] == '\n')
                    {
                        index++;
                    }
                }
            }
            index = (int)MathHelper.Clamp(index, 0, this.Text.Length);
            return (row, 0, index);
        }
		#endregion

		private void UITextArea_OnClick(UIMouseEvent e, UIElement sender)
        {
            if (!IsFocused)
            {
                return;
            }
            var (row, column, index) = FindCaret(e.MouseScreen);
            caret = index;
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

        private (string[] texts, int amount) WrapWord(string text)
        {
            var height = FontHeight;
            var lines = Utils.WordwrapString(text, Main.fontMouseText, Width - 5, (int)Math.Ceiling(Height / height), out int amount);
            return (lines, amount);
        }
    }
}
