using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UIEditor.UILib.Events;
using Terraria.GameInput;

namespace UIEditor.UILib.Components
{
	public class UITextBox : UIElement
	{
		public delegate void KeyEvent(UIKeyEvent e, UIElement sender);

		private UILabel _label;

		private static UIStateMachine StateMachine => UIEditor.Instance.UIStateMachine;


		public bool AllowMultiline
		{
			get;
			set;
		}

		public string Text
		{
			get => _label.Text;
			set => _label.Text = value;
		}

		public float TextScale
		{
			get => _label.TextScale;
			set => _label.TextScale = value;
		}

		public Color TextColor
		{
			get => _label.TextColor;
			set => _label.TextColor = value;
		}

		public bool Focused
		{
			get;
			private set;
		}

		//public event KeyEvent OnKeyDown;
		//public event KeyEvent OnKeyUp;
		//public event KeyEvent OnKeyPress;
		public event KeyEvent OnKeyInput;

		public UITextBox()
		{
			BlockPropagation = true;
			_label = new UILabel()
			{
				AnchorPoint = new Vector2(0.5f, 0.5f),
				Pivot = new Vector2(0.5f, 0.5f),
				NoEvent = true
			};

			this.AppendChild(_label);

			OnClick += HandleClick;
		}


		public override void UpdateSelf(GameTime gameTime)
		{
			base.UpdateSelf(gameTime);

			if (Focused)
			{
				var keys = Main.keyState.GetPressedKeys();
				if (keys.Length == 1)
				{
					var key = keys[0];
					var e = new UIKeyEvent(this, gameTime.TotalGameTime, key);
					KeyInput(e);
					if (e.Handled)
					{
						return;
					}
					if (key == Keys.Enter && !Main.oldKeyState.IsKeyDown(Keys.Enter) && AllowMultiline)
					{
						Text += "\n";
					}
					else if (key == Keys.Tab && !Main.oldKeyState.IsKeyDown(Keys.Tab))
					{
						Text += "    ";
					}
					else if (key == Keys.Escape && !Main.oldKeyState.IsKeyDown(Keys.Escape))
					{
						UnFocus();
						PlayerInput.WritingText = true;
						Main.blockInput = false;
						return;
					}
				}
				else if (keys.Length == 2)
				{
					bool now = Main.keyState.IsKeyDown(Keys.LeftAlt) && Main.keyState.IsKeyDown(Keys.C);
					bool old = Main.oldKeyState.IsKeyDown(Keys.LeftAlt) && Main.oldKeyState.IsKeyDown(Keys.C);
					if (!old && now)
					{
						Text = string.Empty;
					}
				}
				else
				{
					InputText();
				}
				if (Main.mouseLeft && !ScreenHitBox.Contains(Main.MouseScreen))
				{
					UnFocus();
				}
			}
		}

		public void Focus()
		{
			Focused = true;
		}

		public void UnFocus()
		{
			Focused = false;
		}

		public void KeyInput(UIKeyEvent e)
		{
			OnKeyInput?.Invoke(e, this);
		}
		//public void KeyDown(UIKeyEvent e)
		//{
		//	OnKeyDown?.Invoke(e, this);
		//}
		//public void KeyUp(UIKeyEvent e)
		//{
		//	OnKeyUp?.Invoke(e, this);
		//}
		//public void KeyPress(UIKeyEvent e)
		//{
		//	OnKeyPress?.Invoke(e, this);
		//}


		private void InputText()
		{
			PlayerInput.WritingText = true;
			Main.blockInput = false;
			//Main.instance.HandleIME();
#warning 问题很大
			Text = Main.GetInputText(Text);
		}

		private void HandleClick(UIMouseEvent e, UIElement sender)
		{
			Focus();
		}


	}
}
