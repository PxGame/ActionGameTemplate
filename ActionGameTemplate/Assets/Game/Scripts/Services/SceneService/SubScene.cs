/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2020/10/26 15:05:59
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    public interface ISubScene
    {
        IEnumerator Load(params object[] objs);

        IEnumerator Unload();
    }

    /// <summary>
    /// SubScene
    /// </summary>
    public abstract class SubScene<T> : ISubScene where T : class, IWorld
    {
        public SceneService scene { get; private set; }
        public T world { get; private set; }

        public SubScene(SceneService scene)
        {
            this.scene = scene;
        }

        public virtual IEnumerator Load(params object[] objs)
        {
            world = App.Make<T>(objs);
            yield return world.Initialize();
        }

        public virtual IEnumerator Unload()
        {
            world = App.Make<T>();
            yield return world.UnInitialize();
            App.Release(world);
            world = null;
        }
    }
}