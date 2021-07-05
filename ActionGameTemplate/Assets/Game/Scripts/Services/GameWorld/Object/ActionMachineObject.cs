/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/6 1:35:55
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ActionMachineObject
    /// </summary>
    public class ActionMachineObject : TransformObject
    {
        public ActionMachineData actionMachine { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            actionMachine = AddComponent<ActionMachineData>();
        }

        public override void Destory()
        {
            actionMachine = null;
            base.Destory();
        }
    }
}