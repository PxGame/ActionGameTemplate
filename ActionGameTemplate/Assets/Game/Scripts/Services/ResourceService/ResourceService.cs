/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/27 22:56:38
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XMLib;

using XMLib.AM;

namespace AGT
{
    /// <summary>
    /// ResourceService
    /// </summary>
    public class ResourceService : IServiceInitialize, IDisposable
    {
        private ObjectPool<string> _pool = new ObjectPool<string>();
        private GameObject _poolRoot;

        private const string ResourceDataPath = "Data/resources";
        private const string MachineConfigPath = "MachineConfig";
        private ResourceData _data;

        public IEnumerator OnServiceInitialize()
        {
            _poolRoot = new GameObject("[Pool]");
            GameObject.DontDestroyOnLoad(_poolRoot);

            TextAsset asset = Resources.Load<TextAsset>(ResourceDataPath);
            _data = DataUtility.FromJson<ResourceData>(asset.text);

            yield break;
        }

        public MachineConfig LoadMachineConfig(string configName)
        {
            TextAsset asset = Resources.Load<TextAsset>($"{MachineConfigPath}/{configName}");
            return DataUtility.FromJson<MachineConfig>(asset.text);
        }

        public GameObject FindGO(string tag)
        {
            if (!_data.tag2path.TryGetValue(tag, out var path))
            {
                throw new RuntimeException($"未找到 Tag 为 {tag} 的资源路径");
            }

            GameObject obj = Resources.Load<GameObject>(path);
            if (obj == null)
            {
                throw new RuntimeException($"未找到 Tag:路径 为 {tag}:{path} 的资源");
            }

            return obj;
        }

        public GameObject CreateGO(string tag)
        {
            IResourceItem item = _pool.Pop<IResourceItem>(tag);
            if (item != null)
            {
                item.transform.SetParent(null, false);
                SceneManager.MoveGameObjectToScene(item.gameObject, SceneManager.GetActiveScene());
                item.gameObject.SetActive(true);
                return item.gameObject;
            }
            else
            {
                GameObject preGO = FindGO(tag);
                return GameObject.Instantiate(preGO);
            }
        }

        public void DestoryGO(GameObject go)
        {
            IResourceItem item = go.GetComponent<IResourceItem>();
            if (item == null)
            {
                GameObject.Destroy(go);
                return;
            }

            item.transform.SetParent(_poolRoot.transform, false);
            item.gameObject.SetActive(false);

            _pool.Push(item);
        }

        public void Dispose()
        {
            if (_poolRoot != null)
            {
                GameObject.Destroy(_poolRoot);
                _poolRoot = null;
            }
        }
    }
}