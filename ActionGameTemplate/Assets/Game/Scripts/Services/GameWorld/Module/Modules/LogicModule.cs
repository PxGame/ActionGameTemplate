/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:25:02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// LogicModule
    /// </summary>
    public class LogicModule : IModule
    {
        public override void Destory()
        {
            //SuperLog.Log("LogicModule Destory");
            DebugTool.RemoveGizmo(OnGizmo);
        }

        public override void Initialize()
        {
            //SuperLog.Log("LogicModule Initialize");
            DebugTool.AddGizmo(OnGizmo);
        }

        public override void LogicUpdate()
        {
            foreach (var (entity, actionMachineData) in EntityManager.Foreach<ActionMachineData>())
            {
                ActionMachineManager.Update(entity, actionMachineData, Game.deltaTime);
            }
        }

        public override void ViewUpdate()
        {
        }

        #region OnGizmo

#if UNITY_EDITOR

        private void OnGizmo()
        {
        }

#endif

        #endregion OnGizmo
    }
}