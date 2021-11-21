/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/21 1:58:07
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ActionMachineEditorData
    /// </summary>
    [CreateAssetMenu(menuName = "ActionMachinePackage")]
    public class ActionMachinePackage : ScriptableObject
    {
        public TextAsset actionMachineData;
        public ActionMachineData data;
    }
}