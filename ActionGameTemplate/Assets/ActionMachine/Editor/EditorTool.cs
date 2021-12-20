/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 21:24:53
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    [Flags]
    public enum VersionChangeType
    {
        Bindings = 0x1,
        ViewData = 0x2,
        Hierarchy = 0x4,
        Layout = 0x8,
        StyleSheet = 0x10,
        Styles = 0x20,
        Overflow = 0x40,
        BorderRadius = 0x80,
        BorderWidth = 0x100,
        Transform = 0x200,
        Size = 0x400,
        Repaint = 0x800,
        Opacity = 0x1000
    }

    /// <summary>
    /// Tools
    /// </summary>
    public static class EditorTool
    {
        [MenuItem("XMLib/Action Machine")]
        private static void ShowActionMachine()
        {
            ActionMachineWindow win = EditorWindow.GetWindow<ActionMachineWindow>();
            win.Show();
        }

        [MenuItem("XMLib/ActionMachine/创建ActionMachine配置文件")]
        public static void CreateFileMenu()
        {
            CreateFile();
        }

        public static VisualTreeAsset LoadUXML(string name)
        {
            string path = string.Format("UXML/{0}", name);
            return Resources.Load<VisualTreeAsset>(path);
        }

        public static StyleSheet LoadUSS(string name)
        {
            string path = string.Format("USS/{0}", name);
            return Resources.Load<StyleSheet>(path);
        }

        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static ActionMachineData ReadData(TextAsset textAsset)
        {
            return JsonConvert.DeserializeObject<ActionMachineData>(textAsset.text, _jsonSettings);
        }

        public static void WriteData(ActionMachineData data, TextAsset textAsset)
        {
            string jsonData = JsonConvert.SerializeObject(data, _jsonSettings);
            File.WriteAllText(AssetDatabase.GetAssetPath(textAsset), jsonData);
            EditorUtility.SetDirty(textAsset);
            AssetDatabase.Refresh();
        }

        public static string CreateFile()
        {
            string filePath = EditorTool.CreateValidFilePathFromSelectDirectory("ActionMachine.json");
            string jsonData = JsonConvert.SerializeObject(ActionMachineData.Default, _jsonSettings);
            File.WriteAllText(filePath, jsonData);
            AssetDatabase.Refresh();
            return filePath;
        }

        public static StateData CreateStateData()
        {
            var state = new StateData()
            {
                setting = new StateSettingData()
                {
                    stateName = $"State({UnityEngine.Random.Range(0, 100)})"
                }
            };

            return state;
        }

        public static void ApplyWireMaterial()
        {
            var method = typeof(HandleUtility).GetMethod("ApplyWireMaterial", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, Array.Empty<Type>(), Array.Empty<ParameterModifier>());
            method.Invoke(null, new object[] { });
        }

        public static float RoundToPixelGrid(float v)
        {
            var method = typeof(GUIUtility).GetMethod("RoundToPixelGrid", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            return (float)method.Invoke(null, new object[] { v });
        }

        public static bool IsValid(this SerializedProperty property)
        {
            var isValid = typeof(SerializedProperty).GetProperty("isValid", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)isValid.GetValue(property);
        }

        public static string CreateValidFilePathFromSelectDirectory(string fileName)
        {
            string dir = GetSelectDirectory();
            if (string.IsNullOrEmpty(dir)) { return string.Empty; }
            string path = Path.Combine(dir, fileName);
            return ValidFilePath(path);
        }

        /// <summary>
        /// 获取选择的文件夹
        /// </summary>
        /// <returns></returns>
        public static string GetSelectDirectory()
        {
            string[] strs = Selection.assetGUIDs;
            if (strs.Length == 0) { return "Assets"; }
            string resourceDirectory = AssetDatabase.GUIDToAssetPath(strs[0]);
            if (string.IsNullOrEmpty(resourceDirectory) || !Directory.Exists(resourceDirectory)) { return "Assets"; }
            return resourceDirectory;
        }

        /// <summary>
        /// 获取选择的文件
        /// </summary>
        /// <param name="suffixFilter"></param>
        /// <returns></returns>
        public static string GetSelectFile(params string[] suffixFilter)
        {
            string[] strs = Selection.assetGUIDs;
            if (strs.Length == 0) { return string.Empty; }
            string resourceFile = AssetDatabase.GUIDToAssetPath(strs[0]);
            if (!File.Exists(resourceFile)) { return string.Empty; }
            string suffix = Path.GetExtension(resourceFile);
            if (suffixFilter.Length > 0 && Array.Exists(suffixFilter, t => 0 == string.Compare(t, suffix, true))) { return resourceFile; }
            return string.Empty;
        }

        public static string ValidFilePath(string filePath)
        {
            int index = 0;
            string target = filePath;
            do
            {
                if (!File.Exists(target)) { return target; }

                string ext = Path.GetExtension(filePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string dir = Path.GetDirectoryName(filePath).Replace('\\', '/');

                index++;
                target = $"{dir}/{fileName}_{index}{ext}";
            }
            while (true);
        }
    }
}