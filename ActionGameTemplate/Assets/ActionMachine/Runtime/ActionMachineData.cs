/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/10 22:52:18
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.AM
{
    /// <summary>
    /// ActionMachineData
    /// </summary>
    [System.Serializable]
    public class ActionMachineData
    {
        public string defaultStateName;

        [SerializeReference]
        public List<StateData> states = new List<StateData>();

        public static readonly ActionMachineData Default = new ActionMachineData()
        {
            defaultStateName = "Default State Name"
        };
    }

    [System.Serializable]
    public class StateData
    {
        public StateSettingData setting;
        public List<TrackData> tracks;
    }

    [System.Serializable]
    public class StateSettingData
    {
        public string stateName;
        public string animName;
        public int stateLoopCnt;
        public string nextStateName;
    }

    [System.Serializable]
    public class TrackData
    {
        public string name;
    }

    [System.Serializable]
    public class ActionData
    {
        public Guid startNodeId;
        public List<NodeData> nodes;
        public List<LinkData> links;
    }

    [System.Serializable]
    public class NodeData
    {
        public Guid id;
    }

    [System.Serializable]
    public class LinkData
    {
        public Guid id;

        public Guid leftNodeId;
        public Guid rightNodeId;
    }
}