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
    /// WorldPage
    /// </summary>
    public class WorldPage : AGTToolPage
    {
        public override string title => "世界";
        private string selectModuleName;

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }

        public override void OnGUI()
        {
            if (Game.gw == null)
            {
                GUILayout.Label("GameWorld 未创建");
                return;
            }

            using (var lay = new EditorGUILayout.VerticalScope())
            {
                string[] moduleNames = Game.gw.debugPageDict.Keys.ToArray();
                int lastSelectIndex = Array.FindIndex(moduleNames, t => 0 == string.Compare(t, selectModuleName));
                lastSelectIndex = Mathf.Clamp(lastSelectIndex, -1, moduleNames.Length - 1);
                int selectIndex = GUILayout.SelectionGrid(lastSelectIndex, moduleNames, moduleNames.Length, GUI.skin.FindStyle("ToolBarButton"));
                selectModuleName = selectIndex >= 0 ? moduleNames[selectIndex] : (moduleNames.Length > 0 ? moduleNames[0] : string.Empty);

                if (!string.IsNullOrEmpty(selectModuleName))
                {
                    Action page = Game.gw.debugPageDict[selectModuleName];
                    page();
                }
            }
        }

        public override void OnInit()
        {
        }
    }
}