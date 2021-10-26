/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/26 23:08:47
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// EntityPhysic
    /// </summary>
    public class EntityPhysic : MonoBehaviour
    {
        public Rigidbody rigid => _rigid;

        [SerializeField]
        private Rigidbody _rigid;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_rigid == null)
            {
                _rigid = GetComponent<Rigidbody>();
            }
        }

#endif
    }
}