/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/6 1:35:02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// TransformObject
    /// </summary>
    public class TransformObject : BaseObject
    {
        public TransformData transform { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            transform = AddComponent<TransformData>();
        }

        public override void Destory()
        {
            transform = null;
            base.Destory();
        }
    }

    public class TransformData : IComponentData
    {
        public Vector3 position;
        public Quaternion rotation;

        public void Reset()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }
    }
}