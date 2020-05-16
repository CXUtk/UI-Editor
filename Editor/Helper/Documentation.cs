using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Terraria;

namespace UIEditor.Editor.Helper {
    internal class Documentation {
        private Dictionary<string, XmlNode> _propertyDocs;
        private const string FilePath = "UIEditor.xml";
        internal Documentation() {
            _propertyDocs = new Dictionary<string, XmlNode>();
        }
        internal void Load() {
            string text;
            using (StreamReader sr = new StreamReader(UIEditor.Instance.GetFileStream(FilePath))) {
                text = sr.ReadToEnd();
            }
            XmlDocument document = new XmlDocument();
            document.LoadXml(text);
            var memberNode = document.SelectSingleNode("doc/members");
            foreach (XmlNode node in memberNode.ChildNodes) {
                string name = node.Attributes["name"].Value;
                if (name.Substring(0, 1) == "P") {
                    _propertyDocs.Add(name.Substring(2, name.Length - 2), node);
                }
            }
        }
        internal XmlNode GetPropertyInfo(string fullname) {
            if (!_propertyDocs.ContainsKey(fullname)) {
                return null;
            }
            return _propertyDocs[fullname];
        }

    }
}
