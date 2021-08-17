/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/29 1:54:52
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.AM;

namespace AGT
{
    /// <summary>
    /// ActionMachineProcessor
    /// </summary>
    public class ActionMachineProcessor
    {
        public MachineConfig LoadConfig(string configName)
        {
            return null;
        }

        public void Update(Entity obj)
        {
            ActionMachineData data = obj.GetComponent<ActionMachineData>();
            if (data == null) { return; }

            UpdateInitialize(obj, data);
            UpdateState(obj, data);
            UpdateGlobalFrame(obj, data);
            UpdateState(obj, data);
            UpdateFrame(obj, data);
        }

        private void UpdateFrame(Entity obj, ActionMachineData data)
        {
        }

        private void UpdateGlobalFrame(Entity obj, ActionMachineData data)
        {
        }

        private void UpdateState(Entity obj, ActionMachineData data)
        {
            //清理
            data.nextStateName = null;
            data.nextStatePriority = 0;
            data.nextAnimIndex = -1;
            data.nextAnimStartTime = default;

            //状态改变
            data.eventTypes |= (ActionMachineEvent.StateChanged | ActionMachineEvent.AnimChanged);
        }

        private void UpdateInitialize(Entity obj, ActionMachineData data)
        {
            data.eventTypes = ActionMachineEvent.None;
        }
    }
}