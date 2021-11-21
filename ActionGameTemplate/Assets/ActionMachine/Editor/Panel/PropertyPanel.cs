/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 21:42:02
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// PropertyPanel
    /// </summary>
    public class PropertyPanel : BindablePanelElement
    {
        private VisualElement _propertyContainer;

        public override string uxmlPath => "Panel/PropertyPanel";

        public PropertyPanel() : base()
        {
            _propertyContainer = this.Q("property-container");
        }

        public new class UxmlFactory : PanelElement.UxmlFactory<PropertyPanel, UxmlTraits>
        {
        }

        public new class UxmlTraits : PanelElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }

        protected override void OnPropertyChanged()
        {
            base.OnPropertyChanged();

            string propertyName = "property";
            _propertyContainer.Clear();

            if (property == null) { return; }
            PropertyField field = new PropertyField();
            field.name = propertyName;
            field.BindProperty(property);
            _propertyContainer.Add(field);
        }
    }
}