/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/28 21:42:33
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// DebugTool
    /// </summary>
    public class DebugTool : MonoBehaviour
    {
#if UNITY_EDITOR
        private Dictionary<string, Action> _pageDict = new Dictionary<string, Action>();
        private List<Action> _gizmosList = new List<Action>();
        private List<Action> _updateList = new List<Action>();

        private void Update()
        {
            UpdateInvoke(_updateList);
        }

        private void OnDrawGizmos()
        {
            UpdateInvoke(_gizmosList);
        }

        private void UpdateInvoke(List<Action> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                Action action = actions[i];
                if (action == null || (!action.Method.IsStatic && action.Target == null))
                {
                    actions.RemoveAt(i);
                    i--;
                    return;
                }
                action.Invoke();
            }
        }

        public static bool isInited => instance != null;

        private static DebugTool instance;

        public static IReadOnlyDictionary<string, Action> pageDict => instance._pageDict;

        private static void Init()
        {
            if (instance != null) { return; }
            GameObject obj = new GameObject("DebugTool");
            GameObject.DontDestroyOnLoad(obj);
            instance = obj.AddComponent<DebugTool>();
        }

        [Conditional("UNITY_EDITOR")]
        public static void RunPage(string pageName)
        {
            if (!instance._pageDict.TryGetValue(pageName, out Action page)) { return; }
            if (page == null || (!page.Method.IsStatic && page.Target == null))
            {
                instance._pageDict.Remove(pageName);
                return;
            }
            page.Invoke();
        }

        [Conditional("UNITY_EDITOR")]
        public static void AddPage(string pageName, Action onPage)
        {
            Init();
            instance._pageDict.Add(pageName, onPage);
        }

        [Conditional("UNITY_EDITOR")]
        public static void AddGizmo(Action onGizmo)
        {
            Init();
            instance._gizmosList.Add(onGizmo);
        }

        [Conditional("UNITY_EDITOR")]
        public static void AddUpdate(Action onUpdate)
        {
            Init();
            instance._updateList.Add(onUpdate);
        }

#endif
    }
}