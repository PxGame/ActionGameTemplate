/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/12/19 14:51:00
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace XMLib.AM
{
    /// <summary>
    /// DataManager
    /// </summary>
    public class DataManager
    {
        public Action onPackageChanged;
        public Action onPropertyChanged;
        public Action onStateChanged;

        public Action<SerializedPropertyChangeEvent> onPropertyValueChanged;

        public ActionMachinePackage package { get; private set; }

        public SerializedObject serializedObject { get; private set; }

        public SerializedProperty currentProperty { get; private set; }
        public SerializedProperty currentState { get; private set; }

        public void RecordSource()
        {
            Undo.RecordObject(package, "ActionMachineData");
        }

        public void SetPackage(ActionMachinePackage package)
        {
            this.package = package;

            if (package != null)
            {
                serializedObject = new SerializedObject(package);
            }
            else
            {
                serializedObject = null;
            }

            onPackageChanged?.Invoke();

            InspectState(null);
        }

        #region property

        public void InspectProperty(SerializedProperty property)
        {
            currentProperty = property;
            onPropertyChanged?.Invoke();
        }

        #endregion property

        #region state

        public void InspectState(SerializedProperty property)
        {
            currentState = property;
            onStateChanged?.Invoke();

            var setting = property?.FindPropertyRelative("setting");
            InspectProperty(setting);
        }

        public void AddNewState()
        {
            package.data.states.Add(EditorTool.CreateStateData());
        }

        public void InsertNewState(int index)
        {
            package.data.states.Insert(index, EditorTool.CreateStateData());
        }

        public void RemoveStateAt(int index)
        {
            package.data.states.RemoveAt(index);
        }

        #endregion state
    }
}