/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/7 0:41:07
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ViewObject
    /// </summary>
    public class ViewObject : TransformObject
    {
        public ViewData view { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            view = AddComponent<ViewData>();
        }

        public override void Destory()
        {
            view = null;
            base.Destory();
        }
    }

    public class ViewData : IComponentData
    {
        public void Reset()
        {
        }
    }
}