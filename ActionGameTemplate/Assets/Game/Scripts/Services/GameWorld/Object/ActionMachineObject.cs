/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/6 1:35:55
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ActionMachineObject
    /// </summary>
    public class ActionMachineObject : ViewObject
    {
        public ActionMachineData am { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            am = AddComponent<ActionMachineData>();
        }

        public override void Destory()
        {
            am = null;
            base.Destory();
        }
    }

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