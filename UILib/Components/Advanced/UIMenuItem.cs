using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Components {
    // Pivot和Anchor得用(0, 0)
    public class UIMenuItem : UIElement {
        #region Fields
        private List<UIMenuItem> items;
        private UILabel label;
        #endregion
        #region Properties
        public float ItemsPadding {
            get;
            set;
        }
        public bool ShouldExpand {
            get;
            private set;
        }
        public string Text {
            get => label.Text;
            set { label.Text = value; UpdateSize(); }
        }

        public Rectangle SubMenusRect {
            get {
                throw new NotImplementedException();
            }
        }
        public float ItemsWidth {
            get;
            set;
        }
        #endregion
        #region Ctor
        public UIMenuItem() {
            label = new UILabel() {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                NoEvent = true,
            };
            Pivot = Vector2.Zero;
            AnchorPoint = Vector2.Zero;
            label.Text = string.Empty;
            AppendChild(label);
            BlockPropagation = true;
            Size = new Vector2(19, 22);
            items = new List<UIMenuItem>();
            ItemsWidth = 20;
        }
        #endregion
        #region Methods
        #region Add & Remove
        public void AddItem(UIMenuItem item) {
            items.Add(item);
            AppendChild(item);
        }
        public void RemoveItem(UIMenuItem item) {
            items.Remove(item);
            RemoveChild(item);
        }
        #endregion
        #region Draw
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
        }
        public override void DrawChildren(SpriteBatch sb) {
            foreach (var child in Children) {
                if (child.IsActive && !(child is UIMenuItem)) {
                    child.Draw(sb);
                }
            }
            if (ShouldExpand) {
                items.ForEach(item => item.Draw(sb));
            }
        }
        #endregion
        #region Update
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (IsMouseHover) {
                label.TextColor = Color.Aqua;
            } else {
                label.TextColor = Color.White;
            }
        }
        public override void UpdateChildren(GameTime gameTime) {
            foreach (var child in Children) {
                if (child.IsActive && !(child is UIMenuItem)) {
                    child.Update(gameTime);
                }
            }
            if (ShouldExpand) {
                items.ForEach(item => item.Update(gameTime));
            }
        }
        // 1为向右, -1为向左, 0直接向下
        internal void UpdateSubItemsPosition(int dir) {
            foreach (var item in items) {
                item.Size = new Vector2(item.Size.X, ItemsWidth);
            }
            switch (dir) {
                case 0: {
                        Vector2 startpos = PostionScreen - new Vector2(Width, Height) * Pivot;
                        startpos.Y += Height;
                        foreach (var item in items) {
                            item.PostionScreen = startpos;
                            startpos.Y += item.Height - 1;
                        }
                        var center = PostionScreen - new Vector2(Width, Height) * Pivot + new Vector2(ItemsWidth, Height) / 2;
                        if (center.X < Main.screenWidth / 2) {
                            // 往右
                            dir = 1;
                        } else {
                            // 往左
                            dir = -1;
                        }
                        break;
                    }
                case -1: {
                        Vector2 startpos = Parent.PostionScreen - new Vector2(Width, Height) * Pivot - new Vector2(((UIMenuItem)Parent).ItemsWidth, 0);
                        foreach (var item in items) {
                            item.PostionScreen = startpos;
                            startpos.Y += item.Height - 1;
                        }
                        break;
                    }
                case 1: {
                        Vector2 startpos = Parent.PostionScreen + new Vector2(Width, Height) * (-Pivot + new Vector2(1, 0)) + new Vector2(((UIMenuItem)Parent).ItemsWidth, 0);
                        foreach (var item in items) {
                            item.PostionScreen = startpos;
                            startpos.Y += item.Height - 1;
                        }
                        break;
                    }
            }
            foreach (var item in items) {
                if (item.ShouldExpand) {
                    item.UpdateSubItemsPosition(dir);
                    return;
                }
            }
        }
        internal void UpdateExpand() {
            var mouseInside = ScreenHitBox.Contains(Main.MouseScreen);
            if (!ShouldExpand) {
                if (mouseInside) {
                    ShouldExpand = true;
                    return;
                }
            }
            if (ShouldExpand) {
                if (!mouseInside) {
                    ShouldExpand = false;
                    items.ForEach(item => item.UpdateExpand());
                    if (items.Count(item => item.ShouldExpand) > 0) {
                        ShouldExpand = true;
                    }
                }
            }
        }
        private void UpdateSize() {
            Size = new Vector2(label.MeasureSize(Text).X, Size.Y);
            if (Parent is UIMenu menu) {
                Size = new Vector2(Size.X + menu.ItemsPadding, Size.Y);
            } else if (Parent is UIMenuItem item) {
                Size = new Vector2(Size.X, Size.Y + item.ItemsPadding);
            }
        }
        #endregion
        #region EventHandlers
        private void MouseClickHandler(MouseEvent e, UIElement sender) {
            if (Parent is UIMenu) {

            } else if (((UIMenuItem)Parent).ShouldExpand) {
                ShouldExpand ^= true;
            }
        }
        #endregion
        #endregion
    }
}
