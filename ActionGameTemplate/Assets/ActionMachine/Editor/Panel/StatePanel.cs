/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 21:40:44
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using System.Linq;
using System;

namespace XMLib.AM
{
    /// <summary>
    /// StatePanel
    /// </summary>
    public class StatePanel : PanelElement
    {
        public override string uxmlPath => "Panel/StatePanel";

        private ListView statesList;

        public StatePanel() : base()
        {
            statesList = this.Q<ListView>("list-states");
            statesList.itemHeight = 18;
            statesList.makeItem = () =>
            {
                var text = new Label();
                text.style.unityTextAlign = TextAnchor.MiddleLeft;
                return text;
            };
            statesList.unbindItem = (ve, index) =>
            {
                ve.RemoveFromHierarchy();
            };
            statesList.bindItem = (ve, index) =>
            {
                var state = statesList.itemsSource[index] as SerializedProperty;
                var label = (ve as Label);
                //label.text = state.FindPropertyRelative("setting.stateName").stringValue;
                label.BindProperty(state.FindPropertyRelative("setting.stateName"));

                ve.AddManipulator(new ContextualMenuManipulator((evt) =>
                {
                    evt.menu.AppendAction("删除", (x) =>
                    {
                        ActionMachineManager.data.RecordSource();
                        ActionMachineManager.data.RemoveStateAt(index);
                    });
                    evt.menu.AppendAction("插入状态", (x) =>
                    {
                        ActionMachineManager.data.InsertNewState(index);
                    });
                }));
            };
            statesList.onSelectionChange += (t) =>
            {
                if (t.Count() == 0) { return; }
                var state = t.First() as SerializedProperty;

                ActionMachineManager.data.InspectState(state);
            };
            statesList.AddManipulator(new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction("添加状态", (x) =>
                {
                    ActionMachineManager.data.AddNewState();
                });
            }));

            ActionMachineManager.data.onPackageChanged += OnPackageChanged;
        }

        private void OnPackageChanged()
        {
            if (ActionMachineManager.data.serializedObject != null)
            {
                var states = ActionMachineManager.data.serializedObject.FindProperty("data.states");
                statesList.BindProperty(states);
            }
            else
            {
                statesList.ClearSelection();
                statesList.Unbind();
            }
        }

        #region Design

        public new class UxmlFactory : PanelElement.UxmlFactory<StatePanel, UxmlTraits>
        {
        }

        public new class UxmlTraits : PanelElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }

        #endregion Design
    }
}