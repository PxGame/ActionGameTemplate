/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/18 1:58:28
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// AnimationControl
    /// </summary>
    public class AnimationControl : MonoBehaviour, IViewControl
    {
        [SerializeField]
        private Animator animator;

        private void Awake()
        {
            animator.enabled = false;
        }

        public void OnViewUpdate(Entity entity, TransformData transformData, ViewData viewData, TimeData timeData)
        {
            animator.Update(timeData.renderDeltaTime);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                {
                    animator = GetComponentInChildren<Animator>(true);
                }
            }
        }

#endif
    }
}