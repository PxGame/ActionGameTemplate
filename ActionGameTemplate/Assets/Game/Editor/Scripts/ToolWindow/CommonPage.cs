/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/25 1:26:30
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
    /// CommonPage
    /// </summary>
    public class CommonPage : AGTToolPage
    {
        public override string title => "公共";

        public List<Type> appInitializerTypes = new List<Type>();

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }

        public override void OnGUI()
        {
            GUILayout.BeginVertical();
            foreach (var item in appInitializerTypes)
            {
                EditorGUILayout.TextField(item.GetTypeName());
            }
            GUILayout.EndVertical();
        }

        public override void OnInit()
        {
            appInitializerTypes = AssemblyUtility.FindAllAssignable<IAppInitializer>();
        }
    }
}