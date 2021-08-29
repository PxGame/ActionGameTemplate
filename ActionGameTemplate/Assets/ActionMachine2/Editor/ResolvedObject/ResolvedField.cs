/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/29 18:33:13
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// ResolvedField
    /// </summary>
    public class ResolvedField
    {
        private ResolvedObject _resolvedObject;
        private string _fieldPath;

        public ResolvedObject resolvedObject { get => _resolvedObject; }
        public string fieldPath { get => _fieldPath; }

        public override string ToString()
        {
            int hashCode = resolvedObject.target.GetHashCode();
            object value = GetValue();
            Type valueType = value.GetType();
            return $"[{hashCode}]({fieldPath}):{value}|{valueType}";
        }

        public ResolvedField(ResolvedObject resolvedObject, string fieldPath)
        {
            _resolvedObject = resolvedObject;
            _fieldPath = fieldPath;
        }

        public object SetValue(object value)
        {
            string[] paths = fieldPath.Split(ResolvedObject.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);

            object obj = resolvedObject.target;

            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                object parent = obj;

                if (path[0] == ResolvedObject.listItemFlag)
                {
                    string indexStr = path.Remove(0, 1);
                    int index = int.Parse(indexStr);

                    if (parent is not IList list)
                    {
                        throw new RuntimeException($"实例不是 IList 类型，但路径节点为列表元素，所以无法获取对应路径上的实例");
                    }

                    obj = list[index];

                    if (paths.Length == i + 1)
                    {
                        list[index] = value;
                    }
                }
                else
                {
                    FieldInfo info = parent.GetType().GetField(path);
                    obj = info.GetValue(parent);

                    if (paths.Length == i + 1)
                    {
                        info.SetValue(parent, value);
                    }
                }
            }

            return obj;
        }

        public object GetValue()
        {
            string[] paths = fieldPath.Split(ResolvedObject.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);

            object obj = resolvedObject.target;
            foreach (var path in paths)
            {
                if (path[0] == ResolvedObject.listItemFlag)
                {
                    string indexStr = path.Remove(0, 1);
                    int index = int.Parse(indexStr);

                    if (obj is not IList list)
                    {
                        throw new RuntimeException($"实例不是 IList 类型，但路径节点为列表元素，所以无法获取对应路径上的实例");
                    }

                    obj = list[index];
                }
                else
                {
                    FieldInfo info = obj.GetType().GetField(path);
                    obj = info.GetValue(obj);
                }
            }

            return obj;
        }
    }
}