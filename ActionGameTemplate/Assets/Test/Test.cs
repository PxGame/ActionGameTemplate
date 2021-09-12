/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/10 15:23:53
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XMLib;

namespace XMLib
{
    [System.Serializable]
    public class A
    {
        public int a;
        public string b;
        public B c;
        public List<B> d;
    }

    [System.Serializable]
    public class B
    {
        public bool e;
        public float f;
    }

    /// <summary>
    /// Test
    /// </summary>
    public class Test : MonoBehaviour
    {
        public A a;
    }

    [CustomEditor(typeof(Test))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var a = serializedObject.FindProperty("a");

            foreach (SerializedProperty item in a)
            {
            }
        }
    }
}