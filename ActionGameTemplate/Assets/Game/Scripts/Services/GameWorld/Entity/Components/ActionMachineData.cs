/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/25 13:24:30
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    [Flags]
    public enum ActionMachineEvent
    {
        None = 0b0000_0000,
        FrameChanged = 0b0000_0001,
        StateChanged = 0b0000_0010,
        AnimChanged = 0b0000_0100,
        HoldAnimDuration = 0b0000_1000,

        All = 0b1111_1111
    }

    public interface IActionHandler
    {
        void Enter(ActionNode node);

        void Exit(ActionNode node);

        void Update(ActionNode node, float deltaTime);
    }

    public class ActionNode
    {
        public ActionMachineManager manager;
        public int entityId;

        public int beginFrameIndex;
        public object config;
        public IActionHandler handler;
        public object data;

        public bool isUpdating { get; private set; } = false;
        public int updateCnt { get; private set; } = 0;

        public override string ToString()
        {
            return $"动作节点";
        }

        public void Reset()
        {
            manager = null;
            entityId = EntityManager.NoneID;
            beginFrameIndex = -1;
            config = null;
            handler = null;
            isUpdating = false;
            updateCnt = 0;
            data = null;
        }

        public void InvokeEnter()
        {
            updateCnt = 0;
            isUpdating = true;
            handler.Enter(this);
        }

        public void InvokeExit()
        {
            handler.Exit(this);
            isUpdating = false;
        }

        public void InvokeUpdate(float deltaTime)
        {
            if (!isUpdating)
            {
                return;
            }

            handler.Update(this, deltaTime);
            updateCnt++;
        }
    }

    /// <summary>
    /// ActionMachineData
    /// </summary>
    public class ActionMachineData : IComponentData
    {
        public string configName;
        public bool isInited;

        public int waitFrameCnt;
        public int frameIndex;
        public int globalFrameIndex;
        public int stateBeginFrameIndex;

        public int animIndex;
        public float animStartTime;

        public string stateName;

        public string nextStateName;
        public int nextAnimIndex;
        public float nextAnimStartTime;
        public int nextStatePriority;

        public List<ActionNode> globalActionNodes = new List<ActionNode>();
        public List<ActionNode> actionNodes = new List<ActionNode>();

        public ActionMachineEvent eventTypes;

        public void Reset()
        {
            configName = string.Empty;
            isInited = false;

            waitFrameCnt = 0;
            frameIndex = -1;
            globalFrameIndex = -1;
            stateBeginFrameIndex = -1;

            animIndex = 0;
            animStartTime = 0;

            stateName = string.Empty;

            nextStateName = null;
            nextAnimIndex = -1;
            nextAnimStartTime = 0;
            nextStatePriority = 0;

            globalActionNodes.Clear();
            actionNodes.Clear();

            eventTypes = ActionMachineEvent.None;
        }
    }
}