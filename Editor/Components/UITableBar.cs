﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace UIEditor.UILib.Components.Advanced {
    public class UITableBar : UIElement {
        public float Division { get; set; }

        public UIElement Left { get; }
        public UIElement Right { get; }
        public string LeftTooltip {
            get { return _left.Tooltip; }
            set { _left.Tooltip = value; }
        }

        private UIElement _left;
        private UIElement _right;

        public UITableBar(UIElement left, UIElement right) : base() {
            Division = 0.42f;
            Left = left;
            Right = right;
            _left = new UIElement() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                SizeFactor = new Vector2(0, 1f),
            };
            _right = new UIElement() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                SizeFactor = new Vector2(0, 1f),
            };
            AppendChild(_left);
            AppendChild(_right);
            _left.AppendChild(Left);
            _right.AppendChild(Right);
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _left.SizeFactor = new Vector2(Division, _left.SizeFactor.Y);
            _left.RecalculateSelf();
            _right.Position = new Vector2(_left.Width, 0);
            _right.SizeFactor = new Vector2(1 - Division, _right.SizeFactor.Y);
        }
    }
}
