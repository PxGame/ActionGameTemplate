/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/11 1:32:29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// TimeData
    /// </summary>
    public class TimeData : IComponentData
    {
        public int logicFrameCount;
        public float logicTimer;
        public float renderTimer;
        public float beginRenderTimer;
        public float endRenderTimer;
        public int needUpdateLogicCount;
        public float renderDeltaTime;
        public float timeScale;
        public float renderTimeStep;

        public float nextTimeScale;

        public void Reset()
        {
            logicFrameCount = 0;
            logicTimer = 0f;
            renderTimer = 0f;
            beginRenderTimer = 0f;
            endRenderTimer = 0f;
            needUpdateLogicCount = 0;
            renderDeltaTime = 0f;
            timeScale = 1f;
            renderTimeStep = 0f;

            nextTimeScale = float.NaN;
        }
    }
}