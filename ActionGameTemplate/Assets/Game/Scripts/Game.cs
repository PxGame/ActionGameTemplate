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
        public static DeviceService device { get; set; } = null;
        public static ResourceService resource { get; set; } = null;
        public static ArchiveService archive { get; set; } = null;
        public static InputService input { get; set; } = null;
        public static CameraService camera { get; set; } = null;
        public static AudioService audio { get; set; } = null;
        public static UIService ui { get; set; } = null;
        public static SceneService scene { get; set; } = null;
        public static GameWorld gw { get; set; } = null;

        /// <summary>
        /// 仅用于GameWorld中的循环，根据不同循环，进入循环前会自动赋值对应的deltaTime，退出循环后会重置为0
        /// </summary>
        public static float deltaTime { get; set; } = 0f;
    }
}