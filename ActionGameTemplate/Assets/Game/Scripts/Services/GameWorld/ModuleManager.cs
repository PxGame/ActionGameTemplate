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
    public enum UpdateState
    {
        None,
        LogicUpdating,
        ViewUpdating,
    }

    /// <summary>
    /// ModuleManager
    /// </summary>
    public class ModuleManager : IManager, IEnumerable<IModule>
    {
        public UpdateState updateState { get; private set; }

        private List<IModule> modules = new List<IModule>();
        private float logicTimer;

        public ModuleManager()
        {
        }

        public void Initialize()
        {
            logicTimer = 0f;
            updateState = UpdateState.None;

            modules.Add(new InitModule());
            modules.Add(new BeginModule());
            modules.Add(new InputModule());
            modules.Add(new LogicModule());
            modules.Add(new EventModule());
            modules.Add(new PhysicModule());
            modules.Add(new ViewModule());
            modules.Add(new EndModule());

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

            Game.deltaTime = 0f;
        }

        public void Update()
        {
            UpdateLogic();
            UpdateView();
        }

        private void UpdateLogic()
        {
            Game.deltaTime = GameWorld.logicDeltaTime;
            updateState = UpdateState.LogicUpdating;
            logicTimer += GameWorld.renderDeltaTime;
            while (logicTimer > GameWorld.logicDeltaTime)
            {
                logicTimer -= GameWorld.logicDeltaTime;

                foreach (var module in modules)
                {
                    module.LogicUpdate();
                }
            }
            updateState = UpdateState.None;
            Game.deltaTime = 0f;
        }

        private void UpdateView()
        {
            Game.deltaTime = GameWorld.renderDeltaTime;
            updateState = UpdateState.ViewUpdating;
            foreach (var module in modules)
            {
                module.ViewUpdate();
            }
            updateState = UpdateState.None;
            Game.deltaTime = 0f;
        }

        #region IEnumerator<IModule>

        public IEnumerator<IModule> GetEnumerator() => modules.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => modules.GetEnumerator();

        #endregion IEnumerator<IModule>
    }
}