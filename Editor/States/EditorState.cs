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

namespace UIEditor.Editor.States {
    public class EditorState : UIState {
        public EditorState(string name) : base(name) { }

        private UIWindow _window;
        private UIElement _body;
        private UIElement _navigator;
        private Browser _hierbrowser;
        private Viewer _viewer;
        private UIElement _propertyInspector;
        private UITreeList _list;
        private UIList _toolBarList;
        private UIScrollBarV scrollBar;

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
            _hierbrowser = new Browser() {
                Name = "Browser",
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.382f, 1f),
                Overflow = OverflowType.Hidden,
            };
            _viewer = new Viewer() {
                Name = "Viewer",
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.618f, 0.7f),
            };
            _propertyInspector = new Inspecter() {
                Name = "Inspector",
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                SizeFactor = new Vector2(0.618f, 0.3f),
            };
            _window.OnClose += Box1_OnClose;
            AppendChild(_window);
            _window.AppendChild(_body);
            _body.AppendChild(_hierbrowser);
            _body.AppendChild(_viewer);
            _body.AppendChild(_propertyInspector);
        }
        private UIElement _lastFocusElement = null;
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _viewer.Position = new Vector2(_hierbrowser.Width, 0);
            _propertyInspector.Position = new Vector2(_hierbrowser.Width, _viewer.Height);
            if (_lastFocusElement != UIEditor.Instance.UIStateMachine.LastRightClickElement) {
                var e = UIEditor.Instance.UIStateMachine.LastRightClickElement;
                _list.ClearRoots();
                if (e != null) {
                    _list.AddElement(_copy(e));
                }
            }
            _lastFocusElement = UIEditor.Instance.UIStateMachine.LastRightClickElement;
        }


        private UITreeNode _copy(UIElement element) {
            List<UITreeNode> children = new List<UITreeNode>();
            UITreeNode node = new UITreeNode(element.Name, children);
            foreach (var child in element.Children) {
                children.Add(_copy(child));
            }
            return node;
        }

        int tot = 0;
        UITreeNode _build(UITreeNode node, int level) {
            tot++;
            List<UITreeNode> nodes = new List<UITreeNode>();
            if (node == null)
                node = new UITreeNode(tot.ToString(), nodes);
            if (level == 6) return node;
            for (int i = 0; i < 2; i++) {
                UITreeNode child = null;
                child = _build(child, level + 1);
                nodes.Add(child);
            }
            return node;
        }

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
