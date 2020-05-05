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
    public class Inspecter : UIElement {
        private UIPanel _inspecterPanel;
        private UIList _inspectorList;
        public Inspecter() : base() {
            _inspecterPanel = new UIPanel() {
                Pivot = new Vector2(0, 0),
                AnchorPoint = new Vector2(0, 0),
                Position = new Vector2(5, 5),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-10, -9),
            };
            _inspectorList = new UIList() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                Size = new Vector2(-10f, -10f),
                Position = new Vector2(5f, 5f),
            };
            var scrollBar1 = new UIScrollBarV() {
                Name = "ScrollBar",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            _inspectorList.SetScrollBarV(scrollBar1);
            _inspecterPanel.AppendChild(_inspectorList);
            AppendChild(_inspecterPanel);
        }


        private UIElement GetRightElement(Type type) {
            if (type == typeof(bool)) {
                return new UICheckBox() {
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                };
            }
            return new UITextBox() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0f),
                SizeFactor = new Vector2(1, 1),
            };
        }

        public void Add(UIElement element) {
            _inspectorList.Clear();
            foreach (var info in element.GetType().GetProperties()) {
                if (info.IsDefined(typeof(Editor.Attributes.EditorPropertyIgnoreAttribute), true))
                    continue;
                UIElement right = GetRightElement(info.PropertyType);
                var left = new UILabel() {
                    Text = info.Name,
                    SizeFactor = new Vector2(1f, 0f),
                    Size = new Vector2(-10, 20),
                    SizeStyle = SizeStyle.Block,
                    AnchorPoint = new Vector2(0, 0.5f),
                    Pivot = new Vector2(0, 0.5f),
                    Position = new Vector2(5, 0),
                };
                //var text = new UILabel() {
                //    Text = info.Name,
                //    Size = new Vector2(0, 30),
                //    SizeFactor = new Vector2(1, 0),
                //    SizeStyle = SizeStyle.Block,
                //    AnchorPoint = new Vector2(0, 0.5f),
                //    Pivot = new Vector2(0, 0.5f),
                //    Position = new Vector2(5, 0),
                //};
                var item = new UITableBar(left, right) {
                    SizeFactor = new Vector2(1, 0),
                    Size = new Vector2(0, 30),
                };
                _inspectorList.AddElement(item);

            }

        }
    }

}
