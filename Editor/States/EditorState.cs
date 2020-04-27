using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.States {
    public class EditorState : UIState {
        public EditorState(string name) : base(name) { }

        private UIElement _body;
        private UIScrollBarV scrollBar;

        private const float PADDING_BODY = 10f;
        public override void Initialize() {
            base.Initialize();
            var window = new UIWindow() {
                Name = "a",
                Size = new Vector2(800, 640),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Position = new Vector2(100, 100)
            };
            _body = new UIElement() {
                Name = "Body",
                Pivot = new Vector2(0, 0),
                Position = new Vector2(10, 32),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-PADDING_BODY * 2, -32 - PADDING_BODY),
            };
            scrollBar = new UIScrollBarV() {
                Name = "ScrollBar",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
                Position = new Vector2(-5, 0),
            };
            scrollBar.ViewSize = 0.2f;
            window.OnClose += Box1_OnClose;
            AppendChild(window);
            window.AppendChild(_body);
            _body.AppendChild(scrollBar);
        }

        private void Box1_OnClose(UIActionEvent e, UIElement sender) {
            this.IsActive = false;
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }
    }
}
