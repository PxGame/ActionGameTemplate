/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/29 1:54:52
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XMLib;
using XMLib.AM;

namespace AGT
{
    /// <summary>
    /// ActionMachineManager
    /// </summary>
    public class ActionMachineManager : IManager
    {
        public void Initialize()
        {
        }

        public void Destory()
        {
        }

        #region data

        private Dictionary<string, Tuple<MachineConfig, Dictionary<string, StateConfig>>> name2config = new Dictionary<string, Tuple<MachineConfig, Dictionary<string, StateConfig>>>();
        private static Stack<ActionNode> actionNodePool = new Stack<ActionNode>();
        private static Dictionary<Type, IActionHandler> actionHandlerDict = new Dictionary<Type, IActionHandler>();

        private MachineConfig GetMachineConfig(string configName)
        {
            if (name2config.TryGetValue(configName, out var data)) { return data.Item1; }

            MachineConfig config = Game.resource.LoadMachineConfig(configName);

            Dictionary<string, StateConfig> name2stateConifg = new Dictionary<string, StateConfig>();

            foreach (var state in config.states)
            {
                name2stateConifg.Add(state.stateName, state);
            }

            name2config.Add(configName, Tuple.Create(config, name2stateConifg));

            return config;
        }

        public StateConfig GetStateConfig(ActionMachineData data)
        {
            return name2config[data.configName].Item2[data.stateName];
        }

        private ActionNode CreateActionNode()
        {
            if (actionNodePool.Count > 0)
            {
                return actionNodePool.Pop();
            }

            ActionNode node = new ActionNode();
            return node;
        }

        private void RecycleActionNode(ActionNode node)
        {
            node.Reset();
            actionNodePool.Push(node);
        }

        private IActionHandler GetActionHandler(Type type)
        {
            IActionHandler handler = null;

            if (actionHandlerDict.TryGetValue(type, out handler))
            {
                return handler;
            }

            ActionConfigAttribute config = type.GetCustomAttribute<ActionConfigAttribute>(true);

            Type handlerType = config.handlerType;

            handler = Activator.CreateInstance(handlerType) as IActionHandler;
            if (handler == null)
            {
                throw new RuntimeException($"{handlerType} 类型未继承 {nameof(IActionHandler)} 接口");
            }

            actionHandlerDict.Add(type, handler);

            return handler;
        }

        #endregion data

        #region Update

        private void InitActionMachine(Entity entity, ActionMachineData data)
        {
            //初始化全局动作
            MachineConfig mConfig = GetMachineConfig(data.configName);
            InitializeActions(entity, data.globalActionNodes, mConfig.globalActions, 0);

            //初始化第一个状态
            data.stateName = mConfig.firstStateName;
            StateConfig sConfig = GetStateConfig(data);
            data.stateBeginFrameIndex = data.frameIndex + 1;
            data.animIndex = sConfig.dafualtAnimIndex;
            data.animStartTime = default;
            InitializeActions(entity, data.actionNodes, sConfig.actions, 0); //初始化新的动作
        }

        public void Update(Entity entity, ActionMachineData actionMachineData, float deltaTime)
        {
            UpdateInitialize(entity, actionMachineData);
            UpdateState(entity, actionMachineData);
            UpdateGlobalFrame(entity, actionMachineData, deltaTime);
            UpdateState(entity, actionMachineData);
            UpdateFrame(entity, actionMachineData, deltaTime);
        }

        private void UpdateInitialize(Entity entity, ActionMachineData data)
        {
            if (!data.isInited)
            {
                data.isInited = true;
                InitActionMachine(entity, data);
            }

            data.eventTypes = ActionMachineEvent.None;
        }

        private void UpdateGlobalFrame(Entity entity, ActionMachineData data, float deltaTime)
        {
            //全局帧，不受顿帧影响
            data.globalFrameIndex++;

            //更新全局动作-不被顿帧影响
            UpdateActions(data, true, deltaTime);
        }

        private void UpdateFrame(Entity entity, ActionMachineData data, float deltaTime)
        {
            StateConfig sConfig = GetStateConfig(data);
            if (null == sConfig)
            {
                throw new RuntimeException("没有状态配置");
            }

            if (data.waitFrameCnt > 0)
            { //顿帧
                data.waitFrameCnt--;
                return;
            }

            //帧增加
            data.frameIndex++;

            int index = GetStateFrameIndex(data);

            int maxFrameCnt = sConfig.frames.Count;
            if (index >= maxFrameCnt)
            {
                throw new RuntimeException($"当前状态 {sConfig.stateName} 帧序号 {index} 超过上限 {sConfig.frames.Count}");
            }

            //更新动作
            UpdateActions(data, false, deltaTime);

            //更新事件
            data.eventTypes |= ActionMachineEvent.FrameChanged;

            if (!sConfig.enableLoop && index + 1 == maxFrameCnt && string.IsNullOrEmpty(data.nextStateName))
            { //最后一帧
                ChangeState(data, sConfig.nextStateName, animIndex: sConfig.nextAnimIndex);
            }
        }

        private void UpdateState(Entity entity, ActionMachineData data)
        {
            if (string.IsNullOrEmpty(data.nextStateName))
            {
                return;
            }

            data.stateName = data.nextStateName;//设置新的状态
            data.stateBeginFrameIndex = data.frameIndex + 1;//状态起始帧

            StateConfig sConfig = GetStateConfig(data);

            data.animIndex = data.nextAnimIndex < 0 ? sConfig.dafualtAnimIndex : data.nextAnimIndex;
            data.animStartTime = data.nextAnimStartTime;

            //释放已有动作
            DisposeActions(data.actionNodes);
            //初始化新的动作
            InitializeActions(entity, data.actionNodes, sConfig.actions, data.frameIndex);

            data.nextStateName = null;
            data.nextStatePriority = 0;
            data.nextAnimIndex = -1;
            data.nextAnimStartTime = default;

            //状态改变
            data.eventTypes |= (ActionMachineEvent.StateChanged | ActionMachineEvent.AnimChanged);
        }

        #endregion Update

        #region operations

        #region action operations

        private void InitializeActions(Entity entity, List<ActionNode> target, List<object> actions, int index)
        {
            for (int i = 0, count = actions.Count; i < count; i++)
            {
                object action = actions[i];

                ActionNode actionNode = CreateActionNode();
                actionNode.manager = this;
                actionNode.entityId = entity.id;
                actionNode.config = action;
                actionNode.beginFrameIndex = index;
                actionNode.handler = GetActionHandler(action.GetType());
                target.Add(actionNode);
            }
        }

        private void DisposeActions(List<ActionNode> target)
        {
            for (int i = 0, count = target.Count; i < count; i++)
            {
                ActionNode actionNode = target[i];

                if (actionNode.isUpdating)
                {
                    actionNode.InvokeExit();
                }
                RecycleActionNode(actionNode);
            }
            target.Clear();
        }

        private void UpdateActions(ActionMachineData data, bool globalOrNormal, float deltaTime)
        {
            List<ActionNode> target = globalOrNormal ? data.globalActionNodes : data.actionNodes;
            int index = globalOrNormal ? data.globalFrameIndex : data.frameIndex;

            for (int i = 0, count = target.Count; i < count; i++)
            {
                ActionNode action = target[i];

                if (action.config is IHoldFrames hold && hold.EnableBeginEnd())
                {
                    if (!hold.EnableLoop() && GetStateLoopCnt(data) > 0)
                    {//只在第一次循环执行
                        return;
                    }

                    if (hold.GetBeginFrame() == index)
                    {
                        action.InvokeEnter();
                    }

                    action.InvokeUpdate(deltaTime);

                    if (hold.GetEndFrame() == index)
                    {
                        action.InvokeExit();
                    }
                }
                else
                {
                    if (action.updateCnt == 0)
                    {
                        action.InvokeEnter();
                    }

                    action.InvokeUpdate(deltaTime);
                }
            }
        }

        #endregion action operations

        #region state

        public string GetAnimName(ActionMachineData data) => GetStateConfig(data).GetAnimName(data.animIndex);

        public void ReplayAnim(ActionMachineData data)
        {
            data.eventTypes |= ActionMachineEvent.AnimChanged;
            data.eventTypes &= ~ActionMachineEvent.HoldAnimDuration;
        }

        public int GetStateFrameIndex(ActionMachineData data)
        {
            StateConfig config = GetStateConfig(data);
            int interval = data.frameIndex - data.stateBeginFrameIndex;
            int frameMax = config.frames.Count;
            if (config.enableLoop && interval + 1 > frameMax)
            {
                interval %= frameMax;
            }
            return interval;
        }

        public FrameConfig GetStateFrame(ActionMachineData data)
        {
            StateConfig config = GetStateConfig(data);
            if (config == null)
            {
                return null;
            }

            int index = GetStateFrameIndex(data);
            if (index >= 0 && index < config.frames.Count)
            {
                return config.frames[index];
            }

            return null;
        }

        public int GetStateLoopCnt(ActionMachineData data)
        {
            StateConfig config = GetStateConfig(data);
            int loopCnt = 0;
            if (!config.enableLoop)
            {
                return loopCnt;
            }

            int interval = data.frameIndex - data.stateBeginFrameIndex;
            int frameMax = config.frames.Count;
            if (interval + 1 > frameMax)
            {
                loopCnt = Mathf.FloorToInt(interval / frameMax);
            }

            return loopCnt;
        }

        public List<RangeConfig> GetAttackRanges(ActionMachineData data)
        {
            StateConfig sconfig = GetStateConfig(data);
            List<RangeConfig> ranges = sconfig.GetAttackRanges(GetStateFrameIndex(data));
            return ranges;
        }

        public List<RangeConfig> GetBodyRanges(ActionMachineData data)
        {
            StateConfig sconfig = GetStateConfig(data);
            List<RangeConfig> ranges = sconfig.GetBodyRanges(GetStateFrameIndex(data));
            return ranges;
        }

        public void ChangeState(ActionMachineData data, string stateName, int priority = 0, int animIndex = -1, Single animStartTime = default)
        {
            if (!string.IsNullOrEmpty(stateName) && priority < data.nextStatePriority)
            {
                return;
            }

            data.nextStateName = stateName;
            data.nextStatePriority = priority;
            data.nextAnimIndex = animIndex;
            data.nextAnimStartTime = animStartTime;
        }

        #endregion state

        #endregion operations
    }
}