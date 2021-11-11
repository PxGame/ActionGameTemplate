/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 21:41:02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// TimelinePanel
    /// </summary>
    public class TimelinePanel : VisualElement
    {
        public TimelinePanel()
        {
            var uxml = ResourceUtility.LoadUXML("TimelinePanel");
            uxml.CloneTree(this);
        }

        public new class UxmlFactory : UxmlFactory<TimelinePanel, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var timelinePanel = ve as TimelinePanel;
            }
        }
    }
}