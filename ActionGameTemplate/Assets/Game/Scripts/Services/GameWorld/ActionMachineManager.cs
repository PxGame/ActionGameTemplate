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
    public enum ActionMachineEvent
    {
        None = 0x0,
        StateChanged = 0x1,
        AnimChanged = 0x2,
    }

    /// <summary>
    /// ActionMachineManager
    /// </summary>
    public class ActionMachineManager
    {
        public GameWorld gw { get; set; }

        public MachineConfig LoadConfig(string configName)
        {
            return null;
        }

        public void Update(ActionMachineObject obj, float deltaTime)
        {
            UpdateInitialize(obj);
            UpdateState(obj);
            UpdateGlobalFrame(obj, deltaTime);
            UpdateState(obj);
            UpdateFrame(obj, deltaTime);
        }

        private void UpdateFrame(ActionMachineObject obj, float deltaTime)
        {
        }

        private void UpdateGlobalFrame(ActionMachineObject obj, float deltaTime)
        {
        }

        private void UpdateState(ActionMachineObject obj)
        {
            //清理
            obj.actionMachine.nextStateName = null;
            obj.actionMachine.nextStatePriority = 0;
            obj.actionMachine.nextAnimIndex = -1;
            obj.actionMachine.nextAnimStartTime = default;

            //状态改变
            obj.actionMachine.eventTypes |= (ActionMachineEvent.StateChanged | ActionMachineEvent.AnimChanged);
        }

        private void UpdateInitialize(ActionMachineObject controller)
        {
            controller.actionMachine.eventTypes = ActionMachineEvent.None;
        }
    }

    public class ActionMachineData : IComponentData
    {
        public string configName;
        public string stateName;

        public int globalFrameIndex;
        public int frameIndex;

        public int waitFrameCnt;

        public int stateBeginFrameIndex;
        public int animIndex;
        public float animStartTime;

        public int nextStatePriority;
        public string nextStateName;
        public int nextAnimIndex;
        public float nextAnimStartTime;

        public ActionMachineEvent eventTypes;

        public List<object> globalActionNodes;
        public List<object> actionNodes;

        public void Reset()
        {
        }
    }
}