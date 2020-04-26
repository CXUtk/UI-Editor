using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using UIEditor.UILib;
using UIEditor.UILib.Tests;
using UIEditor.Editor.States;

namespace UIEditor {
    public class UIEditor : Mod {
        /// <summary>
        /// UI编辑器的单例实例指针
        /// </summary>
        public static UIEditor Instance;

        private static UIStateMachine UIStateMachine;

        public UIEditor() {

        }

        public override void Load() {
            Instance = this;
            Drawing.Initialize(this);
            UIStateMachine = new UIStateMachine();
            UIStateMachine.Add(new EditorState());
            UIStateMachine.Add(new BottomToolBarState());
            // UIStateMachine.Add(new TestState2());
        }

        public override void Unload() {
            Instance = null;
            UIStateMachine = null;
            Drawing.Unload();
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
