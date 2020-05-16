using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Resources {
    public class SkinManager {
        public Skin CurrentSkin { get; }
        public SkinManager() {
            CurrentSkin = new Skin("default.json");
        }

        public Color GetColor(string name) {
            return CurrentSkin.GetColor(name);
        }

        public Texture2D GetTexture(string name) {
            return CurrentSkin.TryGetTexture(name);
        }
    }
}
