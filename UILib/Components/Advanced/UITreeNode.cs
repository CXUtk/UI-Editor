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
        public bool IsFolded { get; set; }
        public float LeftOffset { get; set; }
        public bool CanFold { get; set; }
        public string Text { get { return _label.Text; } set { _label.Text = value; } }
        private readonly UIImageButton _foldButton;
        private readonly UILabel _label;
        public UITreeNodeDisplay(string text) : base() {
            IsFolded = true;
            CanFold = false;
            IsSelected = false;
            _foldButton = new UIImageButton() {
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
            };
            _foldButton.OnClick += Element_OnClick;
            _label = new UILabel() {
                Text = text,
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                NoEvent = true,
            };
            AppendChild(_foldButton);
            AppendChild(_label);
            OnDoubleClick += Element_OnClick;
        }
        private void Element_OnClick(Events.UIMouseEvent e, UIElement sender) {
            IsFolded ^= true;
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _foldButton.Position = new Vector2(LeftOffset, 0);
            _label.Position = new Vector2(LeftOffset + _foldButton.Width + 5, 0);
            if (!CanFold) {
                _foldButton.Texture = UIEditor.Instance.SkinManager.GetTexture("NoTexture");
                return;
            }

            if (IsFolded) _foldButton.Texture = UIEditor.Instance.SkinManager.GetTexture("Fold_On");
            else _foldButton.Texture = UIEditor.Instance.SkinManager.GetTexture("Fold_Off");
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            var color = UIEditor.Instance.SkinManager.GetColor(IsSelected ? "Background2" : "Background");
            // 容器处于交点颜色就变成highlight
            if (IsSelected && this.Parent.IsFocused) {
                color = UIEditor.Instance.SkinManager.GetColor("Highlight");
            }
            Drawing.DrawAdvBox(sb, 0, 0, Width, Height, color, Main.magicPixel,
                new Vector2(4, 4));

        }
    }
    public class UITreeNode : UIElement {
        public bool CanFold { get { return DisplayElement.CanFold; } set { DisplayElement.CanFold = value; } }
        public UITreeNodeDisplay DisplayElement { get; }
        public IList<UITreeNode> TreeNodes { get; }


        public UITreeNode(string text, IList<UITreeNode> nodes) : base() {
            Pivot = new Vector2(0, 0);
            var display = new UITreeNodeDisplay(text) {
                Pivot = new Vector2(0f, 0f),
                AnchorPoint = new Vector2(0f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0f, 30f),
            };
            DisplayElement = display;
            TreeNodes = nodes;
            //AppendChild(display);
        }
        public UITreeNode(UITreeNodeDisplay nodeDisplay, IList<UITreeNode> nodes) : base() {
            Pivot = new Vector2(0, 0);
            DisplayElement = nodeDisplay;
            TreeNodes = nodes;
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
        }


    }
}
