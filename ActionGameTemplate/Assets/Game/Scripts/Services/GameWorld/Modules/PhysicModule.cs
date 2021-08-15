/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:25:22
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// PhysicModule
    /// </summary>
    public class PhysicModule : IModule
    {
        public override void Destory()
        {
            //SuperLog.Log("PhysicModule Destory");
            DebugTool.RemoveGizmo(OnDebugGizmo);
        }

        public override void Initialize()
        {
            //SuperLog.Log("PhysicModule Initialize");
            DebugTool.AddGizmo(OnDebugGizmo);
        }

        public override void LogicUpdate()
        {
            foreach (var (entity, transformData, physicData, timeData) in EntityManager.Foreach<TransformData, PhysicData, TimeData>())
            {
                float deltaTime = timeData.timeScale * Game.deltaTime;

                transformData.lastPosition = transformData.position;
                transformData.lastRotation = transformData.rotation;

                transformData.rotation = (transformData.rotation * Quaternion.Euler(Vector3.one * 10 * deltaTime)).normalized;
                transformData.position += physicData.velocity * deltaTime;
            }
        }

        public override void ViewUpdate()
        {
        }

#if UNITY_EDITOR

        private void OnDebugGizmo()
        {
            foreach (var (entity, transformData) in EntityManager.Foreach<TransformData>())
            {
                DrawUtility.G.DrawAxes(transformData.matrix);
            }
        }

#endif
    }
}