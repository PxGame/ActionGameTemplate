/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/25 12:52:39
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using System.Linq;
using UnityEditor;
using System;

namespace AGT
{
    /// <summary>
    /// RuntimePage
    /// </summary>
    public class RuntimePage : AGTToolPage
    {
        public override string title => "运行时";
        private string selectPageName;

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }

        public override void OnGUI()
        {
            if (!DebugTool.isInited)
            {
                GUILayout.Label("DebugTool 未创建");
                return;
            }

            using (var lay = new EditorGUILayout.VerticalScope())
            {
                string[] pageNames = DebugTool.pageDict.Keys.ToArray();
                int lastSelectIndex = Array.FindIndex(pageNames, t => 0 == string.Compare(t, selectPageName));
                lastSelectIndex = Mathf.Clamp(lastSelectIndex, -1, pageNames.Length - 1);
                int selectIndex = GUILayout.SelectionGrid(lastSelectIndex, pageNames, pageNames.Length, GUI.skin.FindStyle("ToolBarButton"));
                selectPageName = selectIndex >= 0 ? pageNames[selectIndex] : (pageNames.Length > 0 ? pageNames[0] : string.Empty);
                DebugTool.RunPage(selectPageName);
            }
        }

        public override void OnInit()
        {
        }
    }
}