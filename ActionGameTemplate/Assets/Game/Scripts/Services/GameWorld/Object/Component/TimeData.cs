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
        public float logicTimer;

        public float renderTotalTime;
        public float renderTimer;
        public float renderDeltaTime;
        public float renderTimeStep;

        public float timeScale;
        public float nextTimeScale;

        public void Reset()
        {
            logicTimer = 0f;
            renderTimer = 0f;
            renderTotalTime = 0f;
            renderDeltaTime = 0f;
            renderTimeStep = 0f;

            timeScale = 1f;
            nextTimeScale = float.NaN;
        }
    }
}