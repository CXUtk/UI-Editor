
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;

namespace UIEditor.Editor.States {
    public class Canvas : UIElement {
        public UIElement Root { get; }
        private UISizer _sizer;
        private EditorState _editor;

        public Canvas(EditorState editor) : base() {
            _editor = editor;
            _sizer = new UISizer() {
                IsActive = false,
            };
            Root = new UIElement() {
                SizeFactor = new Vector2(1f, 1f),
                Pivot = new Vector2(0, 0),
            };
            AppendChild(Root);
            Root.AppendChild(new UIButton() {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Size = new Vector2(50, 50),
            });
            AppendChild(_sizer);
        }

        public void PlaceElement(Vector2 pos, UIElement element) {
            Root.AppendChild(element);
            element.RecalculateSelf();
            element.Position = element.ScreenPositionToParentAR(pos);
            _editor.Browser.AddNode(element);
        }
    }
}
