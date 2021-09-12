/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/9/11 13:52:59
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    using Database;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// DatabaseManager
    /// </summary>
    public class DatabaseManager
    {
        public static readonly char pathSeparator = '/';
        public static readonly char listItemFlag = '#';

        private Dictionary<object, string> src2id = new Dictionary<object, string>();
        private Dictionary<string, object> id2src = new Dictionary<string, object>();
        private Dictionary<string, string> alias2id = new Dictionary<string, string>();

        private Dictionary<string, ElementInfo> path2info = new Dictionary<string, ElementInfo>();

        public string AddData(object src, string aliasName = null)
        {
            if (src2id.ContainsKey(src)) { throw new RuntimeException($"数据实例已存在，请勿重复添加"); }
            string id = Guid.NewGuid().ToString("N");

            if (!string.IsNullOrEmpty(aliasName))
            {
                if (alias2id.ContainsKey(aliasName)) { throw new RuntimeException($"{aliasName} 已存在"); }

                alias2id[aliasName] = id;
            }

            src2id[src] = id;
            id2src[id] = src;

            UpdateData(src);

            return id;
        }

        public void UpdateData(object src)
        {
            if (!src2id.TryGetValue(src, out string id))
            {
                throw new RuntimeException($"该对象没有添加");
            }

            //移除旧的数据
            List<string> removePaths = ListPool<string>.Pop();
            removePaths.AddRange(path2info.Keys.Where(t => t.StartsWith(id)));
            foreach (var path in removePaths)
            {
                path2info.Remove(path);
            }
            ListPool<string>.Push(removePaths);

            //添加新的数据
            foreach (var info in Foreach(id, src))
            {
                path2info.Add(info.path, info);
            }
        }

        //public void SetValue(string fullPath, object value)
        //{
        //    string[] paths = fullPath.Split(pathSeparator, StringSplitOptions.RemoveEmptyEntries);

        //    string id = paths[0];

        //    if (!id2src.TryGetValue(id, out object data)) { throw new RuntimeException($"{fullPath} 指向的源不存在"); }

        //    object parent = data;

        //    for (int i = 1; i < paths.Length; i++)
        //    {
        //        string path = paths[i];

        //        if (path[0] == listItemFlag)
        //        {
        //            string indexStr = path.Remove(0, 1);
        //            int index = int.Parse(indexStr);

        //            if (parent is not IList list)
        //            {
        //                throw new RuntimeException($"实例不是 IList 类型，但路径节点为列表元素，所以无法获取对应路径上的实例");
        //            }

        //            list[index] = value;
        //        }
        //        else
        //        {
        //            FieldInfo info = parent.GetType().GetField(path);
        //            obj = info.GetValue(parent);
        //        }
        //    }
        //}

        #region Foreach

        /// <summary>
        /// 不支持展开的类型列表，为可以被Json序列化且被Unity可视化编辑的类型
        /// </summary>
        private static HashSet<Type> notSupportExpandTypes
            = new HashSet<Type>() {
                typeof(Quaternion),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(Vector2Int),
                typeof(Vector3Int),
                typeof(string),
                typeof(AnimationCurve),
        };

        private static bool SupportExpandType(Type type)
        {
            if (type.IsPrimitive || !(type.IsClass || type.IsValueType) || notSupportExpandTypes.Contains(type))
            {
                return false;
            }

            if (type.IsClass &&
                type.GetInterfaces().Any(x => x == typeof(ICollection) || x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>))
                )
            {//检查是否是容器，如果是容器，且不是IList，则报异常
                if (!typeof(IList).IsAssignableFrom(type) || (type.IsGenericType && type.GenericTypeArguments.Length != 1))
                {//限制容器类型为 IList，并且只能有一个模板参数
                    throw new RuntimeException($"不支持类型未继承列表（IList）的容器（ICollection/ICollection<>）类型 {type}");
                }
            }

            return true;
        }

        public static IEnumerable<ElementInfo> Foreach(string id, object parent)
        {
            List<string> pathStack = ListPool<string>.Pop();
            pathStack.Add(id);
            foreach (var item in _Foreach(parent, pathStack))
            {
                yield return item;
            }
            ListPool<string>.Push(pathStack);
        }

        private static IEnumerable<ElementInfo> _Foreach(object parent, List<string> pathStack)
        {
            Type type = parent.GetType();
            if (!SupportExpandType(type))
            {
                yield break;
            }

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                pathStack.Add(field.Name);//路径入栈 01

                object fieldValue = field.GetValue(parent);
                Type fieldType = fieldValue?.GetType() ?? field.FieldType;//优先获取实例的类型

                AvailableTypesAttribute at = field.GetCustomAttribute<AvailableTypesAttribute>(true);
                if (fieldValue == null)
                {//数据为null时，填充默认数据
                    fieldValue = AvailableTypesUtility.CreateInstance(fieldType, at);
                    fieldType = fieldValue.GetType();//使用实例的类型
                    field.SetValue(parent, fieldValue);
                }
                bool isList = typeof(IList).IsAssignableFrom(fieldType);
                bool canExpand = SupportExpandType(fieldType);

                yield return new ElementInfo()
                {
                    availableTypes = at,
                    declareType = field.FieldType,
                    path = BuildPath(pathStack),
                    value = fieldValue
                };

                if (canExpand)
                {//当前属性支持展开
                    if (isList)
                    {//当前成员变量为IList时
                        var list = fieldValue as IList;
                        AvailableItemTypesAttribute ait = field.GetCustomAttribute<AvailableItemTypesAttribute>(true);

                        Type itemType = fieldType.GenericTypeArguments[0];

                        for (int i = 0; i < list.Count; i++)
                        {
                            pathStack.Add($"{listItemFlag}{i}");//路径入栈 02

                            object item = list[i];
                            if (item == null)
                            {
                                item = AvailableTypesUtility.CreateInstance(itemType, ait);
                                list[i] = item;
                            }

                            yield return new ElementInfo()
                            {
                                availableTypes = ait,
                                declareType = itemType,
                                path = BuildPath(pathStack),
                                value = item
                            };

                            foreach (var subResolveField in _Foreach(item, pathStack))
                            {
                                yield return subResolveField;
                            }

                            pathStack.RemoveAt(pathStack.Count - 1);//路径出栈 02
                        }
                    }
                    else
                    {
                        foreach (var subResolveField in _Foreach(fieldValue, pathStack))
                        {
                            yield return subResolveField;
                        }
                    }
                }

                pathStack.RemoveAt(pathStack.Count - 1);//路径出栈 01
            }
        }

        private static string BuildPath(List<string> pathStack)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var path in pathStack)
            {
                stringBuilder.Append(path);
                stringBuilder.Append(pathSeparator);
            }
            return stringBuilder.ToString();
        }

        #endregion Foreach
    }

    namespace Database
    {
        public class ElementInfo
        {
            /// <summary>
            /// 可用的有效类型
            /// </summary>
            public IAvailableTypesAttribute availableTypes;

            /// <summary>
            /// 声名的类型
            /// </summary>
            public Type declareType;

            /// <summary>
            /// 值
            /// </summary>
            public object value;

            /// <summary>
            /// value 的路径
            /// </summary>
            public string path;

            public override string ToString()
            {
                return $"{path} = {value}";
            }
        }
    }
}