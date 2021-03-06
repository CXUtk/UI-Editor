﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEditor.Editor.Attributes {
    /// <summary>
    /// 如果某个UI类型具有此特性，则在层级浏览器中不会显示其子节点
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EditorPropertyReadOnlyAttribute : Attribute {
        public EditorPropertyReadOnlyAttribute() {
        }
    }
}
