using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.Editor.Attributes;
using UIEditor.UILib.Enums;
using Terraria.ID;

namespace UIEditor.UILib.Components {
    [EditorPropertyNoChildren]
    public class UIButton : UIPanel {
        public string Text { get { return _label.Text; } set { _label.Text = value; } }
        public bool DrawPanel { get; set; }
        public bool IsLargeText { get { return _label.IsLargeText; } set { _label.IsLargeText = value; } }
        public Color PanelDefaultColor { get; set; }
        public Color PanelMouseOverColor { get; set; }
        public Color TextDefaultColor { get; set; }
        public Color TextMouseOverColor { get; set; }
        public Vector2 TextAlign { get { return _label.AnchorPoint; } set { _label.AnchorPoint = _label.Pivot = value; } }

        private readonly UILabel _label;
        private bool _isMouseOver;
        private double _timer;

        private void SyncToLabel() {
            _label.NoEvent = true;
        }
        public UIButton() : base() {

            _label = new UILabel();
            Name = "按钮";
            Text = "按钮";
            DrawPanel = true;
            PanelDefaultColor = Color.Gray * 1.2f;
            PanelMouseOverColor = Color.White;

            TextDefaultColor = Color.White;
            TextMouseOverColor = Color.Yellow;
            Color = PanelDefaultColor;
            TextAlign = new Vector2(0.5f, 0.5f);
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
            Main.PlaySound(SoundID.MenuTick);
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
