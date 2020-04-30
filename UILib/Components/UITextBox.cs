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
        public Texture2D FrameTexture { get; set; }
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
        private float _timer;
        private bool _shouldBlink;
        private string _realText;
        private float _offsetX;
        private static UIStateMachine StateMachine => UIEditor.Instance.UIStateMachine;
        public event TextChangeEvent OnTextChange;
        public UITextBox() : base() {
            Overflow = OverflowType.Hidden;
            BlockPropagation = true;
            _shouldBlink = false;
            FrameTexture = Drawing.DefaultBoxTexture;
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
            base.UnFocus(e);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (IsFocused) {
                _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                // 显示文本光标，500ms闪烁一次
                if (_timer > 500) {
                    _shouldBlink ^= true;
                    _timer = 0;
                }
            }
        }


        public override void DrawSelf(SpriteBatch sb) {
            if (IsFocused) {
                PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                string oldString = _realText;
                var newString = Main.GetInputText(oldString);
                if (oldString != newString) {
                    var e = new UITextChangeEvent(oldString, newString, this, Main._drawInterfaceGameTime.TotalGameTime);
                    TextChange(e);
                    if (!e.Cancel) {
                        _realText = newString;
                    }
                }

                _label.CalculateSize();

                // 10像素的偏移是留给光标的
                _offsetX = Math.Min(0, Width - 10 - 10f * TextScale - _label.MeasureSize(_realText).X);
                _label.Position = new Vector2(_offsetX + 10, 0);
                _label.Recalculate();
                DrawIME();
            }
            Text = _realText + (_shouldBlink ? "|" : "");
            Drawing.DrawAdvBox(sb, 0, 0, Width, Height, Color.White, FrameTexture, new Vector2(8, 8));
            base.DrawSelf(sb);
        }

        public void TextChange(UITextChangeEvent e) {
            OnTextChange?.Invoke(e, this);
        }
        private void DrawIME() {
            var pos = _label.InnerRectangleScreen.BottomRight();
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
