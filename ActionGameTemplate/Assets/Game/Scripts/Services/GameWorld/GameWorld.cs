/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/26 17:28:15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// GameWorld
    /// </summary>
    public class GameWorld : IMonoUpdate, IMonoStart, IMonoDestroy
    {
        public static float logicDeltaTime => 1f / 30f;
        public static float deltaTime => Time.deltaTime;

        public ModuleManager module { get; private set; }
        public ObjectManager obj { get; private set; }
        public ActionMachineManager am { get; private set; }

        public void OnMonoUpdate()
        {
            module.Update();
        }

        public void OnMonoDestroy()
        {
            module.Destory();
        }

        public void OnMonoStart()
        {
            module = new ModuleManager() { gw = this };
            obj = new ObjectManager() { gw = this };
            am = new ActionMachineManager() { gw = this };

            module.Initialize();
        }
    }
}