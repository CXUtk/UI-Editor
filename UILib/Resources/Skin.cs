using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace UIEditor.UILib.Resources {
    public class Skin {
        private class SkinData {
            public Dictionary<string, string> textures = new Dictionary<string, string>();
            public Dictionary<string, Color> colors = new Dictionary<string, Color>();
        }
        private SkinData _skinData;
        private Dictionary<string, Texture2D> _textureTable;
        private Dictionary<string, Color> _colorTexture;
        private string _filePath;
        public Skin(string configPath) {
            _skinData = new SkinData();
            _textureTable = new Dictionary<string, Texture2D>();
            _colorTexture = new Dictionary<string, Color>();
            _filePath = Path.Combine("UILib/Resources/Skins", configPath);
            string config;
            using (StreamReader sr = new StreamReader(UIEditor.Instance.GetFileStream(_filePath))) {
                config = sr.ReadToEnd();
            }
            _skinData = JsonConvert.DeserializeObject<SkinData>(config);

            foreach (var path in _skinData.textures) {
                _textureTable.Add(path.Key, ModContent.GetTexture("UIEditor/" + path.Value));
            }
            _colorTexture = _skinData.colors;
        }

        public void Save() {
            using (var sw = new StreamWriter("test.json")) {
                sw.Write(JsonConvert.SerializeObject(_skinData, Formatting.Indented));
            }
        }

        public Color GetColor(string name) {
            if (!_colorTexture.ContainsKey(name)) throw new ArgumentException($"没有这个颜色名字: {name}");
            return _colorTexture[name];
        }

        public Texture2D TryGetTexture(string name) {
            if (!_textureTable.ContainsKey(name)) throw new ArgumentException($"没有这个贴图名字: {name}");
            return _textureTable[name];
        }
    }
}
