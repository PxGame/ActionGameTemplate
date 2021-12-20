/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/9 21:07:07
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ActionMachineWindow
    /// </summary>
    public class ActionMachineWindow : EditorWindow
    {
        private void Awake()
        {
            ActionMachineManager.Reset();
        }

        private void OnEnable()
        {
        }

        protected void CreateGUI()
        {
            ActionMachineManager.Initialize(rootVisualElement);
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {
        }
    }
}