/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/22 0:53:45
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ActionMachinePackageEditor
    /// </summary>
    [CustomEditor(typeof(ActionMachinePackage))]
    public class ActionMachinePackageEditor : Editor
    {
        private VisualElement rootElement;

        public override VisualElement CreateInspectorGUI()
        {
            rootElement = new VisualElement();

            var uxml = ResourceUtility.LoadUXML("ActionMachineEditorData");
            uxml.CloneTree(rootElement);
            rootElement.Bind(serializedObject);

            var btnRead = rootElement.Q<Button>("btn-read");
            var btnWrite = rootElement.Q<Button>("btn-write");

            btnRead.clicked += BtnRead_clicked;
            btnWrite.clicked += BtnWrite_clicked;

            return rootElement;
        }

        private void BtnRead_clicked()
        {
            var data = target as ActionMachinePackage;
            if (data == null || data.actionMachineData == null)
            {
                EditorUtility.DisplayDialog("输入读取", "失败！", "确定");
                return;
            }

            data.data = ActionMachineManager.inst.ReadData(data.actionMachineData);
            serializedObject.Update();
            rootElement.MarkDirtyRepaint();
        }

        private void BtnWrite_clicked()
        {
            serializedObject.Update();
            var data = target as ActionMachinePackage;
            if (data == null || data.data == null || data.actionMachineData == null)
            {
                EditorUtility.DisplayDialog("输入写入", "失败！", "确定");
                return;
            }

            ActionMachineManager.inst.WriteData(data.data, data.actionMachineData);
        }
    }
}