/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 23:35:09
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// MenuElement
    /// </summary>
    public class MenuElement : VisualElement
    {
        public MenuElement()
        {
            var uxml = ResourceUtility.LoadUXML("MenuElement");
            uxml.CloneTree(this);
        }

        public new class UxmlFactory : UxmlFactory<MenuElement, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var menuElement = ve as MenuElement;
            }
        }
    }
}