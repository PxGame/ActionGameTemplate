/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/25 2:35:31
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// AGTInitializer
    /// </summary>
    public class AGTInitializer : IAppInitializer
    {
        public string tag => "AGT";

        public void OnInitializing()
        {
        }

        public void OnRegistServices(XMLib.Application target, List<(Type, Action<object>)> serviceTypes)
        {
            serviceTypes.Add((typeof(DeviceService), t => Game.device = t as DeviceService));
            serviceTypes.Add((typeof(ResourceService), t => Game.resource = t as ResourceService));
            serviceTypes.Add((typeof(ArchiveService), t => Game.archive = t as ArchiveService));
            serviceTypes.Add((typeof(InputService), t => Game.input = t as InputService));
            serviceTypes.Add((typeof(CameraService), t => Game.camera = t as CameraService));
            serviceTypes.Add((typeof(AudioService), t => Game.audio = t as AudioService));
            serviceTypes.Add((typeof(UIService), t => Game.ui = t as UIService));
            serviceTypes.Add((typeof(SceneService), t => Game.scene = t as SceneService));

            target.Singleton<GameWorld>().OnResolving((b, obj) => Game.gw = obj as GameWorld).OnRelease((b, obj) => Game.gw = null);
            target.Bind<GameScene>();
        }

        public void OnInitialized()
        {
        }

        public void OnDisposed()
        {
        }

        public void OnDisposing()
        {
        }
    }
}