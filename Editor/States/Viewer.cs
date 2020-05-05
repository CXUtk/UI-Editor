using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using System.Reflection;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Events;
using Microsoft.Xna.Framework.Graphics;

namespace UIEditor.Editor.States {
    public class Viewer : UIElement {
        private UIPanel _viewerPanel;

        public Viewer() : base() {
            BlockPropagation = true;
            _viewerPanel = new UIPanel() {
                Overflow = OverflowType.Hidden,
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                Position = new Vector2(5, 5),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-10, -10),
            };
            AppendChild(_viewerPanel);
            _viewerPanel.OnClick += _viewerPanel_OnClick;
        }

        private void _viewerPanel_OnClick(UIMouseEvent e, UIElement sender) {
            var button = new UIButton() {
                Position = _viewerPanel.ScreenPositionToNodeAR(e.MouseScreen, Vector2.Zero),
                Size = new Vector2(100, 50),
            };
            _viewerPanel.AppendChild(button);
        }
        public override void UpdateSelf(GameTime gameTime) {

            if (IsMouseHover) {
                Main.cursorOverride = 17;
                Main.cursorColor = Color.White;
            }
            base.UpdateSelf(gameTime);
            var a = _viewerPanel.NodePositionToScreenAR(_viewerPanel.ScreenPositionToNodeAR(Main.MouseScreen, Vector2.Zero));
        }

    }
}
