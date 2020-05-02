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

        public override void Initialize() {
            base.Initialize();
            _toolElements = new List<UIElement>();
            _toolBar = new UIToolBarH() {
                Size = new Vector2(100, 80),
                AnchorPoint = new Vector2(0.5f, 1f),
                Pivot = new Vector2(0.5f, 1f),
            };
            AppendChild(_toolBar);
            ZIndex = 1f;

            var button = new UIImageButton() {
                Texture = Drawing.CogTexture,
                Size = new Vector2(30f, 30f),
                SizeStyle = SizeStyle.Block,
                DefaultColor = Color.White * 0.8f,
                Tooltip = "切换编辑器界面"
            };
            _toolElements.Add(button);
            button.AnchorPoint = new Vector2(0.5f, 0.5f);
            button.OnClick += Button_OnClick;
            button.PostDrawSelf += Button_PostDrawSelf;
            _toolBar.AddToPanel(button);
        }

        private void Button_PostDrawSelf(UIDrawEvent e, UIElement sender) {
            if (sender.IsMouseHover) {
                e.SpriteBatch.Draw(Drawing.CogTexture_White, Vector2.Zero, null, Color.White, 0, Vector2.Zero,
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
