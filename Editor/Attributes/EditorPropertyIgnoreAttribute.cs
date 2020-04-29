using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.Editor.Attributes {
    /// <summary>
    /// 被EditorPropertyIgnore指定的成员属性会在编辑器显示的属性界面中被忽略
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class EditorPropertyIgnoreAttribute : Attribute {
        public EditorPropertyIgnoreAttribute() {
        }
    }
}
