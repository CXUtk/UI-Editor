﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UIEditor.UILib.Events;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;

namespace UIEditor.UILib.Components {

    /// <summary>
    /// 只支持单行的输入框
    /// </summary>
    public class UITextBox : UIElement {
        public delegate void KeyEvent(UIKeyEvent e, UIElement sender);
        public delegate void TextChangeEvent(UIActionEvent e, UIElement sender);


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
                NoEvent = true
            };

            this.AppendChild(_label);
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            if (IsFocused) {
                PlayerInput.WritingText = true;
                //Main.blockInput = true;
                Main.instance.HandleIME();
                string oldString = Text;
                var newString = Main.GetInputText(oldString);
                Text = newString;
            }
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }

        public void KeyInput(UIKeyEvent e) {
            OnKeyInput?.Invoke(e, this);
        }
        //public void KeyDown(UIKeyEvent e)
        //{
        //  OnKeyDown?.Invoke(e, this);
        //}
        //public void KeyUp(UIKeyEvent e)
        //{
        //  OnKeyUp?.Invoke(e, this);
        //}
        //public void KeyPress(UIKeyEvent e)
        //{
        //  OnKeyPress?.Invoke(e, this);
        //}


    }
}
