/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/10 23:11:16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace XMLib.AM
{
    /// <summary>
    /// PanelElement
    /// </summary>
    public abstract class PanelElement : BaseElement
    {
        public abstract string uxmlPath { get; }

        public override VisualElement contentContainer => _module?.contentContainer;

        protected ModuleElement _module;
        public string titleText => _module?.titleText;

        public PanelElement()
        {
            var uxml = EditorTool.LoadUXML(uxmlPath);
            uxml.CloneTree(this);

            _module = this.Q<ModuleElement>("module");

            this.AddManipulator(new PanelManipulator());
        }

        public class UxmlFactory<T, D> : UnityEngine.UIElements.UxmlFactory<T, D>
            where T : PanelElement, new()
            where D : UxmlTraits, new()
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public UxmlStringAttributeDescription _title = new UxmlStringAttributeDescription() { name = "title-text", defaultValue = "标题" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}