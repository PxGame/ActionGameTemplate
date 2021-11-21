/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/24 0:57:27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ActionGraphView
    /// </summary>
    public class ActionGraphView : GraphView
    {
        public ActionGraphView() : base()
        {
            ActionGridBackground gridBackground = new ActionGridBackground();
            Insert(0, gridBackground);
            gridBackground.StretchToParentSize();

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }
    }

    public class ActionGridBackground : GridBackground
    { }
}