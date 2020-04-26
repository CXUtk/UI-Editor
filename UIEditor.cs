using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using UILib.UI;
using UILib.UI.Tests;

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
            UIStateMachine.Add(new TestState());
            UIStateMachine.Add(new TestState2());
        }

        public override void Unload() {
            Instance = null;
            UIStateMachine = null;
            base.Unload();
        }


        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            // 在TR原版鼠标绘制层的下放插入UI层
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
