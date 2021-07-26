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

        public bool pause { get; set; } = false;

        public void OnMonoUpdate()
        {
            if (pause) { return; }

            modules.Update();
        }

        public void OnMonoDestroy()
        {
            modules.Destory();
        }

        public void OnMonoStart()
        {
            modules = new ModuleManager();
            entities = new EntityManager();

            modules.Initialize();
        }

        #region Entity

        public Entity GetPlayerEntity()
        {
            foreach (var entity in entities)
            {
                if (entity.type == EntityType.Player)
                {
                    return entity;
                }
            }

            return null;
        }

        #endregion Entity

#if UNITY_EDITOR

        #region Debug Page

        public IReadOnlyDictionary<string, Action> debugPageDict => _debugPageDict;

        private Dictionary<string, Action> _debugPageDict = new Dictionary<string, Action>();

        [Conditional("UNITY_EDITOR")]
        public void AddDebugPage(string pageName, Action onDraw)
        {
            _debugPageDict.Add(pageName, onDraw);
        }

        [Conditional("UNITY_EDITOR")]
        public void RemoveDebugPage(string pageName)
        {
            _debugPageDict.Remove(pageName);
        }

        #endregion Debug Page

#endif
    }
}