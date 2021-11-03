/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2020/10/26 15:12:17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XMLib;

namespace AGT
{
    /// <summary>
    /// GameScene
    /// </summary>
    public class GameScene : SubScene<GameWorld>
    {
        public GameScene(SceneService scene) : base(scene)
        {
        }

        public override IEnumerator Load(params object[] objs)
        {
#if UNITY_EDITOR
            if (App.launchMode == LaunchMode.Debug && SceneManager.GetActiveScene().name.Equals(SceneNames.Game))
            {
                yield return base.Load(objs);
                yield break;
            }
#endif

            yield return SceneManager.LoadSceneAsync(SceneNames.Game);
            yield return base.Load(objs);
        }

        public override IEnumerator Unload()
        {
            yield return base.Unload();
        }
    }
}