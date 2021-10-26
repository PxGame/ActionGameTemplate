/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/9 0:58:39
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// EntityView
    /// </summary>
    public class EntityView : MonoBehaviour
    {
        private List<IViewControl> controls = new List<IViewControl>();

        private int lastFrameIndex = -1;
        private Vector3 lastFramePosition;
        private Quaternion lastFrameRotation;

        private void Awake()
        {
            GetComponents<IViewControl>(controls);
        }

        private void OnEnable()
        {
            lastFrameIndex = -1;
            lastFramePosition = transform.position;
            lastFrameRotation = transform.rotation;
        }

        public virtual void OnViewUpdate(Entity entity, TransformData transformData, ViewData viewData, TimeData timeData)
        {
            if (lastFrameIndex != timeData.frameIndex)
            {
                lastFrameIndex = timeData.frameIndex;
                lastFramePosition = transform.position;
                lastFrameRotation = transform.rotation;
            }

            transform.position = Vector3.Lerp(lastFramePosition, transformData.position, timeData.renderTimeStep);
            transform.rotation = Quaternion.Lerp(lastFrameRotation, transformData.rotation, timeData.renderTimeStep);

            foreach (var control in controls)
            {
                control.OnViewUpdate(entity, transformData, viewData, timeData);
            }
        }
    }
}