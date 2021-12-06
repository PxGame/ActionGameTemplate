/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/11 23:35:09
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// MenuElement
    /// </summary>
    public class MenuElement : BaseElement
    {
        private ObjectField actionMachineAsset;

        public MenuElement()
        {
            var uxml = ResourceUtility.LoadUXML("MenuElement");
            uxml.CloneTree(this);
        }

        protected override void OnInit(InitEvent evt)
        {
            base.OnInit(evt);

            actionMachineAsset = this.Q<ObjectField>("actionmachine-asset");
            actionMachineAsset.RegisterValueChangedCallback(t =>
            {
                ActionMachineManager.inst.source = t.newValue as ActionMachinePackage;
            });
        }

        public new class UxmlFactory : UxmlFactory<MenuElement, UxmlTraits>
        {
            public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
            {
                VisualElement ve = base.Create(bag, cc);

                var tbMenu = ve.Q<ToolbarMenu>("menu-file");

                tbMenu.menu.AppendAction("Test01", t => Debug.Log(t.name));
                tbMenu.menu.AppendAction("Test02", t => Debug.Log(t.name));
                tbMenu.menu.AppendAction("Test03", t => Debug.Log(t.name));
                tbMenu.menu.AppendAction("Test Event", t =>
                {
                    Debug.Log(t.name);
                });

                return ve;
            }
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