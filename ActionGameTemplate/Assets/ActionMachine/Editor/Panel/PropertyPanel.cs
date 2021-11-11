/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 21:42:02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// PropertyPanel
    /// </summary>
    public class PropertyPanel : VisualElement
    {
        public PropertyPanel()
        {
            var uxml = ResourceUtility.LoadUXML("PropertyPanel");
            uxml.CloneTree(this);
        }

        public new class UxmlFactory : UxmlFactory<PropertyPanel, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var propertyPanel = ve as PropertyPanel;
            }
        }
    }
}