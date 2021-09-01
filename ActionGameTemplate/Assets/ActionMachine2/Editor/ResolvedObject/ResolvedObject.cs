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
                    throw new RuntimeException($"不支持类型未继承列表（IList）的容器（ICollection/ICollection<>）类型 {type}，[{target.GetType()}]/{GetPath(pathStack)}");
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

                ResolvedField resolvedField = new ResolvedField(this, GetPath(pathStack));
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

                            ResolvedField resolvedField2 = new ResolvedField(this, GetPath(pathStack));
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

        private string GetPath(List<string> pathStack)
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
}