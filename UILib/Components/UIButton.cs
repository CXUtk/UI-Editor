using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.Editor.Attributes;

namespace UIEditor.UILib.Components {
    [EditorPropertyNoChildren]
    public class UIButton : UIPanel {
        public string Text { get; set; }
        public bool DrawPanel { get; set; }
        public bool IsLargeText { get; set; }
        public Color PanelDefaultColor { get; set; }
        public Color PanelMouseOverColor { get; set; }
        public Color TextDefaultColor { get; set; }
        public Color TextMouseOverColor { get; set; }

        private readonly UILabel _label;
        private bool _isMouseOver;
        private double _timer;

        private void SyncToLabel() {
            _label.Text = this.Text;
            _label.AnchorPoint = new Vector2(0.5f, 0.5f);
            _label.IsLargeText = this.IsLargeText;
            _label.TextColor = this.TextDefaultColor;
            _label.NoEvent = true;
        }
        public UIButton() : base() {
            Name = "按钮";
            Text = "按钮";
            DrawPanel = true;
            PanelDefaultColor = Color.Gray * 1.2f;
            PanelMouseOverColor = Color.White;

            TextDefaultColor = Color.White;
            TextMouseOverColor = Color.Yellow;
            Color = PanelDefaultColor;

            _label = new UILabel();
            SyncToLabel();
            this.AppendChild(_label);
            this.OnMouseEnter += UITextButton_OnMouseEnter;
            this.OnMouseOut += UITextButton_OnMouseOut;
        }

        private void UITextButton_OnMouseOut(Events.UIMouseEvent e, UIElement sender) {
            _isMouseOver = false;
        }

        private void UITextButton_OnMouseEnter(Events.UIMouseEvent e, UIElement sender) {
            _isMouseOver = true;
            Main.PlaySound(12);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            SyncToLabel();
            if (_isMouseOver) {
                _timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timer > 150) _timer = 150;
            } else {
                _timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timer < 0) _timer = 0;
            }
            float factor = (float)_timer / 150f;
            this.Color = Color.Lerp(PanelDefaultColor, PanelMouseOverColor, factor);
            //this.Scale = new Vector2(1 + _timer / 100f, 1 + _timer / 100f);
            this._label.TextColor = Color.Lerp(TextDefaultColor, TextMouseOverColor, factor);
        }

        public override void DrawSelf(SpriteBatch sb) {
            if (DrawPanel)
                base.DrawSelf(sb);
        }
    }
}
