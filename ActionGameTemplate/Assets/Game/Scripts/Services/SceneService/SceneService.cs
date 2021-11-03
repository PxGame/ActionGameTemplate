/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/27 22:56:48
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XMLib;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace AGT
{
    public static class SceneNames
    {
        public const string Game = "Game";
    }

    /// <summary>
    /// 场景服务
    /// </summary>
    public class SceneService : IServiceLateInitialize, IMonoDestroy, IMonoStart
    {
        [InjectObject] protected UIService ui { get; set; }
        [InjectObject] protected AudioService audio { get; set; }
        [InjectObject] protected ArchiveService archive { get; set; }

        public SuperLogHandler LogHandler = SuperLogHandler.Create("Scene");

        public Type lastSceneType { get; protected set; } = null;
        public ISubScene currentScene { get; protected set; } = null;

        public SceneService()
        {
        }

        public IEnumerator Load<T>(params object[] objs) where T : class, ISubScene
        {
            yield return Load(typeof(T), objs);
        }

        private IEnumerator Load(Type sceneType, params object[] objs)
        {
            if (currentScene != null)
            {
                App.Trigger(Events.Scene.UnInitialize, currentScene);
                yield return currentScene.Unload();

                lastSceneType = currentScene.GetType();
            }
            else
            {
                lastSceneType = null;
            }

            currentScene = App.Make(sceneType) as ISubScene;
            yield return currentScene.Load(objs);
            App.Trigger(Events.Scene.Initialized, currentScene);
        }

        public IEnumerator OnServiceLateInitialize()
        {
            yield break;
        }

        public void OnMonoStart()
        {
            App.StartCoroutine(WaittingLoaded());

            DebugTool.AddPage("场景", OnScenePage);
        }

        public void OnMonoDestroy()
        {
            DebugTool.RemovePage("场景");
            App.Off(this);
        }

        private IEnumerator WaittingLoaded()
        {
            switch (App.launchMode)
            {
                case LaunchMode.Normal:
                    yield return OnLoadNormal();
                    break;

                case LaunchMode.Debug:
                    yield return OnLoadNormal();
                    break;
            }
        }

        private IEnumerator OnLoadNormal()
        {
            yield return new WaitUntil(() => App.isInited);//等待初始化完成
            yield return Load<GameScene>();//加载主场景控制
        }

#if UNITY_EDITOR

        private void OnScenePage()
        {
        }

#endif
    }
}