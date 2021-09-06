/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/25 1:25:34
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

namespace XMLib
{
    /// <summary>
    /// ResolvedFieldInspecter
    /// </summary>
    public class ResolvedFieldInspecter : VisualElement
    {
        private ScrollView propertyScrollView;

        public override VisualElement contentContainer => propertyScrollView.contentContainer;

        public ResolvedFieldInspecter()
        {
            propertyScrollView = new ScrollView(ScrollViewMode.Vertical);
            propertyScrollView.StretchToParentSize();
            hierarchy.Add(propertyScrollView);
        }

        public new class UxmlFactory : UxmlFactory<ResolvedFieldInspecter, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //private UxmlObjectAttributeDescription m_object = new UxmlObjectAttributeDescription { name = "object", defaultValue = "" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                //ObjectInspector objectInspector = ve as ObjectInspector;
                base.Init(ve, bag, cc);
            }
        }
    }
}