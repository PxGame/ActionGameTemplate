/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:24:42
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// EndModule
    /// </summary>
    public class EndModule : IModule
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
            EntityManager.ApplyDestory();
        }

        public override void ViewUpdate()
        {
        }
    }
}