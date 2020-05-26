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
using UIEditor.Editor.Attributes;

namespace UIEditor.UILib.Components {

    /// <summary>
    /// 单行输入的文本框
    /// </summary>
    [EditorPropertyNoChildren]
    public class UITextBox : UIPanel {
        /// <summary>
        /// 这个文本组件可不可以被用户编辑？
        /// </summary>
        public bool Editable { get; set; }

        public bool DrawPanel { get; set; }
        private readonly UILabel _label;
        protected string _text = "";
        private int _carrot;
        private float _timer;
        private bool _shouldBlink;
        private float _offsetR, _offsetL;
        public string Text {
            get {
                return _text;
            }
            set {
                if (_text != value) {

                    var e = new UITextChangeEvent(_text, value, this, new TimeSpan());
                    TextChange(e);
                    if (!e.Cancel) {
                        _text = e.NewString;
                        _carrot = _text.Length;
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
            Editable = true;
            Name = "文本框";
            Overflow = OverflowType.Hidden;
            _text = "";
            _shouldBlink = false;
            PanelTexture = UIEditor.Instance.SkinManager.GetTexture("Box_Default");
            _label = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                NoEvent = true,
                Position = new Vector2(5f, 0f),
            };
            _offsetL = _offsetR = 0;
            OnClick += FindCarrot;
            DrawPanel = true;
            AppendChild(_label);
        }

        private void FindCarrot(UIMouseEvent e, UIElement sender) {
            var localMousePos = _label.ScreenPositionToNodeAR(e.MouseScreen, Vector2.Zero);
            int l = 0, r = Text.Length;
            int ans = r;
            while (l <= r) {
                int mid = (l + r) / 2;
                if (_label.Position.X + _label.MeasureSize(Text.Substring(0, mid)).X < localMousePos.X) {
                    l = mid + 1;
                    ans = mid;
                } else {
                    r = mid - 1;
                }
            }
            _carrot = ans;
        }

        public override void FocusOn(UIActionEvent e) {
            if (!Editable) return;
            PanelBorderTexture = UIEditor.Instance.SkinManager.GetTexture("BoxFrame_Default");
            _shouldBlink = true;
            _timer = 0;
            base.FocusOn(e);
        }
        public override void UnFocus(UIActionEvent e) {
            if (!Editable) return;
            PanelBorderTexture = null;
            _shouldBlink = false;
            _timer = 0;
            // Carrot = Text.Length;
            base.UnFocus(e);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _carrot = (int)MathHelper.Clamp(_carrot, 0, Text.Length);
            if (IsFocused) {
                _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                // 显示文本光标，500ms闪烁一次
                if (_timer > 500) {
                    _shouldBlink ^= true;
                    _timer = 0;
                }
                _label.Text = Text.Insert(_carrot, _shouldBlink ? "|" : " ");
                _label.CalculateSize();
            } else {
                _label.Text = Text;

            }
        }


        public override void DrawSelf(SpriteBatch sb) {
            if (IsFocused && Editable) {
                InputText();
                // 10像素的偏移是留给光标的
                float carrotpos = _label.MeasureSize(Text.Substring(0, _carrot)).X;
                if (carrotpos >= _offsetR - 10f * TextScale) {
                    _offsetR = carrotpos + 10f * TextScale;
                    _offsetL = _offsetR - Width;
                } else if (carrotpos <= _offsetL + 5f * TextScale) {
                    _offsetL = carrotpos - 5f * TextScale;
                    _offsetR = _offsetL + Width;
                }
                _label.Position = new Vector2(Math.Min(5, Width - _offsetR), 0);
                _label.Recalculate();
                DrawIME();

            }
            if (DrawPanel)
                base.DrawSelf(sb);
        }

        public virtual void TextChange(UITextChangeEvent e) {
            OnTextChange?.Invoke(e, this);
        }
        private bool KeyDown(Keys key) {
            return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
        }
        private void InputText() {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            if (KeyDown(Keys.End)) {
                _carrot = Text.Length;
            } else if (KeyDown(Keys.Home)) {
                _carrot = 0;
            } else if (KeyDown(Keys.LeftControl) && KeyDown(Keys.X)) {
                Platform.Current.Clipboard = Text;
                Text = string.Empty;
            } else if (KeyDown(Keys.Left)) {
                _carrot = Math.Max(0, _carrot - 1);
            } else if (KeyDown(Keys.Right)) {
                _carrot = Math.Min(Text.Length, _carrot + 1);
            } else {
                if (_carrot == Text.Length) {
                    Text = Main.GetInputText(Text);
                } else {
                    var front = Text.Substring(0, _carrot);
                    var back = Text.Substring(_carrot);
                    var newString = Main.GetInputText(front);
                    if (front != newString) {
                        Text = newString + back;
                        _carrot = newString.Length;
                    }
                }
            }
        }

        private void DrawIME() {
            var pos = _label.BaseRectangleScreen.BottomLeft() + new Vector2(_label.MeasureSize(Text.Substring(0, _carrot)).X, 0);
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
