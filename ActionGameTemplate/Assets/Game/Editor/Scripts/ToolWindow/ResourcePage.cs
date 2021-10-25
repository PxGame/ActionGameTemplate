/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/26 2:18:45
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using XMLib;

namespace AGT
{
    public partial class AGTToolSetting
    {
        public ResourcePage.Setting resource = new ResourcePage.Setting();
    }

    /// <summary>
    /// ResourcePage
    /// </summary>
    public class ResourcePage : AGTToolPage
    {
        [System.Serializable]
        public class Setting
        {
            public string goRootDir;
            public string settingOutFilePath;
        }

        public override string title => "资源";

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }

        public override void OnGUI()
        {
            setting.resource.goRootDir = EditorGUILayout.TextField("预制体根目录", setting.resource.goRootDir);
            setting.resource.settingOutFilePath = EditorGUILayout.TextField("配置文件输出", setting.resource.settingOutFilePath);
            if (GUILayout.Button("导出资源配置"))
            {
                ExportResourceSetting();
            }
        }

        private void ExportResourceSetting()
        {
            if (FindResourcesInPath(setting.resource.goRootDir) < 0)
            {
                throw new RuntimeException($"资源路径中不包含 Resources 目录：{setting.resource.goRootDir}");
            }

            string[] searchFolders = { setting.resource.goRootDir };
            string[] guids = AssetDatabase.FindAssets("t:prefab", searchFolders);

            Dictionary<string, string> name2path = new Dictionary<string, string>();

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                IResourceItem item = obj.GetComponent<IResourceItem>();
                if (item == null)
                {
                    throw new RuntimeException($"{path} 资源没有继承 {nameof(IResourceItem)} 接口的组件");
                }
                else if (string.IsNullOrEmpty(item.poolTag))
                {
                    throw new RuntimeException($"{path} 资源没有设置 Tag");
                }
                else if (name2path.TryGetValue(item.poolTag, out var sameTagPath))
                {
                    throw new RuntimeException($"{path} 资源设置的 Tag 与 {sameTagPath} 资源重复");
                }

                name2path.Add(item.poolTag, path);
            }

            ResourceData data = new ResourceData();
            foreach (var item in name2path)
            {
                string relativePath = GetRelativeResourcesPath(item.Value);
                data.tag2path[item.Key] = relativePath;
            }

            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(setting.resource.settingOutFilePath, json);
            AssetDatabase.Refresh();
        }

        public const string ReourcesFolderName = "Resources";

        public int FindResourcesInPath(string path)
        {
            int index = path.IndexOf("resources", StringComparison.OrdinalIgnoreCase);
            return index;
        }

        public string GetRelativeResourcesPath(string path)
        {
            int index = FindResourcesInPath(path);
            if (index < 0)
            {
                throw new RuntimeException($"资源路径中不包含 Resources 目录：{path}");
            }
            index += ReourcesFolderName.Length + 1;
            path = path.Substring(index);

            string dir = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            path = Path.Combine(dir, fileName).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            return path;
        }

        public override void OnInit()
        {
        }
    }
}