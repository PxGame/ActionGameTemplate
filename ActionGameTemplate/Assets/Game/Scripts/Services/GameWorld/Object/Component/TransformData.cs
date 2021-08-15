/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/25 13:24:43
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// TransformData
    /// </summary>
    public class TransformData : IComponentData
    {
        public Vector3 position;
        public Quaternion rotation;

        public Vector3 lastPosition;
        public Quaternion lastRotation;

        public Matrix4x4 matrix => Matrix4x4.TRS(position, rotation, Vector3.one);

        public void Reset()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            lastPosition = Vector3.zero;
            lastRotation = Quaternion.identity;
        }
    }
}