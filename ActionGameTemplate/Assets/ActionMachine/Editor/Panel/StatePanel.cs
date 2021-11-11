/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 21:40:44
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// StatePanel
    /// </summary>
    public class StatePanel : VisualElement
    {
        public StatePanel()
        {
            var uxml = ResourceUtility.LoadUXML("StatePanel");
            uxml.CloneTree(this);
        }

        public new class UxmlFactory : UxmlFactory<StatePanel, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var statePanel = ve as StatePanel;
            }
        }
    }
}