/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 21:41:02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// TimelinePanel
    /// </summary>
    public class TimelinePanel : BindablePanelElement
    {
        public override string uxmlPath => "Panel/TimelinePanel";
        private Timeline _timeline;

        public TimelinePanel() : base()
        {
            _timeline = this.Q<Timeline>("timeline");
        }

        protected override void OnPropertyChanged()
        {
            base.OnPropertyChanged();

            _timeline.Inspect(property);
        }

        public new class UxmlFactory : PanelElement.UxmlFactory<TimelinePanel, UxmlTraits>
        {
        }

        public new class UxmlTraits : PanelElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}