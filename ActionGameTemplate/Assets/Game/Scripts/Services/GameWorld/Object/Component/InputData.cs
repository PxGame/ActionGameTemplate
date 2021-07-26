/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/26 15:13:05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// InputData
    /// </summary>
    public class InputData : IComponentData
    {
        public InputStatus status;
        public float axisAngle;

        public void Reset()
        {
            status = InputStatus.None;
            axisAngle = 0;
        }
    }
}