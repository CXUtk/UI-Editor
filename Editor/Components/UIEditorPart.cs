using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.Editor.States;
using UIEditor.UILib;

namespace UIEditor.Editor.Components {
    public abstract class UIEditorPart : UIElement {
        public EditorState Editor { get; }
        public UIEditorPart(EditorState editor) {
            Editor = editor;
        }
        public abstract void Initialize();
    }
}
