﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.States {
    public class EditorState : UIState {
        public EditorState(string name) : base(name) { }

        private UIElement _body;
        private UIList _list;
        private UIScrollBarV scrollBar;

        private const float PADDING_BODY = 10f;
        public override void Initialize() {
            base.Initialize();
            Overflow = OverflowType.Hidden;
            var window = new UIWindow() {
                Name = "a",
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
            _list = new UITreeList() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(0.4f, 1f),
            };
            scrollBar = new UIScrollBarV() {
                Name = "ScrollBar",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
                Position = new Vector2(-5, 0),
            };
            var scrollBarH = new UIScrollBarH() {
                Name = "ScrollBarH",
            };
            var progress = new UIProgressBar() {
                Name = "Progress",
                AnchorPoint = new Vector2(1, 0),
                Pivot = new Vector2(1, 0),
                Position = new Vector2(-5, 5),
                SizeFactor = new Vector2(0.5f, 0f),
                Size = new Vector2(0, 25),
            };
            var textbox = new UITextBox() {
                Name = "Text",
                AnchorPoint = new Vector2(1, 0),
                Pivot = new Vector2(1, 0),
                Position = new Vector2(-5, 50),
                SizeFactor = new Vector2(0.5f, 0f),
                Size = new Vector2(0, 30),
            };
            _list.SetScrollBarV(scrollBar);
            _list.SetScrollBarH(scrollBarH);
            window.OnClose += Box1_OnClose;
            AppendChild(window);
            window.AppendChild(_body);
            _body.AppendChild(_list);
            _body.AppendChild(progress);
            _body.AppendChild(textbox);

            //for (int j = 0; j < 2; j++) {
            //    var list = new List<UITreeNode>();
            //    for (int i = 0; i < 10; i++) {
            //        list.Add(new UITreeNode("Leaf", new List<UITreeNode>()));
            //    }
            //    var root = new UITreeNode("Root", list);
            //    _list.AddElement(root);
            //}
            UITreeNode node = null;
            node = _build(node, 0);
            node.Name = "根节点";
            _list.AddElement(node);
        }
        int tot = 0;
        UITreeNode _build(UITreeNode node, int level) {
            tot++;
            List<UITreeNode> nodes = new List<UITreeNode>();
            if (node == null) {
                node = new UITreeNode(tot.ToString(), nodes);
            }
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

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            //var progress = _body.GetChildByName("Progress") as UIProgressBar;
            //progress.CurrentValue = (float)Math.Abs(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 0.5f));
            //progress.Rotation = -progress.CurrentValue;
        }
    }
}
