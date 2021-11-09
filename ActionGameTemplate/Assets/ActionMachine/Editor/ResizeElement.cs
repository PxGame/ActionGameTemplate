/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/9 21:27:57
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace XMLib.AM
{
    [Flags]
    public enum ResizeMode
    {
        None = 0b0000,
        Left = 0b0001,
        Right = 0b0010,
        Top = 0b0100,
        Bottom = 0b1000
    }

    /// <summary>
    /// ResizeElement
    /// </summary>
    public class ResizeElement : VisualElement
    {
        public override VisualElement contentContainer => _body;

        protected VisualElement _body;

        public bool resizeLeft
        {
            get => (resizeMode & ResizeMode.Left) != 0;
            set
            {
                resizeMode = value ? (resizeMode | ResizeMode.Left) : (resizeMode & ~ResizeMode.Left);
            }
        }

        public bool resizeRight
        {
            get => (resizeMode & ResizeMode.Right) != 0;
            set
            {
                resizeMode = value ? (resizeMode | ResizeMode.Right) : (resizeMode & ~ResizeMode.Right);
            }
        }

        public bool resizeTop
        {
            get => (resizeMode & ResizeMode.Top) != 0;
            set
            {
                resizeMode = value ? (resizeMode | ResizeMode.Top) : (resizeMode & ~ResizeMode.Top);
            }
        }

        public bool resizeBottom
        {
            get => (resizeMode & ResizeMode.Bottom) != 0;
            set
            {
                resizeMode = value ? (resizeMode | ResizeMode.Bottom) : (resizeMode & ~ResizeMode.Bottom);
            }
        }

        protected ResizeMode _resizeMode;

        public ResizeMode resizeMode
        {
            get => _resizeMode;
            set
            {
                _resizeMode = value;
                OnResizeModeChanged();
            }
        }

        private void OnResizeModeChanged()
        {
            foreach (var handle in _handles)
            {
                bool enable = (handle.mode & resizeMode) != 0;
                handle.ve.SetEnabled(enable);
                handle.ve.visible = enable;
            }
        }

        public ResizeElement()
        {
            var uxml = ResourceUtility.LoadUXML("ResizeElement");
            uxml.CloneTree(this);

            _body = this.Q("body");

            InitResizeHandle();
        }

        private List<(ResizeMode mode, VisualElement ve, ResizeManipulator man)> _handles = new List<(ResizeMode, VisualElement, ResizeManipulator)>();

        private void InitResizeHandle()
        {
            foreach (var horizontal in new[] { ResizeMode.Left, ResizeMode.Right })
            {
                foreach (var vertical in new[] { ResizeMode.Top, ResizeMode.Bottom })
                {
                    var ve = this.Q($"resize-{horizontal}-{vertical}".ToLower());
                    if (ve == null) { continue; }
                    InitResizeHandle(ve, horizontal | vertical);
                }
            }

            foreach (var mode in new[] { ResizeMode.Left, ResizeMode.Right, ResizeMode.Top, ResizeMode.Bottom })
            {
                var ve = this.Q($"resize-{mode}".ToLower());
                if (ve == null) { continue; }
                InitResizeHandle(ve, mode);
            }
        }

        private void InitResizeHandle(VisualElement ve, ResizeMode mode)
        {
            var man = new ResizeManipulator(this, mode);
            ve.AddManipulator(man);
            _handles.Add((mode, ve, man));
        }

        public new class UxmlFactory : UxmlFactory<ResizeElement, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public UxmlBoolAttributeDescription resizeLeft = new UxmlBoolAttributeDescription() { name = "resize-left" };
            public UxmlBoolAttributeDescription resizeRight = new UxmlBoolAttributeDescription() { name = "resize-right" };
            public UxmlBoolAttributeDescription resizeTop = new UxmlBoolAttributeDescription() { name = "resize-top" };
            public UxmlBoolAttributeDescription resizeBottom = new UxmlBoolAttributeDescription() { name = "resize-bottom" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var resizeElement = ve as ResizeElement;

                var mode = ResizeMode.None;
                mode |= resizeLeft.GetValueFromBag(bag, cc) ? ResizeMode.Left : mode;
                mode |= resizeRight.GetValueFromBag(bag, cc) ? ResizeMode.Right : mode;
                mode |= resizeTop.GetValueFromBag(bag, cc) ? ResizeMode.Top : mode;
                mode |= resizeBottom.GetValueFromBag(bag, cc) ? ResizeMode.Bottom : mode;

                resizeElement.resizeMode = mode;
            }
        }
    }
}