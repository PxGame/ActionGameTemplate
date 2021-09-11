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

        public string AddData(object data, string aliasName = null)
        {
            if (src2id.ContainsKey(src2id)) { throw new RuntimeException($"数据实例已存在，请勿重复添加"); }
            string id = Guid.NewGuid().ToString("N");

            if (!string.IsNullOrEmpty(aliasName))
            {
                if (alias2id.ContainsKey(aliasName)) { throw new RuntimeException($"{aliasName} 已存在"); }

                alias2id[aliasName] = id;
            }

            src2id[data] = id;
            id2src[id] = data;

            UpdateObjectStructure(data);

            return id;
        }

        private void UpdateObjectStructure(object data)
        {
        }

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

        public static IEnumerable<ForeachInfo> Foreach(object parent)
        {
            List<string> pathStack = ListPool<string>.Pop();
            foreach (var item in _Foreach(parent, pathStack))
            {
                yield return item;
            }
            ListPool<string>.Push(pathStack);
        }

        private static IEnumerable<ForeachInfo> _Foreach(object parent, List<string> pathStack)
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

                yield return new ForeachInfo()
                {
                    parent = parent,
                    fieldInfo = field,
                    value = fieldValue,
                    type = fieldType,
                    path = BuildPath(pathStack),
                    depth = pathStack.Count,
                    list = null,
                    index = -1
                };

                if (SupportExpandType(fieldType))
                {//当前属性支持展开
                    if (typeof(IList).IsAssignableFrom(fieldType))
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

                            yield return new ForeachInfo()
                            {
                                parent = parent,
                                fieldInfo = field,
                                value = item,
                                type = itemType,
                                path = BuildPath(pathStack),
                                depth = pathStack.Count,
                                list = list,
                                index = i
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
    }

    namespace Database
    {
        public struct ForeachInfo
        {
            /// <summary>
            ///  index >= 0 时，list 为 parent 的成员变量，否则 value 为 parent 的成员变量
            /// </summary>
            internal object parent;

            /// <summary>
            /// parent 成员变量信息
            /// </summary>
            internal FieldInfo fieldInfo;

            /// <summary>
            /// value 的类型
            /// </summary>
            internal Type type;

            /// <summary>
            /// index >= 0 时，为 list[index] 的值, 否则为 fieldInfo.GetValue(parent)的值
            /// </summary>
            internal object value;

            /// <summary>
            /// value 的路径
            /// </summary>
            internal string path;

            /// <summary>
            /// value 的深度
            /// </summary>
            internal int depth;

            /// <summary>
            /// index >=0 时，list 有效，且为 value 的容器
            /// </summary>
            internal IList list;

            /// <summary>
            /// value 在 list 中的序号
            /// </summary>
            internal int index;

            public override string ToString()
            {
                return $"[{depth}]{path} = {value}";
            }
        }
    }
}