/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/18 1:41:03
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// IViewControl
    /// </summary>
    public interface IViewControl
    {
        void OnViewUpdate(Entity entity, TransformData transformData, ViewData viewData, TimeData timeData);

        void OnViewCreate(Entity entity);

        void OnViewDestory(Entity entity);
    }
}