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
            _body.AppendChild(Browser);
            _body.AppendChild(Viewer);
            _body.AppendChild(Inspecter);

            Browser.Refresh();
        }

        private UIElement _lastFocusElement;

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            Viewer.Position = new Vector2(Browser.Width, 0);
            Inspecter.Position = new Vector2(Browser.Width, Viewer.Height);
            var e = (BrowserTreeNode)Browser.SelectedElement;
            if (_lastFocusElement != e) {
                OnSelectionChange.Invoke(new UIActionEvent(e, gameTime.TotalGameTime), this);
            }
            _lastFocusElement = e;
        }

        public void NotifySelectionChange(UIElement element, GameTime gameTime) {
            OnSelectionChange.Invoke(new UIActionEvent(element, gameTime.TotalGameTime), this);
        }
        internal UIElement PlaceElement { get; private set; }
        public void SetPlaceMode(UIElement element) {
            PlaceElement = element;
        }


        //int tot = 0;
        //UITreeNode _build(UITreeNode node, int level) {
        //    tot++;
        //    List<UITreeNode> nodes = new List<UITreeNode>();
        //    if (node == null)
        //        node = new UITreeNode(tot.ToString(), nodes);
        //    if (level == 6) return node;
        //    for (int i = 0; i < 2; i++) {
        //        UITreeNode child = null;
        //        child = _build(child, level + 1);
        //        nodes.Add(child);
        //    }
        //    return node;
        //}

        private void Box1_OnClose(UIActionEvent e, UIElement sender) {
            this.IsActive = false;
        }



        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);


            //sb.End();
            //sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            //    DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateScale(1) * Matrix.CreateRotationZ(0) * Matrix.CreateTranslation(new Vector3(10, 10, 0)) * Main.UIScaleMatrix);

            // sb.End();
            //sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            //    DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }


        //public override void UpdateSelf(GameTime gameTime) {
        //    base.UpdateSelf(gameTime);
        //    return;
        //    if (_lastFocusElement != UIEditor.Instance.UIStateMachine.FocusedElement) {
        //        var e = UIEditor.Instance.UIStateMachine.FocusedElement;

        //        _list.Clear();
        //        if (e != null) {
        //            foreach (var info in e.GetType().GetProperties()) {

        //                if (info.IsDefined(typeof(Editor.Attributes.EditorPropertyIgnoreAttribute), true))
        //                    continue;
        //                var text = new UILabel() {
        //                    Text = info.Name + ": " + info.GetValue(e)?.ToString(),
        //                    SizeFactor = new Vector2(1f, 0f),
        //                    Size = new Vector2(0, 30),
        //                    SizeStyle = SizeStyle.Block,
        //                };
        //                _list.AddElement(text);

        //            }
        //        }
        //    }
        //    _lastFocusElement = UIEditor.Instance.UIStateMachine.FocusedElement;
        //    //var progress = _body.GetChildByName("Progress") as UIProgressBar;
        //    //progress.CurrentValue = (float)Math.Abs(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 0.5f));
        //    //progress.Rotation = -progress.CurrentValue;
        //}



    }
}
