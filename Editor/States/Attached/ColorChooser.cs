using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using Microsoft.Xna.Framework;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Advanced;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;
using Terraria;
using UIEditor.Editor.Components;
using System.Reflection;
using UIEditor.UILib.Components.Interface;

namespace UIEditor.Editor.States.Attached {
    public class ColorChooser : UIState {

        private class ColorPreviwer : UIElement {
            public Color Color { get; set; }
            public ColorPreviwer() : base() { }
            public override void DrawSelf(SpriteBatch sb) {
                base.DrawSelf(sb);
                sb.End();
                SpriteBatchBegin(sb, BlendState.NonPremultiplied);
                for (int i = 0; i < Width; i += 12) {
                    for (int j = 0; j < Height; j += 12) {
                        sb.Draw(ModContent.GetTexture("UIEditor/Images/ColorBG2"), new Rectangle(i, j, 12, 12), Color.White);
                    }
                }
                sb.Draw(Main.magicPixel, new Rectangle(0, 0, Width, Height), Color);
                sb.End();
                SpriteBatchBegin(sb, BlendState.AlphaBlend);
            }
        }
        private UIWindow _window;
        private ColorPreviwer _preview;
        private UIValueSlider _R;
        private UIValueSlider _G;
        private UIValueSlider _B;
        private UIValueSlider _A;
        private UIColorBar _colorBar;
        private UILabel _hex;
        public int R { get { return _R.Value; } }
        public int G { get { return _G.Value; } }
        public int B { get { return _B.Value; } }
        public int A { get { return _A.Value; } }
        public Color SelectedColor {
            get { return new Color(R, G, B, A); }
            set {
                _R.Value = value.R;
                _G.Value = value.G;
                _B.Value = value.B;
                _A.Value = value.A;
            }
        }

        public PropertyInfo Info { get; set; }
        public UIElement Target { get; set; }
        internal IUIUpdateable Inspecter { get; set; }

        public ColorChooser(string name) : base(name) {
            IsActive = false;
            _window = new UIWindow() {
                Size = new Vector2(270, 420),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                CloseButtonOffset = new Vector2(0, 0),
            };
            _window.OnClose += _window_OnClose;
            ZIndex = 0.15f;

            // 下半部分
            var chooser = new UIElement() {
                SizeFactor = new Vector2(1f, 0.5f),
                AnchorPoint = new Vector2(0.5f, 1f),
                Pivot = new Vector2(0.5f, 1f),
            };
            var chooserContainer = new UIElement() {
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-20, -20),
                Position = new Vector2(10, 10),
            };

            var labelR = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                Text = "R",
                TextColor = Color.Red,
            };
            _R = new UIValueSlider() {
                Min = 0,
                Max = 255,
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
            };
            var R = new UITableBar(labelR, _R) {
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 30f),
                Division = 0.15f,
                Pivot = new Vector2(0, 0),
            };
            AppendChild(_window);
            _window.AppendChild(chooser);
            chooser.AppendChild(chooserContainer);
            chooserContainer.AppendChild(R);
            var labelG = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                Text = "G",
                TextColor = Color.LimeGreen,
            };
            _G = new UIValueSlider() {
                Min = 0,
                Max = 255,
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
            };
            var G = new UITableBar(labelG, _G) {
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 30f),
                Division = 0.15f,
                Pivot = new Vector2(0, 0),
                Position = new Vector2(0, 40),
            };
            chooserContainer.AppendChild(G);
            var labelB = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                Text = "B",
                TextColor = Color.Cyan,
            };
            _B = new UIValueSlider() {
                Min = 0,
                Max = 255,
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
            };
            var B = new UITableBar(labelB, _B) {
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 30f),
                Division = 0.15f,
                Pivot = new Vector2(0, 0),
                Position = new Vector2(0, 80),
            };
            chooserContainer.AppendChild(B);
            var labelA = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                Text = "A",
                TextColor = Color.White,
            };
            _A = new UIValueSlider() {
                Min = 0,
                Max = 255,
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
            };
            var A = new UITableBar(labelA, _A) {
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 30f),
                Division = 0.15f,
                Pivot = new Vector2(0, 0),
                Position = new Vector2(0, 120),
            };
            chooserContainer.AppendChild(A);


            var labelHex = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                Text = "Hex",
                TextColor = Color.White,
            };
            _hex = new UILabel() {
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                Position = new Vector2(20, 0),
            };
            var hex = new UITableBar(labelHex, _hex) {
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 30f),
                Division = 0.15f,
                Pivot = new Vector2(0, 0),
                Position = new Vector2(0, 160),
            };
            _preview = new ColorPreviwer() {
                SizeFactor = new Vector2(0f, 0f),
                Size = new Vector2(72f, 24f),
                Pivot = new Vector2(1, 1),
                AnchorPoint = new Vector2(1, 1),
                Position = new Vector2(0, -3),
            };
            chooserContainer.AppendChild(hex);
            chooserContainer.AppendChild(_preview);



            // 上半部分

            var colorView = new UIElement() {
                SizeFactor = new Vector2(1f, 0.5f),
                AnchorPoint = new Vector2(0.5f, 0f),
                Pivot = new Vector2(0.5f, 0f),
                Position = new Vector2(0, 30),
                Size = new Vector2(0, -30f),
            };
            var colorViewContainer = new UIElement() {
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-20, -20),
                Position = new Vector2(10, 10),
            };
            _window.AppendChild(colorView);
            colorView.AppendChild(colorViewContainer);


            _colorBar = new UIColorBar() {
                Pivot = new Vector2(1f, 1f),
                AnchorPoint = new Vector2(1f, 1f),
                SizeFactor = new Vector2(0f, 1f),
                Size = new Vector2(25f, -5f),
                Position = new Vector2(-10, 0),
            };
            colorViewContainer.AppendChild(_colorBar);

            _R.OnValueChanged += _R_OnValueChanged;
            _G.OnValueChanged += _R_OnValueChanged;
            _B.OnValueChanged += _R_OnValueChanged;
            _A.OnValueChanged += _R_OnValueChanged;

        }

        private void _window_OnClose(UILib.Events.UIActionEvent e, UIElement sender) {
            this.IsActive = false;
        }

        private void _R_OnValueChanged(UILib.Events.UIActionEvent e, UIElement sender) {
            Inspecter?.UpdateValue();
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _hex.Text = "#" + SelectedColor.Hex4();
            _preview.Color = SelectedColor;
            if (Info != null && Target != null) {
                Info.SetValue(Target, SelectedColor);
            }
        }

    }
}
