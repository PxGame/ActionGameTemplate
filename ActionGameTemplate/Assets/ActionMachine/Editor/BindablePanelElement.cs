/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/12/4 21:46:24
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// BindablePanelElement
    /// </summary>
    public abstract class BindablePanelElement : PanelElement
    {
        protected SerializedProperty property { get; private set; }

        public BindablePanelElement() : base()
        {
        }

        protected override void ExecuteDefaultAction(EventBase evt)
        {
            if (property != null && !property.IsValid())
            {
                Inspect(null);
            }

            base.ExecuteDefaultAction(evt);
        }

        public void Inspect(SerializedProperty target)
        {
            if (target == property) { return; }
            if (target != null && !target.IsValid()) { throw new Exception("传入的属性无效"); }

            property = target;

            OnPropertyChanged();
        }

        protected virtual void OnPropertyChanged()
        {
        }
    }
}