using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.States {
    public class ViewerMiddle : UIElement {
        public Canvas Canvas { get; }
        public UISizer Sizer { get; }
        private EditorState Editor { get { return Canvas.Editor; } }
        public Rectangle SizerRectScreen {
            get {
                if (Sizer.IsActive) {
                    return Sizer.InnerRectangleScreen;
                }
                return Rectangle.Empty;
            }
        }
        public ViewerMiddle(Canvas canvas) : base() {
            Canvas = canvas;
            AppendChild(Canvas);
            Sizer = new UISizer() {
                IsActive = false,
            };
            Sizer.OnSizerChanged += _sizer_OnSizerChanged;
            Canvas.OnScrollWheel += Canvas_OnScrollWheel;
            AppendChild(Sizer);
        }

        private void Canvas_OnScrollWheel(UIScrollWheelEvent e, UIElement sender) {
            Editor.NotifyCanvasScaleChanged(Canvas);
        }

        private void _sizer_OnSizerChanged(UIActionEvent e, UIElement sender) {
            Editor.NotifySizerChanged(Sizer);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (Sizer.TargetElement != null && Sizer.TargetElement.ShouldRecalculate) {
                Sizer.AttachTo(Sizer.TargetElement);
            }
        }

        public void PlaceSizer(UIElement element) {
            if (element == null) {
                Sizer.UnAttach();
                Sizer.IsActive = false;
                return;
            }
            Sizer.IsActive = true;
            Sizer.AttachTo(element);
        }
    }
}
