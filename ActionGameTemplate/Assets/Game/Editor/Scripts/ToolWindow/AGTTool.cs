/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/25 1:14:41
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// AGTTooll
    /// </summary>
    public class AGTTool : ToolWindow
    {
        [MenuItem("AGT/Tools")]
        public static void OpenTool()
        {
            var win = GetWindow<AGTTool>();
            win.titleContent = new GUIContent("AGT 工具");
            win.Show();
        }

        protected override void Init()
        {
            setting = new AGTToolSetting();
            AppendPagesFromBaseType<AGTToolPage>();
        }
    }

    [Serializable]
    public partial class AGTToolSetting : ToolWindow.ISetting
    {
        public int selectPage { get; set; }
    }

    public abstract class AGTToolPage : ToolPage
    {
        public AGTToolSetting setting => (AGTToolSetting)main.setting;
    }
}