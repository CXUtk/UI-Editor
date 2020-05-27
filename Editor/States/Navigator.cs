using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.Editor.Components;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Events;

namespace UIEditor.Editor.States {
    public class Navigator : UIEditorPart {
        public Navigator(EditorState editor) : base(editor) {
            var save = new UIImageButton() {
                Texture = UIEditor.Instance.SkinManager.GetTexture("NewState"),
                WhiteTexture = UIEditor.Instance.SkinManager.GetTexture("NewState_White"),
                Size = new Vector2(30f, 30f),
                DefaultColor = Color.White * 0.8f,
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                Position = new Vector2(20, 0),
                Tooltip = "保存"
            };
            save.OnClick += Save_OnClick;
            AppendChild(save);
        }

        public override void Initialize() {

        }

        private void Save_OnClick(UIMouseEvent e, UIElement sender) {
            //var list = Editor.Viewer.Canvas.Root.Children;
            //var text = JsonConvert.SerializeObject(list, Formatting.Indented);
            //using (StreamWriter sw = new StreamWriter(File.Open("test.json", FileMode.OpenOrCreate), Encoding.UTF8)) {
            //    sw.Write(text);
            //}
            Main.NewText("这个功能没有实现！", Color.Red);
        }
    }
}
