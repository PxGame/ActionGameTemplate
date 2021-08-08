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
        public ActionMachineProcessor am { get; private set; }

        public override void Destory()
        {
            //SuperLog.Log("LogicModule Destory");
        }

        public override void Initialize()
        {
            //SuperLog.Log("LogicModule Initialize");
            am = new ActionMachineProcessor();

            DebugTool.AddGizmo(OnGizmo);
        }

        public override void LogicUpdate()
        {
        }

        public override void ViewUpdate()
        {
        }

        #region OnGizmo

#if UNITY_EDITOR

        private void OnGizmo()
        {
            foreach (var data in EntityManager.Foreach<TransformData>())
            {
                DrawUtility.G.DrawCross(1, data.cmp1.matrix);
            }
        }

#endif

        #endregion OnGizmo
    }
}