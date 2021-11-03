/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2020/9/24 22:52:56
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// AudioBase
    /// </summary>
    public class AudioBase : MonoBehaviour, ISubPoolCallback
    {
        [SerializeField]
        protected AudioSource _source;

        public AudioService service { get; private set; }

        public bool isLoop => _source.loop;

        public virtual bool isPlaying => _source.isPlaying;

        public virtual float volume { get => _source.volume; set => _source.volume = value; }

        public virtual void Initialize(AudioService service)
        {
            this.service = service;
        }

        public virtual void Play()
        {
            _source.Play();
        }

        public virtual void Stop()
        {
            _source.Stop();
        }

        public virtual void OnPopPool()
        {
        }

        public virtual void OnPushPool()
        {
            if (_source.isPlaying)
            {
                _source.Stop();
            }
            volume = 1f;//还原音量
        }

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (_source == null)
            {
                _source = GetComponent<AudioSource>();
            }
        }

#endif
    }
}