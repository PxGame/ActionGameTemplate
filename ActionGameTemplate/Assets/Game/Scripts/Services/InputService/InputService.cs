/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/27 22:56:27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// InputService
    /// </summary>
    public class InputService : IMonoStart, IMonoDestroy
    {
        private GameInput input;

        public GameInput.PlayerActions GetPlayerActions() => input.Player;

        public void OnMonoStart()
        {
            input = new GameInput();
            input.Enable();
        }

        public void OnMonoDestroy()
        {
            input.Disable();
        }
    }
}