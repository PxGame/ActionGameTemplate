/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 17:42:50
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// PanelManipulator
    /// </summary>
    public class PanelManipulator : Manipulator
    {
        private PanelElement panel => (PanelElement)target;

        public PanelManipulator()
        {
        }

        protected override void RegisterCallbacksOnTarget()
        {
        }

        protected override void UnregisterCallbacksFromTarget()
        {
        }
    }
}