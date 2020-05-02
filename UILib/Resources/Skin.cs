using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace UIEditor.UILib.Resources {
    public class Skin {
        private Dictionary<string, string> _texturePath;
        private Dictionary<string, Texture2D> _textureTable;
        private string _filePath;
        public Skin(string configPath) {
            _texturePath = new Dictionary<string, string>();
            _textureTable = new Dictionary<string, Texture2D>();
            _filePath = Path.Combine("UILib/Resources/Skins", configPath);
            string config;
            using (StreamReader sr = new StreamReader(UIEditor.Instance.GetFileStream(_filePath))) {
                config = sr.ReadToEnd();
            }
            _texturePath = JsonConvert.DeserializeObject<Dictionary<string, string>>(config);

            foreach (var path in _texturePath) {
                _textureTable.Add(path.Key, ModContent.GetTexture("UIEditor/" + path.Value));
            }
        }

        public void Save() {
            using var sw = new StreamWriter(_filePath);
            sw.Write(JsonConvert.SerializeObject(_texturePath));
        }

        public Texture2D TryGetTexture(string name) {
            if (!_textureTable.ContainsKey(name)) throw new ArgumentException($"没有这个贴图名字: {name}");
            return _textureTable[name];
        }
    }
}
