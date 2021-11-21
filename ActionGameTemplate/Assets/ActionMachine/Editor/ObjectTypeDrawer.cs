/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 1:48:03
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ObjectTypeDrawer
    /// </summary>

    [CustomPropertyDrawer(typeof(ObjectTypeAttribute))]
    public class ObjectTypeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            container.Add(new Label() { text = "Test" });

            return container;
        }
    }
}