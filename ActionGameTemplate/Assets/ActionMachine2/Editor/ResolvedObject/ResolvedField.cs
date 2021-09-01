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

        /// <summary>
        /// <para>index >= 0 时，表示当前对象为列表容器中的元素，
        /// parent 则为其实现 IList 接口的承载容器 ，
        /// fieldInfo 为 parent 的 FieldInfo，
        /// value 为当前对象的实例</para>
        /// <para>index < 0 时，表示当前对象为成员变量，
        /// parent 则为其所在的对象的实例 ，
        /// fieldInfo 为 value 在 parent 中的 FieldInfo，
        /// value 为当前对象的实例</para>
        /// </summary>
        public struct Infos
        {
            public object parent;
            public FieldInfo fieldInfo;

            public int index;
            public object value;

            public IAvailableTypesAttribute GetAvailiable()
            {
                return index >= 0 ? fieldInfo.GetCustomAttribute<AvailableItemTypesAttribute>(true) : fieldInfo.GetCustomAttribute<AvailableTypesAttribute>(true);
            }

            public void SetValue(object target)
            {
                IAvailableTypesAttribute iat = GetAvailiable();
                if (iat != null && !iat.IsInvaild(target))
                {
                    throw new RuntimeException($"{target} 不是 IAvailableTypesAttribute 所支持的类型");
                }

                if (index >= 0)
                {//index >=0 则父对象为IList
                    (parent as IList)[index] = target;
                }
                else
                {
                    fieldInfo.SetValue(parent, target);
                }
            }
        }

        public Infos GetInfos()
        {
            Infos results = new Infos { index = -1 };

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

                    results.parent = list;
                    results.index = index;
                    results.value = obj;
                }
                else
                {
                    FieldInfo info = parent.GetType().GetField(path);
                    obj = info.GetValue(parent);

                    results.parent = parent;
                    results.fieldInfo = info;
                    results.index = -1;
                    results.value = obj;
                }
            }

            if (results.parent == null || results.fieldInfo == null)
            {
                throw new RuntimeException($"GetInfos 数据不完整");
            }

            return results;
        }

        public object SetValue(object value)
        {
            var infos = GetInfos();
            infos.SetValue(value);
            return infos.value;
        }

        public object GetValue()
        {
            var infos = GetInfos();
            return infos.value;
        }
    }
}