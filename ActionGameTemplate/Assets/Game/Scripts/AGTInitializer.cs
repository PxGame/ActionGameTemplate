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

        public void OnRegistServices(XMLib.Application target, List<Tuple<Type, Action<object>>> serviceTypes)
        {
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(DeviceService), t => Game.device = t as DeviceService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(ResourceService), t => Game.resource = t as ResourceService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(ArchiveService), t => Game.archive = t as ArchiveService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(InputService), t => Game.input = t as InputService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(CameraService), t => Game.camera = t as CameraService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(AudioService), t => Game.audio = t as AudioService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(UIService), t => Game.ui = t as UIService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(SceneService), t => Game.scene = t as SceneService));
            serviceTypes.Add(new Tuple<Type, Action<object>>(typeof(GameWorld), t => Game.gw = t as GameWorld));
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