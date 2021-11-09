/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/9 22:08:39
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ResizeManipulator
    /// </summary>
    public class ResizeManipulator : Manipulator
    {
        public readonly ResizeElement resizeElement;
        public readonly ResizeMode mode;

        public ResizeManipulator(ResizeElement resizeElement, ResizeMode mode)
        {
            this.resizeElement = resizeElement;
            this.mode = mode;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            evt.StopPropagation();
            evt.target.ReleaseMouse();

            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);

            Debug.Log("OnMouseUp");
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            evt.StopPropagation();
            evt.target.CaptureMouse();

            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);

            Debug.Log("OnMouseDown");
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            evt.StopPropagation();
        }
    }
}