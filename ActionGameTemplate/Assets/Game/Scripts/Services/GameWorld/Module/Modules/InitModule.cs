/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/26 15:19:58
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// InitModule
    /// </summary>
    public class InitModule : IModule
    {
        public override void Destory()
        {
            //SuperLog.Log("BeginModule Destory");
        }

        public override void Initialize()
        {
            //SuperLog.Log("BeginModule Initialize");
        }

        public override void LogicUpdate()
        {
            foreach (var (entity, timeData) in EntityManager.Foreach<TimeData>())
            {
                if (!float.IsNaN(timeData.nextTimeScale))
                {
                    timeData.timeScale = timeData.nextTimeScale >= 0f ? timeData.nextTimeScale : 0f;
                    timeData.nextTimeScale = float.NaN;
                }

                float deltaTime = Game.deltaTime * timeData.timeScale;

                timeData.logicTimer += deltaTime;
                timeData.renderTotalTime = (timeData.renderTotalTime - timeData.renderTimer) + deltaTime;
                timeData.renderTimer = 0f;
            }
        }

        public override void ViewUpdate()
        {
            foreach (var (entity, timeData) in EntityManager.Foreach<TimeData>())
            {
                float deltaTime = Game.deltaTime * timeData.timeScale;
                float nextRenderTimer = timeData.renderTimer + deltaTime;
                if (nextRenderTimer > timeData.renderTotalTime)
                {
                    nextRenderTimer = timeData.renderTotalTime;
                    deltaTime = nextRenderTimer - timeData.renderTimer;
                }

                timeData.renderDeltaTime = deltaTime;
                timeData.renderTimer = nextRenderTimer;
                timeData.renderTimeStep = (timeData.renderTimer > 0 && timeData.renderTotalTime > 0) ? timeData.renderTimer / timeData.renderTotalTime : 0;
            }
        }
    }
}