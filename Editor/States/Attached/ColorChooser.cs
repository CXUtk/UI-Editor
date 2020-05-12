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

namespace UIEditor.Editor.States.Attached {
    public class ColorChooser : UIState {
        private UIWindow _window;
        public ColorChooser(string name) : base(name) {
            _window = new UIWindow() {
                Size = new Vector2(280, 400),
                AnchorPoint = new Vector2(0.5f, 0.5f),
            };
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
            var valR = new UIValueSlider() {
                Min = 0,
                Max = 255,
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
            };
            var R = new UITableBar(labelR, valR) {
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
            var valG = new UIValueSlider() {
                Min = 0,
                Max = 255,
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
            };
            var G = new UITableBar(labelG, valG) {
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
            var valB = new UIValueSlider() {
                Min = 0,
                Max = 255,
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 1),
            };
            var B = new UITableBar(labelB, valB) {
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 30f),
                Division = 0.15f,
                Pivot = new Vector2(0, 0),
                Position = new Vector2(0, 80),
            };
            chooserContainer.AppendChild(B);
        }
    }
}
