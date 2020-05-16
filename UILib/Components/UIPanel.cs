using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Components {
    public class UIPanel : UIElement {
        /// <summary>
        /// UI面板的背景贴图
        /// </summary>
        [JsonIgnore]
        public Texture2D PanelTexture { get; set; }
        /// <summary>
        ///  UI面板的边框贴图，如果为null就不画
        /// </summary>
        [JsonIgnore]
        public Texture2D PanelBorderTexture {
            get; set;
        }
        public Vector2 CornerSize {
            get; set;
        }
        public Color Color {
            get; set;
        }
        public UIPanel() : base() {
            Name = "UIPanel";
            PanelTexture = UIEditor.Instance.SkinManager.GetTexture("Panel_Default");
            PanelBorderTexture = null;
            CornerSize = new Vector2(8f, 8f);
            Color = Color.White;
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            Drawing.DrawAdvBox(sb, 0, 0, Width, Height, Color, PanelTexture, CornerSize);
            if (PanelBorderTexture != null)
                Drawing.DrawAdvBox(sb, 0, 0, Width, Height, Color, PanelBorderTexture, CornerSize);
        }
    }
}
