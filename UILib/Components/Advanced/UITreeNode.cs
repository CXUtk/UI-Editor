using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace UIEditor.UILib.Components.Advanced {
    public class UITreeNodeDisplay : UIElement {
        public float DisplayPaddingLeft { get; set; }
        public UIImageButton _foldButton;
        private UILabel _label;
        public UITreeNodeDisplay(string text) : base() {
            _foldButton = new UIImageButton() {
                Texture = ModContent.GetTexture("UIEditor/Images/Cog"),
                Size = new Vector2(32f, 32f),
                SizeStyle = SizeStyle.Block,
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
            };
            _label = new UILabel() {
                Text = text,
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
            };
            AppendChild(_foldButton);
            AppendChild(_label);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _foldButton.Position = new Vector2(DisplayPaddingLeft, 0);
            _label.Position = new Vector2(DisplayPaddingLeft + _foldButton.Width + 5, 0);
        }
    }
    public class UITreeNode : UIElement {

        public UITreeNodeDisplay DisplayElement { get; }
        public IList<UITreeNode> TreeNodes { get; }
        public bool IsFolded { get; set; }

        public UITreeNode(string text, IList<UITreeNode> nodes) : base() {
            Pivot = new Vector2(0, 0);
            IsFolded = true;
            var display = new UITreeNodeDisplay(text) {
                Pivot = new Vector2(0f, 0f),
                AnchorPoint = new Vector2(0f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0f, 32f),
            };
            display._foldButton.OnClick += Element_OnClick;
            DisplayElement = display;
            TreeNodes = nodes;
            //AppendChild(display);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

        }

        private void Element_OnClick(Events.UIMouseEvent e, UIElement sender) {
            IsFolded ^= true;
        }
    }
}
