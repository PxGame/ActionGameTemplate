/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/29 20:15:50
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// AvailableTypesAttribute
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class AvailableTypesAttribute : Attribute
    {
        public Type[] types => _types;
        protected Type[] _types;

        public AvailableTypesAttribute(params Type[] types)
        {
            _types = types;
        }

        public Type GetDefaultType() => _types.Length > 0 ? _types[0] : null;

        public object CreateValueFromDefaultType()
        {
            Type defaultType = GetDefaultType();
            if (defaultType == null) { return null; }
            return TypeUtility.CreateInstance(defaultType);
        }
    }

    /// <summary>
    /// AvailableTypesFromParentAttribute
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    public class AvailableTypesFromParentAttribute : AvailableTypesAttribute
    {
        public AvailableTypesFromParentAttribute(Type parentType)
            : base(UnityEditor.TypeCache.GetTypesDerivedFrom(parentType).ToArray())
        {
        }
    }

    public static class AvailableTypesUtility
    {
        public static object CreateInstance(Type type, AvailableTypesAttribute availableTypes)
        {
            if (availableTypes != null && !typeof(IList).IsAssignableFrom(type))
            {//仅用于当前对象类型不是IList，否则直接实例化
                return availableTypes.CreateValueFromDefaultType();
            }

            if (type == typeof(string)) { return string.Empty; }

            if ((type.IsClass || type.IsValueType || type.IsPrimitive) && (type != typeof(object)))
            {
                return Activator.CreateInstance(type);
            }

            throw new RuntimeException($"{type} 类型为不可实例化类型，且没有标记 {nameof(AvailableTypesAttribute)} 特性");
        }
    }
}