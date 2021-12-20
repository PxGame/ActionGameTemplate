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
    public class PropertyPanel : PanelElement
    {
        private VisualElement _propertyContainer;

        public override string uxmlPath => "Panel/PropertyPanel";

        private SerializedProperty _property;

        private PropertyField _field;

        public PropertyPanel() : base()
        {
            _propertyContainer = this.Q("property-container");
            ActionMachineManager.data.onPropertyChanged += OnPropertyChanged;

            _field = new PropertyField();
            _field.RegisterValueChangeCallback(OnPropertyChanged);
            _field.name = "property";
            _propertyContainer.Add(_field);
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

        protected void OnPropertyChanged()
        {
            if (ActionMachineManager.data.currentProperty != null)
            {
                _field.BindProperty(ActionMachineManager.data.currentProperty);
            }
            else
            {
                _field.Unbind();
                _field.Clear();
            }
        }

        private void OnPropertyChanged(SerializedPropertyChangeEvent evt)
        {
            ActionMachineManager.data?.onPropertyValueChanged(evt);
        }
    }
}