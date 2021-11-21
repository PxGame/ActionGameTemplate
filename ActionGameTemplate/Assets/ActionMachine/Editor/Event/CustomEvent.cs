/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/14 17:17:10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// InitEvent
    /// </summary>
    public class InitEvent : EventBase<InitEvent>
    {
        public MainElement mainElement { get; set; }

        public static InitEvent GetPooled(IEventHandler target, MainElement mainElement)
        {
            var evt = GetPooled();
            evt.target = target;
            evt.mainElement = mainElement;
            return evt;
        }
    }

    public class PackageChanged : EventBase<PackageChanged>
    {
        public ActionMachinePackage newPackage { get; set; }

        public static PackageChanged GetPooled(IEventHandler target, ActionMachinePackage newPackage)
        {
            var evt = GetPooled();
            evt.target = target;
            evt.newPackage = newPackage;
            return evt;
        }
    }

    public class DataChanged : EventBase<DataChanged>
    {
        public static DataChanged GetPooled(IEventHandler target)
        {
            var evt = GetPooled();
            evt.target = target;
            return evt;
        }
    }
}