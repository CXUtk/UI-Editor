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
using UIEditor.UILib.Enums;

namespace UIEditor.Editor.States {
    public class Viewer : UIElement {
        public Canvas Canvas { get; }
        private UIPanel _viewerPanel;
        private EditorState _editor;

        public Viewer(EditorState editor) : base() {
            _editor = editor;
            PropagationRule = PropagationFlags.FocusEvents;
            _viewerPanel = new UIPanel() {
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                Position = new Vector2(5, 5),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-10, -10),
            };
            AppendChild(_viewerPanel);
            Canvas = new Canvas(editor) {
                Name = "画布",
                Overflow = OverflowType.Hidden,
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                Position = new Vector2(2, 2),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-4, -4),
            };
            _viewerPanel.AppendChild(Canvas);
            Canvas = Canvas;
        }
        public override void MouseLeftDown(UIMouseEvent e) {
            base.MouseLeftDown(e);
            if (_editor.PlaceElement != null)
                Canvas.PlaceElement(e.MouseScreen, _editor.PlaceElement);
        }

        private bool IsSizer(UIElement element) {
            if (element == null) return false;
            if (Parent == null) return false;
            if (element.GetType() == typeof(UISizer)) return true;
            return IsSizer(element.Parent);
        }
        private bool _wasDown;
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (IsFocused && _wasDown && Main.mouseLeftRelease) {
                var target = Canvas.ElementAt(Main.MouseScreen);
                if (!IsSizer(target)) {
                    if (target == Canvas.Root) {
                        Canvas.PlaceSizer(null);
                    } else {
                        _editor.NotifySelectionChange(_editor.Browser.FindTreeElement(target), gameTime);
                        Canvas.PlaceSizer(target);
                    }
                }
            }
            _wasDown = Main.mouseLeft;
            //var a = _canvas.NodePositionToScreenAR(_canvas.ScreenPositionToNodeAR(Main.MouseScreen, Vector2.Zero));
        }

    }
}
