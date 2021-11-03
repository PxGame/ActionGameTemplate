/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/4 0:12:12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// Events
    /// </summary>
    public static class Events
    {
        private static int nextEventId = 1;

        /// <summary>
        /// 场景事件
        /// </summary>
        public static class Scene
        {
            /// <summary>
            /// 场景加载完成
            /// <para>void OnInitialized(ISubScene scene)</para>
            /// </summary>
            public readonly static int Initialized = nextEventId++;

            /// <summary>
            /// 场景开始卸载
            /// <para>void OnUnInitialize(ISubScene scene)</para>
            /// </summary>
            public readonly static int UnInitialize = nextEventId++;
        }
    }
}