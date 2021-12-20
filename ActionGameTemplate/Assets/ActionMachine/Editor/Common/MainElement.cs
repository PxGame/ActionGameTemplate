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
            var uxml = EditorTool.LoadUXML("MainElement");
            uxml.CloneTree(this);

            this.StretchToParentSize();
            this.AddManipulator(new MainManipulator());

            panelContainer = this.Q("panel-container");

            menu = this.Q<MenuElement>("menu");
            state = this.Q<StatePanel>("panel-state");
            property = this.Q<PropertyPanel>("panel-property");
            timeline = this.Q<TimelinePanel>("panel-timeline");
            graph = this.Q<GraphPanel>("panel-graph");

            ShowPanel(false);

            ActionMachineManager.data.onPackageChanged += OnPackageChanged;
        }

        public void ShowPanel(bool show)
        {
            panelContainer.style.visibility = show ? Visibility.Visible : Visibility.Hidden;
            panelContainer.SetEnabled(show);
        }

        public void OnPackageChanged()
        {
            ShowPanel(ActionMachineManager.data.package != null);
        }
    }
}