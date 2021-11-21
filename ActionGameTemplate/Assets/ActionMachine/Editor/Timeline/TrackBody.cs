/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/12/4 1:29:35
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using System;

namespace XMLib.AM
{
    /// <summary>
    /// TrackBody
    /// </summary>
    public class TrackBody : VisualElement
    {
        private Label _name;

        public TrackBody()
        {
            var uxml = ResourceUtility.LoadUXML("Timeline/TrackBody");
            uxml.CloneTree(this);

            _name = this.Q<Label>("name");
        }

        public void Inspect(SerializedProperty property)
        {
            _name.BindProperty(property.FindPropertyRelative("name"));
        }

        public new class UxmlFactory : UxmlFactory<TrackBody, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}