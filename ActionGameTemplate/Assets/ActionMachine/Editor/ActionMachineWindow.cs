/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/9 21:07:07
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XMLib.AM
{
    /// <summary>
    /// ActionMachineWindow
    /// </summary>
    public class ActionMachineWindow : EditorWindow
    {
        [MenuItem("XMLib/Action Machine")]
        protected static void ShowMenu()
        {
            var win = GetWindow<ActionMachineWindow>();
            win.Show();
        }

        protected void CreateGUI()
        {
            var uxml = ResourceUtility.LoadUXML("ActionMachineWindow");
            uxml.CloneTree(rootVisualElement);
        }
    }
}