using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UIEditor.UILib.Events;

namespace UIEditor.UILib.Components {
    public class UIItemSlot : UIElement {
        private static Texture2D slotBack = Main.inventoryBackTexture;

        private int itemType;
        private int rightDownTime;
        private bool rightDowned;

        public int ItemType {
            get {
                return itemType;
            }
            set {
                itemType = value;
                Tooltip = Lang.GetItemNameValue(itemType);
            }
        }

        public float ItemScale { get; set; }

        public UIItemSlot() {
            ItemScale = 1f;
            PropagationRule = Enums.PropagationFlags.FocusEvents | Enums.PropagationFlags.ScrollWheel;
            Size = new Microsoft.Xna.Framework.Vector2(slotBack.Width, slotBack.Height);

            OnClick += UIItemSlot_OnClick;
            OnRightClick += UIItemSlot_OnRightClick;

            OnMouseRightDown += UIItemSlot_OnMouseRightDown;
            OnMouseRightUp += UIItemSlot_OnMouseRightUp;
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

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            var topLeft = PositionScreen - new Vector2(Width, Height) * Pivot;
            sb.Draw(slotBack, Pivot * new Vector2(Width, Height), null, Color.White * 0.875f, 0, Pivot * slotBack.Size(),
                     new Vector2(1, 1), SpriteEffects.None, 0f);

            if (0 < ItemType && ItemType < Main.itemTexture.Length) {
                var itemTexture = Main.itemTexture[ItemType];
                var frame = Main.itemAnimations[ItemType] != null ? Main.itemAnimations[ItemType].GetFrame(Main.itemTexture[ItemType]) : Main.itemTexture[ItemType].Frame(1, 1, 0, 0);
                float scale = 1f;
                if (itemTexture.Width > 32 || itemTexture.Height > 32)
                    scale = (itemTexture.Width > itemTexture.Height ? (32f / itemTexture.Width) : (32f / itemTexture.Height));
                sb.Draw(itemTexture, 0.5f * new Vector2(Width, Height), frame, Color.White, 0, 0.5f * frame.Size(),
                         scale, SpriteEffects.None, 0f);
            }
        }

        private void UIItemSlot_OnClick(UIMouseEvent e, UIElement sender) {
            Main.playerInventory = true;
            if (Main.mouseItem.IsAir) {
                Main.mouseItem = new Item();
                Main.mouseItem.SetDefaults(ItemType);
                Main.mouseItem.stack = Main.mouseItem.maxStack;
                Main.PlaySound(SoundID.Coins);
            }
        }

        private void UIItemSlot_OnRightClick(UIMouseEvent e, UIElement sender) {
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
            Main.playerInventory = true;
            rightDownTime = 0;
            rightDowned = true;
        }

        private void UIItemSlot_OnMouseRightUp(UIMouseEvent e, UIElement sender) {
            rightDownTime = 0;
            rightDowned = false;
        }
    }
}
