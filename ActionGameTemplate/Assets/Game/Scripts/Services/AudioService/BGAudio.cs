/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2020/9/24 22:55:05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using DG.Tweening;

namespace AGT
{
    /// <summary>
    /// BGAudio
    /// </summary>
    public class BGAudio : AudioBase
    {
        [SerializeField]
        protected float _fadeInTime = 1f;

        [SerializeField]
        protected float _fadeOutTime = 1f;

        public bool fading { get; private set; }

        public override void Play()
        {
            base.Play();

            _source.DOKill(true);
            fading = true;
            _source.DOFade(1.0f, _fadeInTime).ChangeStartValue(0f).OnComplete(() =>
            {
                fading = false;
            });
        }

        public override void Stop()
        {
            _source.DOKill(true);
            fading = true;
            _source.DOFade(0f, _fadeInTime).ChangeStartValue(1f).OnComplete(() =>
            {
                fading = false;
                base.Stop();
            });
        }

        public override void OnPushPool()
        {
            _source.DOKill(true);
            base.OnPushPool();
        }
    }
}