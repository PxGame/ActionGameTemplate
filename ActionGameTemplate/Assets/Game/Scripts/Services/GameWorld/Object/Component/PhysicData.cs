/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/8 19:36:03
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// PhysicData
    /// </summary>
    public class PhysicData : IComponentData
    {
        public Vector3 velocity;

        public void Reset()
        {
            velocity = Vector3.zero;
        }
    }
}