/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/21 0:47:57
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// MainManipulator
    /// </summary>
    public class MainManipulator : Manipulator
    {
        private MainElement mainElement => (MainElement)target;

        public MainManipulator()
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