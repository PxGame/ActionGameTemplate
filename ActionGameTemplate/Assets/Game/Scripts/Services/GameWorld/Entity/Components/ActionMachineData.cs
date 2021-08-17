/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/25 13:24:30
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    public enum ActionMachineEvent
    {
        None = 0x0,
        StateChanged = 0x1,
        AnimChanged = 0x2,
    }

    /// <summary>
    /// ActionMachineData
    /// </summary>
    public class ActionMachineData : IComponentData
    {
        public string configName;
        public string stateName;

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