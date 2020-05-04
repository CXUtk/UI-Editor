﻿using Microsoft.Xna.Framework;
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
    public class Browser : UIElement {

        private UIPanel _listPanel;
        private UITreeList _treeList;
        private UIList _toolBarList;
        public Browser() : base() {
            _listPanel = new UIPanel() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                Size = new Vector2(-15f, -10f),
                Position = new Vector2(15f, 5f),
                PanelTexture = UIEditor.Instance.SkinManager.GetTexture("Box_Default"),
            };
            _treeList = new UITreeList() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                Size = new Vector2(-1f, -1f),
                Position = new Vector2(1f, 1f),
            };
            var scrollBar1 = new UIScrollBarV() {
                Name = "ScrollBar",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            var scrollBar2 = new UIScrollBarH() {
                Name = "ScrollBarH",
            };
            var toolbar2 = new UIToolBarV() {
                SizeFactor = new Vector2(1f, 1f),
                AnchorPoint = new Vector2(0f, 0.5f),
                Pivot = new Vector2(0f, 0.5f),
                BlockPropagation = true,
                ButtonTooltip = "工具栏",
            };
            AppendChild(_listPanel);
            AppendChild(toolbar2);
            _listPanel.AppendChild(_treeList);
            _treeList.SetScrollBarV(scrollBar1);
            _treeList.SetScrollBarH(scrollBar2);

            _toolBarList = new UIList() {
                Pivot = new Vector2(0, 0),
                Position = new Vector2(10, 10),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-20, -20),

            };
            for (int i = 1; i <= 100; i++) {
                var toolButton = new UIButton() {
                    Text = $"工具{i}",
                    SizeFactor = new Vector2(1, 0),
                    Size = new Vector2(0, 30),
                };
                _toolBarList.AddElement(toolButton);
            }
            var scrollBar3 = new UIScrollBarV() {
                Name = "工具栏进度条",
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            _toolBarList.SetScrollBarV(scrollBar3);
            toolbar2.AddToPanel(_toolBarList);
        }
    }
}