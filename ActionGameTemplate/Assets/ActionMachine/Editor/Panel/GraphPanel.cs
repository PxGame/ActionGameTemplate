/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 21:41:26
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// GraphPanel
    /// </summary>
    public class GraphPanel : PanelElement
    {
        private VisualElement _graphViewRoot;
        private ActionGraphView _graphView;

        public override string uxmlPath => "Panel/GraphPanel";

        public GraphPanel() : base()
        {
            _graphViewRoot = this.Q("graph-view-root");

            _graphView = new ActionGraphView();
            _graphViewRoot.Add(_graphView);
            _graphView.StretchToParentSize();
        }

        public new class UxmlFactory : PanelElement.UxmlFactory<GraphPanel, UxmlTraits>
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