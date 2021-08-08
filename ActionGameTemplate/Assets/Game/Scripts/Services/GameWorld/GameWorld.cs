/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/26 17:28:15
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static float renderDeltaTime => Time.unscaledDeltaTime;

        public ModuleManager modules { get; private set; }
        public EntityManager entities { get; private set; }
        public ViewManager views { get; private set; }

        public bool pause { get; set; } = false;

        private List<IManager> managers = new List<IManager>();

        public void OnMonoUpdate()
        {
            if (pause) { return; }

            modules.Update();
        }

        public void OnMonoDestroy()
        {
            DestoryManager();
        }

        public void OnMonoStart()
        {
            InitializeManager();
        }

        private void InitializeManager()
        {
            modules = new ModuleManager(); managers.Add(modules);
            entities = new EntityManager(); managers.Add(entities);
            views = new ViewManager(); managers.Add(views);

            foreach (var manager in managers)
            {
                manager.Initialize();
            }
        }

        private void DestoryManager()
        {
            foreach (var manager in managers)
            {
                manager.Destory();
            }
            managers.Clear();
        }

        #region Entity

        public Entity GetPlayerEntity()
        {
            foreach (var entity in entities)
            {
                TagData tag = entity.GetComponent<TagData>();
                if (tag != null && tag.value == EntityTag.Player)
                {
                    return entity;
                }
            }

            return null;
        }

        #endregion Entity
    }
}