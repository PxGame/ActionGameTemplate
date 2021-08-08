/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:25:31
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ViewModule
    /// </summary>
    public class ViewModule : IModule
    {
        public override void Destory()
        {
            //SuperLog.Log("ViewModule Destory");
        }

        public override void Initialize()
        {
            //SuperLog.Log("ViewModule Initialize");
        }

        public override void LogicUpdate()
        {
            foreach (var data in EntityManager.Foreach<TransformData, ViewData>())
            {
                if ((data.entity.status & EntityStatus.Destoryed) != 0)
                {
                    ViewManager.Destory(data.entity);
                }
                else if (!data.cmp2.isCreated)
                {
                    ViewManager.Create(data.entity);
                }
            }
        }

        public override void ViewUpdate()
        {
            foreach (var data in EntityManager.Foreach<TransformData, ViewData>())
            {
                ViewManager.Update(data.entity, data.cmp1, data.cmp2);
            }
        }
    }
}