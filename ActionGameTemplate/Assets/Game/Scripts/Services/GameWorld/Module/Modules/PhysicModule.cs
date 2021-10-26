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

            Physics.autoSimulation = true;
        }

        public override void Initialize()
        {
            //SuperLog.Log("PhysicModule Initialize");
            DebugTool.AddGizmo(OnDebugGizmo);

            Physics.autoSimulation = false;
        }

        public override void LogicUpdate()
        {
            foreach (var (entity, physicData) in EntityManager.Foreach<PhysicData>())
            {
                if ((entity.status & EntityStatus.Destoryed) != 0)
                {
                    PhysicManager.Destory(entity);
                }
                else if ((entity.status & EntityStatus.PhysicCreated) == 0)
                {
                    PhysicManager.Create(entity);
                }
            }

            foreach (var (entity, transformData, physicData) in EntityManager.Foreach<TransformData, PhysicData>())
            {
                EntityPhysic physic = PhysicManager.Get(entity.id);
                if (physic == null) { continue; }

                physic.rigid.position = transformData.position;
                physic.rigid.rotation = transformData.rotation;
                physic.rigid.velocity = physicData.velocity;
            }

            Physics.Simulate(Game.deltaTime);

            foreach (var (entity, transformData, physicData) in EntityManager.Foreach<TransformData, PhysicData>())
            {
                EntityPhysic physic = PhysicManager.Get(entity.id);
                if (physic == null) { continue; }

                transformData.position = physic.rigid.position;
                transformData.rotation = physic.rigid.rotation;
                physicData.velocity = physic.rigid.velocity;
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