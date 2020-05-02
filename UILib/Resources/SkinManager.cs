using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.UILib.Resources {
    public class SkinManager {
        private Skin _currentSkin;
        public void Load() {
            _currentSkin = new Skin("default.json");
        }

        public Texture2D GetTexture(string name) {
            return _currentSkin.TryGetTexture(name);
        }
    }
}
