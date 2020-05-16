using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace UIEditor.UILib.Tests {
    public class ItemForTest : ModItem {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault(nameof(ItemForTest));
            DisplayName.AddTranslation(GameCulture.Chinese, nameof(ItemForTest));
        }

        public override void SetDefaults() {
            base.SetDefaults();
            item.CloneDefaults(ItemID.DirtBlock);
        }
    }
}
