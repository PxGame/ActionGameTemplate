/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/21 0:47:57
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
    /// MainManipulator
    /// </summary>
    public class MainManipulator : Manipulator
    {
        private MainElement mainElement => (MainElement)target;

        public MainManipulator()
        {
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PackageChanged>(OnActionMachinePackageChanged);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PackageChanged>(OnActionMachinePackageChanged);
        }

        private void OnActionMachinePackageChanged(PackageChanged evt)
        {
            Debug.Log("ActionMachinePackage 变化");

            bool hasData = evt.newPackage != null;

            mainElement.panelContainer.SetEnabled(hasData);
            mainElement.panelContainer.visible = hasData;

            if (hasData)
            {
                mainElement.Bind(ActionMachineManager.inst.data);
            }

            mainElement.OnPackageChanged(evt.newPackage);

            var panels = mainElement.Query<BaseElement>();
            panels.ForEach(t =>
            {
                using (var evt2 = DataChanged.GetPooled(t))
                {
                    mainElement.SendEvent(evt2);
                }
            });

            if (!hasData)
            {
                mainElement.Unbind();
            }
        }
    }
}