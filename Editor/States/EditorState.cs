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
using UIEditor.Editor.Components;

namespace UIEditor.Editor.States {
    public class EditorState : UIState {
        internal event ActionEvent OnSelectionChange;
        internal event ActionEvent OnSizerAttached;
        internal event ActionEvent OnSizerChanged;
        internal event ActionEvent OnPropertyChanged;
        internal event ActionEvent OnPlaceElement;
        internal event ActionEvent OnViewerMove;

        public EditorState(string name) : base(name) { }

        public Viewer Viewer { get; private set; }
        public Browser Browser { get; private set; }
        public Inspecter Inspecter { get; private set; }

        private UIWindow _window;
        private UIElement _body;
        private UIElement _navigator;
        private const float PADDING_BODY = 10f;
        public override void Initialize() {
            base.Initialize();
            Overflow = OverflowType.Hidden;
            _window = new UIWindow() {
                Name = "Editor",
                Size = new Vector2(800, 640),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Position = new Vector2(100, 100)
            };
            _body = new UIElement() {
                Name = "Body",
                Pivot = new Vector2(0, 0),
                Position = new Vector2(10, 32),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-PADDING_BODY * 2, -32 - PADDING_BODY),
            };
            Browser = new Browser(this) {
                Name = "Browser",
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.382f, 1f),
                Overflow = OverflowType.Hidden,
            };
            Viewer = new Viewer(this) {
                Name = "Viewer",
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.618f, 0.7f),
            };
            Inspecter = new Inspecter(this) {
                Name = "Inspector",
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.618f, 0.3f),
            };
            _window.OnClose += Box1_OnClose;
            AppendChild(_window);
            _window.AppendChild(_body);
            _body.AppendChild(Viewer);
            _body.AppendChild(Browser);
            _body.AppendChild(Inspecter);

            _init();
        }

        public void _init() {
            Browser.Initialize();
            Viewer.Initialize();
            Inspecter.Initialize();
        }


        private UIElement _lastFocusElement;

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            Viewer.Position = new Vector2(Browser.Width, 0);
            Inspecter.Position = new Vector2(Browser.Width, Viewer.Height);
            var selected = (BrowserTreeNode)Browser.SelectedElement;
            var e = (selected == null) ? null : selected.BindingElement;
            if (_lastFocusElement != e) {
                OnSelectionChange.Invoke(new UIActionEvent(e, gameTime.TotalGameTime), this);
                _lastFocusElement = e;
            }
        }

        public void NotifySizerAttached(UIElement element) {
            _lastFocusElement = element;
            OnSizerAttached?.Invoke(new UIActionEvent(element, Main._drawInterfaceGameTime.TotalGameTime), this);
        }


        public void NotifyElementPropertyChange(UIElement element) {
            OnPropertyChanged?.Invoke(new UIActionEvent(element, Main._drawInterfaceGameTime.TotalGameTime), this);
        }


        public void NotifyPlaceElement(UIElement element) {
            OnPlaceElement?.Invoke(new UIActionEvent(element, Main._drawInterfaceGameTime.TotalGameTime), this);
        }


        public void NotifyMoveViewer(UIElement element) {
            OnViewerMove?.Invoke(new UIActionEvent(element, Main._drawInterfaceGameTime.TotalGameTime), this);
        }

        public void NotifySizerChanged(UIElement element) {
            OnSizerChanged?.Invoke(new UIActionEvent(element, Main._drawInterfaceGameTime.TotalGameTime), this);
        }

        public void NotifyCanvasScaleChanged(UIElement element) {
            OnSizerChanged?.Invoke(new UIActionEvent(element, Main._drawInterfaceGameTime.TotalGameTime), this);
        }



        internal UIElement PlaceElement { get; private set; }

        public void SetPlaceMode(UIElement element) {
            PlaceElement = element;
        }
        private void Box1_OnClose(UIActionEvent e, UIElement sender) {
            this.IsActive = false;
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
        }
    }
}
