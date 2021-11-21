/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 0:59:44
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Diagnostics;
using UnityEditor.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ObjectTypeAttribute
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ObjectTypeAttribute : PropertyAttribute
    {
        public Type baseType { get; private set; }

        public Type[] types { get; private set; }

        public ObjectTypeAttribute(params Type[] types)
        {
            this.types = types;
        }
    }
}