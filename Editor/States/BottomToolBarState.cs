using Microsoft.Xna.Framework;
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
                Texture = ModContent.GetTexture("UIEditor/Images/Cog"),
                Size = new Vector2(32f, 32f),
                SizeStyle = SizeStyle.Block,
            };
            _toolElements.Add(button);
            button.AnchorPoint = new Vector2(0.5f, 0.5f);
            button.OnClick += Button_OnClick;
            _toolBar.AddToPanel(button);
        }

        private void Button_OnClick(UIMouseEvent e, UIElement sender) {
            UIEditor.Instance.UIStateMachine.Toggle("Editor");
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

        }


    }
}
