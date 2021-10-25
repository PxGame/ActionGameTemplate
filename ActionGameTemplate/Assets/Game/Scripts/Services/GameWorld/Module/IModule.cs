/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/27 12:05:52
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// IModule
    /// </summary>
    public abstract class IModule
    {
        public GameWorld GameWorld => Game.gw;
        public ModuleManager ModuleManager => GameWorld.modules;
        public EntityManager EntityManager => GameWorld.entities;
        public ViewManager ViewManager => GameWorld.views;
        public PhysicManager PhysicManager => GameWorld.physics;
        public InputService InputService => Game.input;

        public abstract void Initialize();

        public abstract void Destory();

        public abstract void LogicUpdate();

        public abstract void ViewUpdate();
    }
}