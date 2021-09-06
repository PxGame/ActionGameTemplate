/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/19 0:09:29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Reflection;
using UnityEditor.UIElements;

namespace XMLib.AM2
{
    /// <summary>
    /// ActionEditorWindow
    /// </summary>
    public class ActionEditorWindow : EditorWindow
    {
        [MenuItem("XMLib/动作编辑2")]
        public static void Open()
        {
            var win = GetWindow<ActionEditorWindow>();
            win.Show();
        }

        public SerializedObject so;

        private void OnEnable()
        {
            so = new SerializedObject(this);

            var uxml = Resources.Load<VisualTreeAsset>("ActionMachine2/ActionEditorWindow");
            uxml.CloneTree(rootVisualElement);

            var objInspector = rootVisualElement.Q<ResolvedFieldInspecter>("ResolvedObjectInspecter");
        }

        private void OnGUI()
        {
        }
    }
}