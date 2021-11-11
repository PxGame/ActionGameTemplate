/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/10 22:54:45
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace XMLib.AM
{
    /// <summary>
    /// AcionMachinePackage
    /// </summary>
    [CreateAssetMenu(menuName = "XMLib/Action Machine Package")]
    [System.Serializable]
    public class AcionMachinePackage : ScriptableObject
    {
        public ActionMachineData data;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(AcionMachinePackage))]
    public class AcionMachinePackageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var target = this.target as AcionMachinePackage;
            if (GUILayout.Button("A"))
            {
                target.data.objs.Add(new A());
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("B"))
            {
                target.data.objs.Add(new B());
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("C"))
            {
                target.data.objs.Add(new C());
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("D"))
            {
                Debug.Log($"{target.GetType().AssemblyQualifiedName}");
            }
        }
    }

    [System.Serializable]
    public class A
    {
        public int a = 10;
        public string b = "test";
    }

    [System.Serializable]
    public class B
    {
        public bool a = true;
        public float b = 0.125f;
    }

    [System.Serializable]
    public class C : B
    {
        public A data = new A();
    }

#endif
}