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

                timeData.logicTimer += Game.deltaTime * timeData.timeScale;

                int frameCount = (int)(timeData.logicTimer / Game.deltaTime);
                if (frameCount > timeData.logicFrameCount)
                {
                    timeData.needUpdateLogicCount = frameCount - timeData.logicFrameCount;
                    timeData.logicFrameCount = frameCount;

                    timeData.beginRenderTimer = timeData.endRenderTimer;
                    timeData.endRenderTimer += timeData.needUpdateLogicCount * Game.deltaTime;
                }
                else
                {
                    timeData.needUpdateLogicCount = 0;
                }
            }
        }

        public override void ViewUpdate()
        {
            foreach (var (entity, timeData) in EntityManager.Foreach<TimeData>())
            {
                timeData.renderDeltaTime = Game.deltaTime * timeData.timeScale;

                float nextRenderTimer = timeData.renderTimer + timeData.renderDeltaTime;
                if (nextRenderTimer > timeData.endRenderTimer)
                {
                    nextRenderTimer = timeData.endRenderTimer;
                    timeData.renderDeltaTime = nextRenderTimer - timeData.renderTimer;
                }
                timeData.renderTimer = nextRenderTimer;
            }
        }
    }
}