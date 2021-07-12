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
        public enum UpdateState
        {
            None,
            LogicUpdating,
            ViewUpdating,
        }

        public UpdateState updateState { get; private set; }

        private List<IModule> modules;
        private float logicTimer;

        private GameWorld gw;

        public void Initialize(GameWorld gw)
        {
            this.gw = gw;

            modules = new List<IModule>() {
            new BeginModule(),
            new InputModule(),
            new LogicModule(),
            new EventModule(),
            new PhysicModule(),
            new ViewModule(),
            new EndModule() };

            logicTimer = 0f;
            updateState = UpdateState.None;

            foreach (var module in modules)
            {
                module.Initialize(gw);
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
            updateState = UpdateState.LogicUpdating;
            logicTimer += GameWorld.deltaTime;
            while (logicTimer > GameWorld.logicDeltaTime)
            {
                logicTimer -= GameWorld.logicDeltaTime;

                foreach (var module in modules)
                {
                    module.LogicUpdate();
                }
            }
            updateState = UpdateState.None;
        }

        private void UpdateView()
        {
            updateState = UpdateState.ViewUpdating;
            foreach (var module in modules)
            {
                module.ViewUpdate();
            }
            updateState = UpdateState.None;
        }
    }
}