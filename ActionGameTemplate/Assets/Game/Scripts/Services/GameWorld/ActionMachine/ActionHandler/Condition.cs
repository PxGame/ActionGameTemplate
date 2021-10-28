/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/29 1:40:18
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.AM;

namespace AGT
{
    [Flags]
    public enum CheckType : byte
    {
        None = 0b0000,
        FowardWall = 0b0001,
        Ground = 0b0010,
        Air = 0b0100,

        /// <summary>
        /// 部分匹配模式
        /// </summary>
        KeyCode = 0b1000,

        /// <summary>
        /// 全部匹配模式
        /// </summary>
        KeyCodeAll = 0b0001_0000,
    }

    public class ConditionTypesAttribute : ObjectTypesAttribute
    {
        public override Type baseType => typeof(Conditions.IItem);
    }

    /// <summary>
    /// ConditionConfig
    /// </summary>
    [Serializable]
    [ActionConfig(typeof(Condition))]
    public class ConditionConfig : HoldFrames
    {
        public string stateName;
        public int priority;

        /// <summary>
        /// 延迟调用跳转，动作最后一帧执行跳转,必须启用EnableBeginEnd，否则无效
        /// </summary>
        [EnableToggle()]
        [EnableToggleItem(nameof(enableBeginEnd))]
        public bool delayInvoke;

        /// <summary>
        /// 立即执行
        /// </summary>
        [EnableToggleItem(nameof(delayInvoke), nameof(enableBeginEnd))]
        public int forceFrameIndex;

        /// <summary>
        /// 每一帧都需要为真，才能在最后跳转
        /// </summary>
        [EnableToggleItem(nameof(delayInvoke), nameof(enableBeginEnd))]
        public bool allFrameCheck;

        [ConditionTypes]
        [SerializeReference]
        public List<Conditions.IItem> checker;

        public override string ToString()
        {
            return $"{GetType().Name} > {stateName} - {priority}";
        }
    }

    /// <summary>
    /// Condition
    /// </summary>
    public class Condition : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            ConditionConfig config = (ConditionConfig)node.config;
            //IActionMachine machine = node.actionMachine;
            //IActionController controller = (IActionController)node.actionMachine.controller;
            node.data = 0;

            //校验
            if (config.delayInvoke && !config.EnableBeginEnd())
            {
                throw new RuntimeException($"使用延迟调用(DelayInvoke)，必须启用区间(EnableBeginEnd)\n{node}");
            }
            //
        }

        public void Exit(ActionNode node)
        {
            //TSConditionConfig config = (TSConditionConfig)node.config;
            //IActionMachine machine = node.actionMachine;
            //IActionController controller = (IActionController)node.actionMachine.controller;
        }

        public void Update(ActionNode node, float deltaTime)
        {
            ConditionConfig config = (ConditionConfig)node.config;
            Entity entity = Game.gw.entities.Get(node.entityId);
            ActionMachineManager manager = node.manager;
            ActionMachineData actionMachineData = entity.GetComponent<ActionMachineData>();

            if (!config.delayInvoke || !config.EnableBeginEnd())
            {
                if (!Checker(config.checker, node))
                {
                    return;
                }
            }
            else
            {
                int successCnt = (int)node.data;
                if (Checker(config.checker, node))
                {//为true时计数+1
                    node.data = ++successCnt;
                }

                if (successCnt != 0
                && ((manager.GetStateFrameIndex(actionMachineData) == config.GetEndFrame()) //到达最后一帧
                    || (config.forceFrameIndex > 0 && config.forceFrameIndex < node.updateCnt)) //强制执行
                && (!config.allFrameCheck || successCnt == (node.updateCnt + 1)))//每一帧都必须为true,updateCnt需要+1是因为updateCnt在Update后才会递增
                {
                }
                else
                {
                    return;
                }
            }

            manager.ChangeState(actionMachineData, config.stateName, config.priority);
        }

        public static bool Checker(List<Conditions.IItem> checkers, ActionNode node)
        {
            if (checkers == null || checkers.Count == 0)
            {
                return true;
            }

            foreach (var checker in checkers)
            {
                if (!checker.Execute(node))
                {
                    return false;
                }
            }

            return true;
        }
    }

    namespace Conditions
    {
        #region Items

        public interface IItem
        {
            bool Execute(ActionNode node);
        }

        [Serializable]
        public class KeyCodeChecker : IItem
        {
            public InputStatus status;
            public bool isNot;

            public bool Execute(ActionNode node)
            {
                Entity entity = Game.gw.entities.Get(node.entityId);
                InputData input = entity.GetComponent<InputData>();
                bool result = (input.status & status) != 0;
                return isNot ? !result : result;
            }
        }

        #endregion Items
    }
}