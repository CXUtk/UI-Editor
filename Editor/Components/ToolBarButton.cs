using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Events;
using UIEditor.UILib.Components;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace UIEditor.Editor.Components {
    public class ToolBarButton : UIElement {
        public Texture2D ButtonTexture { get { return _image.Texture; } set { _image.Texture = value; } }
        public string Text { get { return _label.Text; } set { _label.Text = value; } }
        private readonly UIImage _image;
        private readonly UILabel _label;
        public ToolBarButton() : base() {
            IsSelected = false;
            _image = new UIImage() {
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
            };
            _label = new UILabel() {
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                NoEvent = true,
            };
            AppendChild(_image);
            AppendChild(_label);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _image.Position = new Vector2(25f, 0);
            _label.Position = new Vector2(25f + _image.Width + 15f, 0);
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            var color = UIEditor.Instance.SkinManager.GetColor(IsSelected ? "Highlight" : "Background");
            Drawing.DrawAdvBox(sb, 0, 0, Width, Height, color, Main.magicPixel,
                new Vector2(4, 4));

        }
    }
}
