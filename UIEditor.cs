using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using UIEditor.UILib;
using UIEditor.UILib.Tests;
using UIEditor.Editor.States;
using UIEditor.UILib.Resources;
using Microsoft.Xna.Framework.Graphics;
using UIEditor.Editor.States.Attached;
using UIEditor.Editor.Helper;

namespace UIEditor {
    public class UIEditor : Mod {
        /// <summary>
        /// UI编辑器的单例实例指针
        /// </summary>
        public static UIEditor Instance;

        public UIStateMachine UIStateMachine;
        public SkinManager SkinManager;
        internal Documentation Documentation;

        public UIEditor() {

        }

        public override void Load() {
            Instance = this;
            SkinManager = new SkinManager();

            UIStateMachine = new UIStateMachine();
            UIStateMachine.Add(new EditorState("Editor"));
            UIStateMachine.Add(new BottomToolBarState("ToolbarBottom"));
            UIStateMachine.Add(new TestState2("tstate2"));
            UIStateMachine.Add(new ColorChooser("ColorChooser"));
        }

        public override void PostSetupContent() {
            base.PostSetupContent();
            Documentation = new Documentation();
            Documentation.Load();
            Array.Resize(ref Main.cursorTextures, 25);
            Main.cursorTextures[17] = GetTexture("Images/Cursors/CursorMove");
            Main.cursorTextures[18] = GetTexture("Images/Cursors/CursorScale");
            Main.cursorTextures[19] = GetTexture("Images/Cursors/CursorScaleH");
            On.Terraria.Main.DrawInterface_36_Cursor += Main_DrawInterface_36_Cursor;
        }

        private void Main_DrawInterface_36_Cursor(On.Terraria.Main.orig_DrawInterface_36_Cursor orig) {
            if (Main.cursorOverride != -1) {
                Color color = new Color((int)(Main.cursorColor.R * 0.2f), (int)(Main.cursorColor.G * 0.2f),
                    (int)(Main.cursorColor.B * 0.2f), (int)(Main.cursorColor.A * 0.5f));
                Color white = Main.cursorColor;
                bool flag = true;
                bool flag2 = true;
                float rotation = 0f;
                Vector2 value = default(Vector2);
                float num = 1f;
                if (Main.cursorOverride == 2) {
                    flag = false;
                    white = Color.White;
                    num = 0.7f;
                    value = new Vector2(0.1f);
                }
                switch (Main.cursorOverride) {
                    case 2:
                        flag = false;
                        white = Color.White;
                        num = 0.7f;
                        value = new Vector2(0.1f);
                        break;
                    case 3:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        flag = false;
                        white = Color.White;
                        break;
                    case 17:
                    case 18:
                    case 19:
                        white = Color.White;
                        value = new Vector2(0.5f, 0.5f);
                        break;
                }
                if (flag) {
                    Main.spriteBatch.Draw(Main.cursorTextures[Main.cursorOverride], new Vector2(Main.mouseX + 1, Main.mouseY + 1), null, color, rotation, value * Main.cursorTextures[Main.cursorOverride].Size(), Main.cursorScale * 1.1f * num, SpriteEffects.None, 0f);
                }
                if (flag2) {
                    Main.spriteBatch.Draw(Main.cursorTextures[Main.cursorOverride], new Vector2(Main.mouseX, Main.mouseY), null, white, rotation, value * Main.cursorTextures[Main.cursorOverride].Size(), Main.cursorScale * num, SpriteEffects.None, 0f);
                }
            } else if (Main.SmartCursorEnabled) {
                Main.DrawCursor(Main.DrawThickCursor(smart: true), smart: true);
            } else {
                Main.DrawCursor(Main.DrawThickCursor());
            }
        }

        public override void Unload() {
            Array.Resize(ref Main.cursorTextures, 17);
            On.Terraria.Main.DrawInterface_36_Cursor -= Main_DrawInterface_36_Cursor;
            Instance = null;
            UIStateMachine = null;
            SkinManager = null;
            base.Unload();
        }


        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            // 在TR原版鼠标绘制层的下方插入UI层
            int mouseLayer = layers.FindIndex((layer) => layer.Name.Equals("Vanilla: Mouse Text"));
            layers.Insert(mouseLayer, new LegacyGameInterfaceLayer("TemplateMod: UI",
                () => {
                    try {
                        UIStateMachine.Draw(Main.spriteBatch);
                    } catch (Exception ex) {
                        // Ignored
                        Logger.Error(ex.ToString());
                    }
                    return true;
                })
            );
        }

        public override void UpdateUI(GameTime gameTime) {
            try {
                UIStateMachine.Update(gameTime);
            } catch (Exception ex) {
                Logger.Error(ex.ToString());
            }
            base.UpdateUI(gameTime);
        }
    }
}
