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
using XMLib.AM;

namespace AGT
{
    /// <summary>
    /// AnimationControl
    /// </summary>
    public class AnimationControl : MonoBehaviour, IViewControl
    {
        [SerializeField]
        private Animator _animator;

        public Animator animator => _animator;

        private void Awake()
        {
            _animator.enabled = false;
        }

        public void OnViewCreate(Entity entity)
        {
            InitializeActionMachine(entity);
        }

        public void OnViewDestory(Entity entity)
        {
        }

        public void OnViewUpdate(Entity entity, TransformData transformData, ViewData viewData, TimeData timeData)
        {
            UpdateActionMachine(entity);

            _animator.Update(timeData.renderDeltaTime);
        }

        private void InitializeActionMachine(Entity entity)
        {
            ActionMachineData actionMachineData = entity.GetComponent<ActionMachineData>();
            if (actionMachineData == null) { return; }

            string animName = Game.gw.actionMachine.GetAnimName(actionMachineData);

            animator.Play(animName, 0);
            animator.Update(0);
        }

        private void UpdateActionMachine(Entity entity)
        {
            ActionMachineData actionMachineData = entity.GetComponent<ActionMachineData>();
            if (actionMachineData == null || (actionMachineData.eventTypes & ActionMachineEvent.AnimChanged) == 0) { return; }

            ActionMachineEvent eventTypes = actionMachineData.eventTypes;

            StateConfig config = Game.gw.actionMachine.GetStateConfig(actionMachineData);

            float fixedTimeOffset = actionMachineData.animStartTime;
            float fadeTime = config.fadeTime;
            string animName = Game.gw.actionMachine.GetAnimName(actionMachineData);

            if ((eventTypes & ActionMachineEvent.HoldAnimDuration) != 0)
            {
                fixedTimeOffset = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }

            animator.CrossFadeInFixedTime(animName, fadeTime, 0, fixedTimeOffset);
            animator.Update(0);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                if (_animator == null)
                {
                    _animator = GetComponentInChildren<Animator>(true);
                }
            }
        }

#endif
    }
}