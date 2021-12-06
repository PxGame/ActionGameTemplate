/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 21:24:53
 */

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
        public static void ApplyWireMaterial()
        {
            var method = typeof(HandleUtility).GetMethod("ApplyWireMaterial", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, Array.Empty<Type>(), Array.Empty<ParameterModifier>());
            method.Invoke(null, new object[] { });
        }

        public static void IncrementVersion(this VisualElement ve, VersionChangeType changeType)
        {
            var method = typeof(VisualElement).GetMethod("IncrementVersion", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(ve, new object[] { (int)changeType });
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