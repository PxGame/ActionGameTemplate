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
    public class PanelElement : VisualElement
    {
        public override VisualElement contentContainer => _body;

        protected VisualElement _body;
        protected Label _title;

        public string titleText { get => _title.text; set => _title.text = value; }

        public PanelElement()
        {
            var uxml = ResourceUtility.LoadUXML("PanelElement");
            uxml.CloneTree(this);

            _body = this.Q("body");
            _title = this.Q<Label>("title");
        }

        public new class UxmlFactory : UxmlFactory<PanelElement, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public UxmlStringAttributeDescription _title = new UxmlStringAttributeDescription() { name = "title-text", defaultValue = "标题" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var panelElement = ve as PanelElement;
                panelElement.titleText = _title.GetValueFromBag(bag, cc);
            }
        }
    }
}