/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/21 0:37:22
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// BaseElement
    /// </summary>
    public abstract class BaseElement : VisualElement
    {
        public MainElement main { get; private set; }

        public BaseElement()
        {
            this.RegisterCallback<InitEvent>(OnInit);
            this.RegisterCallback<DataChanged>(OnDataChanged);
        }

        protected virtual void OnDataChanged(DataChanged evt)
        {
            Debug.Log($"{GetType().Name} => OnDataChanged");
        }

        protected virtual void OnInit(InitEvent evt)
        {
            main = evt.mainElement;
        }
    }
}