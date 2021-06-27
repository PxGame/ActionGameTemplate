/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:26:42
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ModuleManager
    /// </summary>
    public class ModuleManager
    {
        private List<IModule> modules = new List<IModule>() {
            new BeginModule(),
            new InputModule(),
            new LogicModule(),
            new EventModule(),
            new PhysicModule(),
            new ViewModule(),
            new EndModule()};

        private float logicTimer;

        public void Initialize()
        {
            logicTimer = 0f;

            foreach (var module in modules)
            {
                module.Initialize();
            }
        }

        public void Destory()
        {
            foreach (var module in modules)
            {
                module.Destory();
            }
        }

        public void Update()
        {
            UpdateLogic();
            UpdateView();
        }

        private void UpdateLogic()
        {
            logicTimer += GameWorld.deltaTime;
            while (logicTimer > GameWorld.logicDeltaTime)
            {
                logicTimer -= GameWorld.logicDeltaTime;

                foreach (var module in modules)
                {
                    module.LogicUpdate();
                }
            }
        }

        private void UpdateView()
        {
            foreach (var module in modules)
            {
                module.ViewUpdate();
            }
        }
    }
}