using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using UIEditor.UILib.Events;

namespace UIEditor.UILib.Components {
    public class UIItemSlot : UIElement {
        public bool Enabled { get; set; }
        private Texture2D slotBack = Main.inventoryBackTexture;

        private int itemType;
        private int rightDownTime;
        private bool rightDowned;

        public int ItemType {
            get {
                return itemType;
            }
            set {
                itemType = value;
            }
        }

        public float Opacity { get; set; }

        public int ItemStack { get; set; }

        public float ItemScale { get; set; }

        public UIItemSlot() {
            Enabled = false;
            ItemScale = 1f;
            PropagationRule = Enums.PropagationFlags.FocusEvents | Enums.PropagationFlags.ScrollWheel;
            Size = new Vector2(slotBack.Width, slotBack.Height);

            OnClick += UIItemSlot_OnClick;
            OnRightClick += UIItemSlot_OnRightClick;

            OnMouseRightDown += UIItemSlot_OnMouseRightDown;
            OnMouseRightUp += UIItemSlot_OnMouseRightUp;
            ItemStack = 1;
            Opacity = 1f;
        }

        protected bool _mouseHover;

        public override void MouseEnter(UIMouseEvent e) {
            _mouseHover = true;
            base.MouseEnter(e);
        }

        public override void MouseOut(UIMouseEvent e) {
            _mouseHover = false;
            base.MouseOut(e);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

            if (!Main.mouseRight) {
                rightDowned = false;
                rightDownTime = 0;
            }
            if (!Main.playerInventory) {
                rightDownTime = 0;
                return;
            }
            if (rightDowned) {
                rightDownTime++;

                bool ok = false;
                if (rightDownTime < 12) {
                    ok = false;
                } else if (rightDownTime < 60 * 2) {
                    if (rightDownTime % 10 == 0) {
                        ok = true;
                    }
                } else if (rightDownTime < 60 * 4) {
                    if (rightDownTime % 5 == 0) {
                        ok = true;
                    }
                } else if (rightDownTime < 60 * 5) {
                    if (rightDownTime % 3 == 0) {
                        ok = true;
                    }
                } else {
                    if (rightDownTime % 2 == 0) {
                        ok = true;
                    }
                }
                if (ok) {
                    if (Main.mouseItem.IsAir) {
                        Main.mouseItem = new Item();
                        Main.mouseItem.SetDefaults(ItemType);
                        Main.PlaySound(SoundID.Coins);
                    } else if (Main.mouseItem.type == ItemType) {
                        if (Main.mouseItem.stack < Main.mouseItem.maxStack) {
                            Main.mouseItem.stack++;
                            Main.PlaySound(SoundID.Coins);
                        }
                    }
                }
            }
        }
        public virtual void HandleTooltip() {
            Item item = new Item();
            item.SetDefaults(ItemType);
            item.stack = ItemStack;
            Main.hoverItemName = item.Name;
            if (item.stack > 1)
                Main.hoverItemName = Main.hoverItemName + " (" + item.stack + ")";
            if (item.modItem != null)
                Main.hoverItemName = Main.hoverItemName + " [" + item.modItem.mod.DisplayName + "]";
            Main.HoverItem = item;
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            if (_mouseHover) {
                HandleTooltip();
            }
            Drawing.DrawAdvBox(sb, new Rectangle(0, 0, Width, Height), Color.White * Opacity, IsSelected ? Main.inventoryBack15Texture : slotBack, new Vector2(6, 6));

            if (0 < ItemType && ItemType < Main.itemTexture.Length) {
                var itemTexture = Main.itemTexture[ItemType];
                var frame = Main.itemAnimations[ItemType] != null ? Main.itemAnimations[ItemType].GetFrame(Main.itemTexture[ItemType]) : Main.itemTexture[ItemType].Frame(1, 1, 0, 0);
                float scale = 1f;
                if (frame.Width > 32 || frame.Height > 32)
                    scale = (frame.Width > frame.Height ? (32f / frame.Width) : (32f / frame.Height));
                sb.Draw(itemTexture, 0.5f * new Vector2(Width, Height), frame, Color.White, 0, 0.5f * frame.Size(),
                         scale, SpriteEffects.None, 0f);
                if (ItemStack > 1) {
                    ChatManager.DrawColorCodedStringWithShadow(sb, Main.fontItemStack, ItemStack.ToString(), new Vector2(10, Height - 20), Color.White, 0, Vector2.Zero, Scale * 0.9f);
                }
            }
        }

        private void UIItemSlot_OnClick(UIMouseEvent e, UIElement sender) {
            if (!Enabled) return;
            Main.playerInventory = true;
            if (Main.mouseItem.IsAir) {
                Main.mouseItem = new Item();
                Main.mouseItem.SetDefaults(ItemType);
                Main.mouseItem.stack = Main.mouseItem.maxStack;
                Main.PlaySound(SoundID.Coins);
            }
        }

        private void UIItemSlot_OnRightClick(UIMouseEvent e, UIElement sender) {
            if (!Enabled) return;
            if (!Main.playerInventory || rightDownTime >= 12) {
                return;
            }
            if (Main.mouseItem.IsAir) {
                Main.mouseItem = new Item();
                Main.mouseItem.SetDefaults(ItemType);
                Main.PlaySound(SoundID.Coins);
            } else if (Main.mouseItem.type == ItemType) {
                if (Main.mouseItem.stack < Main.mouseItem.maxStack) {
                    Main.mouseItem.stack++;
                    Main.PlaySound(SoundID.Coins);
                }
            }
        }

        private void UIItemSlot_OnMouseRightDown(UIMouseEvent e, UIElement sender) {
            if (!Enabled) return;
            Main.playerInventory = true;
            rightDownTime = 0;
            rightDowned = true;
        }

        private void UIItemSlot_OnMouseRightUp(UIMouseEvent e, UIElement sender) {
            if (!Enabled) return;
            rightDownTime = 0;
            rightDowned = false;
        }
    }
}
