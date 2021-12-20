/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 17:10:49
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ActionMachineManager
    /// </summary>
    public class ActionMachineManager
    {
        public static ActionMachineManager inst
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionMachineManager();
                }
                return _instance;
            }
        }

        public static void Initialize(VisualElement rootVisualElement)
        {
            inst._main = new MainElement();
            rootVisualElement.Add(main);
            onInitialized?.Invoke();
        }

        public static void Reset()
        {
            _instance = null;
        }

        private static ActionMachineManager _instance;

        public static Action onInitialized;
        public static DataManager data => inst._data;
        public static MainElement main => inst._main;
        public static Operation opt => inst._operation;

        private MainElement _main;
        private DataManager _data = new DataManager();
        private Operation _operation = new Operation();

        public class Operation
        {
        }
    }
}