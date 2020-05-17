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
using UIEditor.Editor.States.Attached;
using UIEditor.UILib.Components.Interface;

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
        public Navigator Navigator { get; private set; }
        private UIWindow _window;
        private UIElement _body;
        private Vector2 _placeSize;
        private bool _inPlaceMode;

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
            Navigator = new Navigator(this) {
                Name = "Navigator",
                Pivot = new Vector2(0.5f, 0f),
                AnchorPoint = new Vector2(0.5f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(0, 35f),
                Position = new Vector2(0, 0),
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
            _body.AppendChild(Navigator);
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
            Browser.Position = new Vector2(0, Navigator.Position.Y + Navigator.Height);
            Browser.Size = new Vector2(0f, -(Navigator.Position.Y + Navigator.Height));
            Viewer.Position = new Vector2(Browser.Width, Navigator.Position.Y + Navigator.Height);
            Viewer.Size = new Vector2(0, -(Navigator.Position.Y + Navigator.Height));
            Inspecter.Position = new Vector2(Browser.Width, Viewer.Height + Navigator.Position.Y + Navigator.Height);
            var selected = (BrowserTreeDisplayNode)Browser.SelectedElement;
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

        internal void OpenColorChooser(PropertyInfo info, UIElement target, IUIUpdateable updateable) {
            UIStateMachine.Activate("ColorChooser");
            var chooser = (ColorChooser)UIStateMachine.GetState("ColorChooser");
            UIStateMachine.FocusOn(chooser, Main._drawInterfaceGameTime);
            chooser.SelectedColor = (Color)info.GetValue(target);
            chooser.Info = info;
            chooser.Target = target;
            chooser.Inspecter = updateable;
        }



        internal UIElement PlaceElement { get; private set; }

        public void SetPlaceMode(UIElement element) {
            PlaceElement = element;
            if (element != null) {
                _inPlaceMode = true;
                _placeSize = new Vector2(element.Width, element.Height);
            } else {
                _inPlaceMode = false;
            }
        }
        private void Box1_OnClose(UIActionEvent e, UIElement sender) {
            this.IsActive = false;
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            if (_inPlaceMode) {
                var mousePos = Main.MouseScreen;
                Drawing.DrawAdvBox(sb, new Rectangle((int)(mousePos.X - _placeSize.X / 2), (int)(mousePos.Y - _placeSize.Y / 2), (int)_placeSize.X, (int)_placeSize.Y),
                    Color.Yellow, UIEditor.Instance.SkinManager.GetTexture("BoxFrame_Default"), new Vector2(4, 4));
            }
        }
    }
}
