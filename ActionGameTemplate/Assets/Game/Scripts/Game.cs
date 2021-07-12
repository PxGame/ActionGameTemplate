/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/13 0:33:02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// Game
    /// </summary>
    public static class Game
    {
        public static DeviceService device;
        public static ResourceService resource;
        public static ArchiveService archive;
        public static InputService input;
        public static CameraService camera;
        public static AudioService audio;
        public static UIService ui;
        public static SceneService scene;
        public static GameWorld gw;
    }
}