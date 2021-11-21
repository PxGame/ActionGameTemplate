/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 17:10:49
 */

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XMLib.AM
{
    /// <summary>
    /// ActionMachineManager
    /// </summary>
    public class ActionMachineManager
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
            inst.CreateFile();
        }

        public static void Reset()
        {
            _instance = null;
        }

        private static ActionMachineManager _instance;

        public static ActionMachineManager inst
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionMachineManager();
                }
                return _instance;
            }
        }

        public static Operation opt => inst._operation;

        private MainElement _main;

        public MainElement main
        {
            get
            {
                if (_main == null)
                {
                    _main = EditorWindow.GetWindow<ActionMachineWindow>().main;
                }
                return _main;
            }
            private set
            {
                _main = value;
            }
        }

        public SerializedObject data { get; private set; }

        private ActionMachinePackage _source;

        public ActionMachinePackage source
        {
            get => _source;
            set
            {
                _source = value;
                data = _source != null ? new SerializedObject(_source) : null;
                using (var evt = PackageChanged.GetPooled(main, value))
                {
                    main.SendEvent(evt);
                }
            }
        }

        private Operation _operation = new Operation();

        private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public ActionMachineData ReadData(TextAsset textAsset)
        {
            return JsonConvert.DeserializeObject<ActionMachineData>(textAsset.text, _jsonSettings);
        }

        public void WriteData(ActionMachineData data, TextAsset textAsset)
        {
            string jsonData = JsonConvert.SerializeObject(data, _jsonSettings);
            File.WriteAllText(AssetDatabase.GetAssetPath(textAsset), jsonData);
            EditorUtility.SetDirty(textAsset);
            AssetDatabase.Refresh();
        }

        public void RecordSource()
        {
            Undo.RecordObject(source, "ActionMachineData");
        }

        public string CreateFile()
        {
            string filePath = EditorTool.CreateValidFilePathFromSelectDirectory("ActionMachine.json");
            string jsonData = JsonConvert.SerializeObject(ActionMachineData.Default, _jsonSettings);
            File.WriteAllText(filePath, jsonData);
            AssetDatabase.Refresh();
            return filePath;
        }

        public StateData CreateStateData()
        {
            var state = new StateData()
            {
                setting = new StateSettingData()
                {
                    stateName = $"State({Random.Range(0, 100)})"
                }
            };

            return state;
        }

        public class Operation
        {
            public MainElement main => inst.main;

            public void Inspect(SerializedProperty property)
            {
                main.property.Inspect(property);
            }
        }
    }
}