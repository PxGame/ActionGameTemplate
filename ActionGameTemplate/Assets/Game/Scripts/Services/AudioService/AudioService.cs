/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/27 22:55:58
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace AGT
{
    /// <summary>
    /// AudioService
    /// </summary>
    public class AudioService : IServiceInitialize, IMonoStart, IMonoDestroy, IMonoUpdate
    {
        [InjectObject] protected ResourceService res { get; set; }

        protected List<AudioBase> audios = new List<AudioBase>();
        private Transform audioRoot;
        public SuperLogHandler LogHandler = SuperLogHandler.Create("Audio");

        public AudioBase Play(string resourceTag)
        {
            return Play(resourceTag, Vector3.zero);
        }

        public BGAudio PlayBGM(string resourceTag, bool forceChange = false)
        {
            if (!forceChange)
            {
                foreach (var item in audios)
                {
                    if (item is BGAudio bga)
                    {
                        bga.Stop();
                    }
                }
            }
            else
            {
                for (int i = 0; i < audios.Count; i++)
                {
                    if (audios[i] is BGAudio bga)
                    {
                        audios.RemoveAt(i);
                        res.DestoryGO(bga.gameObject);
                        i--;
                    }
                }
            }

            GameObject obj = res.CreateGO(resourceTag);
            obj.transform.SetParent(audioRoot);

            BGAudio audio = obj.GetComponent<BGAudio>();
            audios.Add(audio);
            audio.Initialize(this);
            audio.Play();

            return audio;
        }

        public AudioBase Play(string resourceTag, Vector3 position)
        {
            GameObject obj = res.CreateGO(resourceTag);
            obj.transform.SetParent(audioRoot);
            obj.transform.position = position;

            AudioBase audio = obj.GetComponent<AudioBase>();
            audios.Add(audio);
            audio.Initialize(this);
            audio.Play();
            return audio;
        }

        public IEnumerator OnServiceInitialize()
        {
            audioRoot = new GameObject("[Audio]").transform;
            GameObject.DontDestroyOnLoad(audioRoot.gameObject);

            yield break;
        }

        public void OnMonoStart()
        {
            DebugTool.AddPage("声音", OnAudioPage);
        }

        public void OnMonoDestroy()
        {
            DebugTool.RemovePage("声音");

            if (null != audioRoot)
            {
                GameObject.Destroy(audioRoot.gameObject);
                audioRoot = null;
                audios.Clear();
            }
        }

        public void OnMonoUpdate()
        {
            for (int i = 0; i < audios.Count; i++)
            {
                AudioBase audio = audios[i];
                if (!audio.isPlaying)
                {//回收
                    audios.RemoveAt(i);
                    res.DestoryGO(audio.gameObject);
                    i--;
                }
            }
        }

        #region 声音 page

#if UNITY_EDITOR

        public string _bgmTagTmp;
        public string _effectTagTmp;

        private void OnAudioPage()
        {
            GUILayout.BeginHorizontal();
            _bgmTagTmp = EditorGUILayout.TextField("背景音乐Tag", _bgmTagTmp);
            if (GUILayout.Button("切换", GUILayout.Width(100)))
            {
                PlayBGM(_bgmTagTmp);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            _effectTagTmp = EditorGUILayout.TextField("音效Tag", _effectTagTmp);
            if (GUILayout.Button("播放", GUILayout.Width(100)))
            {
                Play(_effectTagTmp);
            }
            GUILayout.EndHorizontal();
        }

#endif

        #endregion 声音 page
    }
}