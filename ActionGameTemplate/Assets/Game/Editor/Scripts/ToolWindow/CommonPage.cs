/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/25 1:26:30
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// CommonPage
    /// </summary>
    public class CommonPage : AGTToolPage
    {
        public override string title => "公共";

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }

        public override void OnGUI()
        {
            if (GUILayout.Button("Test"))
            {
                A a = new A();

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(a, XMLib.DataUtility.jsonSetting);

                A b = Newtonsoft.Json.JsonConvert.DeserializeObject<A>(json, XMLib.DataUtility.jsonSetting);
                SuperLog.Log("Test");
            }

            if (GUILayout.Button("Test2"))
            {
                dynamic a = 1;
                SuperLog.Log(a.ToString());
                a = "AAAA";
                SuperLog.Log(a.ToString());
            }

            if (GUILayout.Button("Test3"))
            {
                ResolvedObject resolvedObject = new ResolvedObject(new A());
                var s = resolvedObject.ForeachChild(resolvedObject.fields[19]).ToList();
                resolvedObject.fields[25].SetValue("123");
            }
            if (GUILayout.Button("Test4"))
            {
                A a = new A();
                A b = a.DeepCopy();

                a.b = false;
                a.c = 2.0f;
                var buffer = a.ToBytes();
                var c = ObjectUtility.FromBytes<A>(buffer);
            }
        }

        public override void OnInit()
        {
        }
    }
}

[System.Serializable]
public class A
{
    //[AvailableTypes(typeof(HashSet<string>))]
    //public object j;

    // public object a = new SortedList();
    public object b = true;

    public object c = 1.5f;
    public object d = "Test";

    public object e = Vector2.one;

    public object f = Vector3.one;

    //public object g = AnimationCurve.Linear(0, 0, 1, 1);
    public object h = new TestObject();

    //public object i = new Dictionary<int, string>();
    //public object k = new Stack();
}

[System.Serializable]
public class TestObject
{
    //public SubTestObject test;

    public List<SubTestObject> test2 = new List<SubTestObject> {
    null,
    new SubTestObject(),
    null
    };

    [AvailableTypesFromParent(typeof(TestA))]
    public object test3;

    [AvailableItemTypes(typeof(string))]
    public List<object> objs = new List<object>() {
        new SubTestObject(),
        1,
        "Test",
        null,
        };
}

internal interface TestA
{
}

internal class TestB : TestA
{
    //[AvailableItemTypes(typeof(string))]
    [AvailableTypes(typeof(List<object>))]
    public object obj;
}

internal class TestC
{
    [AvailableItemTypesFromParent(typeof(TestA))]
    public object obj;
}

[System.Serializable]
public class SubTestObject
{
    public int a = 1;
    public string b = "Test";
    //public object c = new SortedList();
}