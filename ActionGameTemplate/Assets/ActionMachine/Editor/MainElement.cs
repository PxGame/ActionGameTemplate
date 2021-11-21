/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/21 0:50:29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace XMLib.AM
{
    /// <summary>
    /// MainElement
    /// </summary>
    public class MainElement : VisualElement
    {
        public VisualElement panelContainer { get; protected set; }
        public MenuElement menu { get; protected set; }
        public StatePanel state { get; protected set; }
        public PropertyPanel property { get; protected set; }
        public TimelinePanel timeline { get; protected set; }
        public GraphPanel graph { get; protected set; }

        public MainElement()
        {
            var uxml = ResourceUtility.LoadUXML("MainElement");
            uxml.CloneTree(this);

            this.StretchToParentSize();
            this.AddManipulator(new MainManipulator());

            panelContainer = this.Q("panel-container");
            panelContainer.visible = false;
            panelContainer.SetEnabled(false);

            menu = this.Q<MenuElement>("menu");
            state = this.Q<StatePanel>("panel-state");
            property = this.Q<PropertyPanel>("panel-property");
            timeline = this.Q<TimelinePanel>("panel-timeline");
            graph = this.Q<GraphPanel>("panel-graph");
        }

        public void Initialize()
        {
            var panels = this.Query<BaseElement>();
            panels.ForEach(t =>
            {
                using (var evt = InitEvent.GetPooled(t, this))
                {
                    t.SendEvent(evt);
                }
            });
        }

        public void OnPackageChanged(ActionMachinePackage package)
        {
            bool hasData = package != null;
            panelContainer.SetEnabled(hasData);
            panelContainer.visible = hasData;
        }
    }
}