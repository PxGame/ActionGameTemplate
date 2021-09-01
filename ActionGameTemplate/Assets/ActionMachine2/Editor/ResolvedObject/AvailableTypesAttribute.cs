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
    public interface IAvailableTypesAttribute
    {
        Type[] types { get; }

        object CreateValueFromDefaultType();

        bool IsInvaild(object obj);

        bool IsInvaild(Type type);
    }

    /// <summary>
    /// AvailableTypesAttribute
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class AvailableTypesAttribute : Attribute, IAvailableTypesAttribute
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

        public bool IsInvaild(object obj) => IsInvaild(obj?.GetType());

        public bool IsInvaild(Type type)
        {
            return _types.Contains(type);
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

    /// <summary>
    /// AvailableItemTypesAttribute
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class AvailableItemTypesAttribute : Attribute, IAvailableTypesAttribute
    {
        public Type[] types => _types;
        protected Type[] _types;

        public AvailableItemTypesAttribute(params Type[] types)
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

        public bool IsInvaild(object obj) => IsInvaild(obj?.GetType());

        public bool IsInvaild(Type type)
        {
            return _types.Contains(type);
        }
    }

    /// <summary>
    /// AvailableTypesFromParentAttribute
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    public class AvailableItemTypesFromParentAttribute : AvailableTypesAttribute
    {
        public AvailableItemTypesFromParentAttribute(Type parentType)
            : base(UnityEditor.TypeCache.GetTypesDerivedFrom(parentType).ToArray())
        {
        }
    }

    public static class AvailableTypesUtility
    {
        public static object CreateInstance(Type type, IAvailableTypesAttribute availableTypes)
        {
            if (availableTypes != null)
            {
                return availableTypes.CreateValueFromDefaultType();
            }

            if (type == typeof(string)) { return string.Empty; }

            if ((type.IsClass || type.IsValueType || type.IsPrimitive) && (type != typeof(object)))
            {
                return Activator.CreateInstance(type);
            }

            throw new RuntimeException($"{type} 类型为不可实例化类型，且没有标记 {nameof(IAvailableTypesAttribute)} 特性");
        }
    }
}