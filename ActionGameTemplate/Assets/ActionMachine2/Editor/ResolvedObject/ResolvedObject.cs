/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/29 18:33:00
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// ResolvedObject
    /// </summary>
    public class ResolvedObject
    {
        public static readonly char pathSeparator = '/';
        public static readonly char listItemFlag = '#';

        private object _target;
        public object target { get => _target; }

        private List<ResolvedField> _fields = new List<ResolvedField>();
        public List<ResolvedField> fields => _fields;

        public ResolvedObject(object target)
        {
            _target = target;
            Update();
        }

        public void Update()
        {
            UpdateField();
        }

        private void UpdateField()
        {
            _fields.Clear();
            foreach (var resolvedField in Foreach(_target, new List<string>()))
            {
                _fields.Add(resolvedField);
            }
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

        private bool SupportExpandType(Type type, List<string> pathStack)
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
                    throw new RuntimeException($"不支持类型未继承列表（IList）的容器（ICollection/ICollection<>）类型 {type}，[{target.GetType()}]/{BuildPath(pathStack)}");
                }
            }

            return true;
        }

        private IEnumerable<ResolvedField> Foreach(object parent, List<string> pathStack)
        {
            Type type = parent.GetType();
            if (!SupportExpandType(type, pathStack))
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

                ResolvedField resolvedField = new ResolvedField(this, BuildPath(pathStack), pathStack.Count);
                yield return resolvedField;

                if (SupportExpandType(fieldType, pathStack))
                {//当前属性支持展开
                    if (typeof(IList).IsAssignableFrom(fieldType))
                    {
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

                            ResolvedField resolvedField2 = new ResolvedField(this, BuildPath(pathStack), pathStack.Count);
                            yield return resolvedField2;
                            foreach (var subResolveField in Foreach(item, pathStack))
                            {
                                yield return subResolveField;
                            }

                            pathStack.RemoveAt(pathStack.Count - 1);//路径出栈 02
                        }
                    }
                    else
                    {
                        foreach (var subResolveField in Foreach(fieldValue, pathStack))
                        {
                            yield return subResolveField;
                        }
                    }
                }

                pathStack.RemoveAt(pathStack.Count - 1);//路径出栈 01
            }
        }

        private string BuildPath(List<string> pathStack)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var path in pathStack)
            {
                stringBuilder.Append(path);
                stringBuilder.Append(pathSeparator);
            }
            return stringBuilder.ToString();
        }

        public ResolvedFieldInfo GetInfos(ResolvedField field)
        {
            if (field.resolvedObject != this)
            {
                throw new RuntimeException($"传入的Field不属于当前的Object");
            }

            ResolvedFieldInfo results = new ResolvedFieldInfo { index = -1 };

            string[] paths = field.GetPaths();

            object obj = target;

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

        public IEnumerable<ResolvedField> ForeachChild(ResolvedField field)
        {
            if (field.resolvedObject != this)
            {
                throw new RuntimeException($"传入的Field不属于当前的Object");
            }
            int childDepth = field.depth + 1;
            return fields.Where(t => t.depth == childDepth && t.fieldPath.StartsWith(field.fieldPath));
        }
    }
}