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
using UIEditor.Editor.Components;

namespace UIEditor.Editor.States {
    public class Viewer : UIEditorPart {
        public Canvas Canvas { get; }
        private ViewerMiddle Middle { get; }
        private UIPanel _viewerPanel;


        public Viewer(EditorState editor) : base(editor) {
            PropagationRule = PropagationFlags.FocusEvents | PropagationFlags.ScrollWheel;

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
                Pivot = new Vector2(0.5f, 0.5f),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Size = new Vector2(1920, 1080),
            };
            Middle = new ViewerMiddle(Canvas) {
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                Position = new Vector2(2, 2),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-4, -4),
                Overflow = OverflowType.Hidden,
            };
            _viewerPanel.AppendChild(Middle);
        }
        public override void MouseLeftDown(UIMouseEvent e) {
            base.MouseLeftDown(e);
            if (Editor.PlaceElement != null)
                Canvas.PlaceElement(e.MouseScreen, Editor.PlaceElement);
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
            if (Main.hasFocus && IsFocused && _wasDown && Main.mouseLeftRelease) {
                var target = Canvas.GetElementAtMouse();
                if (target == Canvas.Root) {
                    if (!Middle.SizerRectScreen.Contains(Main.MouseScreen.ToPoint())) {
                        Editor.NotifySizerAttached(null);
                        Middle.PlaceSizer(null);
                    }
                } else {
                    Editor.NotifySizerAttached(target);
                    Middle.PlaceSizer(target);
                }
            }
            _wasDown = Main.mouseLeft;
        }

        public override void Initialize() {
            Editor.OnSelectionChange += _editor_OnSelectionChange;
            Editor.OnPropertyChanged += _editor_OnPropertyChanged;
        }

        private void _editor_OnPropertyChanged(UIActionEvent e, UIElement sender) {
            Middle.PlaceSizer(Middle.Sizer.TargetElement);
        }

        private void _editor_OnSelectionChange(UIActionEvent e, UIElement sender) {
            Middle.PlaceSizer(e.Target);
        }
    }
}
