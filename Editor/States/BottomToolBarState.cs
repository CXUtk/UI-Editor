using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.States {
    public class BottomToolBarState : UIState {
        private List<UIElement> _toolElements;
        private UIToolBarH _toolBar;

        public BottomToolBarState(string name) : base(name) { }
        private const float InnerMargin = 10f;

        public override void Initialize() {
            base.Initialize();
            _toolElements = new List<UIElement>();
            _toolBar = new UIToolBarH() {
                Size = new Vector2(70, 70),
                AnchorPoint = new Vector2(0.5f, 1f),
                Pivot = new Vector2(0.5f, 1f),
            };
            AppendChild(_toolBar);
            ZIndex = 0.1f;

            var button = new UIImageButton() {
                Texture = UIEditor.Instance.SkinManager.GetTexture("EditorIcon"),
                Size = new Vector2(30f, 30f),
                SizeStyle = SizeStyle.Inline,
                DefaultColor = Color.White * 0.8f,
                Tooltip = "切换编辑器界面",
                AnchorPoint = new Vector2(0f, 0.5f),
                Pivot = new Vector2(0f, 0.5f),
                Position = new Vector2(20, 0),
            };
            _toolElements.Add(button);
            button.OnClick += Button_OnClick;
            button.PostDrawSelf += Button_PostDrawSelf;
            _toolBar.AddToPanel(button);
        }

        public void AddButton(UIElement element) {
            element.AnchorPoint = new Vector2(0, 0.5f);
            element.Pivot = new Vector2(0, 0.5f);
            float currentX = 20f;
            _toolElements.Add(element);
            _toolBar.AddToPanel(element);
            foreach (var button in _toolElements) {
                button.Update(new GameTime());
                button.Position = new Vector2(currentX, 0);
                currentX += button.Size.X + InnerMargin;
            }
            currentX += 10f;
            _toolBar.Size = new Vector2(Math.Max(70, currentX), 80);
        }

        private void Button_PostDrawSelf(UIDrawEvent e, UIElement sender) {
            if (sender.IsMouseHover) {
                e.SpriteBatch.Draw(UIEditor.Instance.SkinManager.GetTexture("EditorIcon_White"), Vector2.Zero, null, Color.White, 0, Vector2.Zero,
                    new Vector2(1, 1), SpriteEffects.None, 0f);
            }
        }

        private void Button_OnClick(UIMouseEvent e, UIElement sender) {
            UIEditor.Instance.UIStateMachine.Toggle("Editor");
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

        }


    }
}
