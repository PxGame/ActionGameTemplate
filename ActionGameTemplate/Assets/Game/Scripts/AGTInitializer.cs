/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/25 2:35:31
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// AGTInitializer
    /// </summary>
    public class AGTInitializer : IAppInitializer
    {
        public string Tag => "AGT";

        public void OnRegistGlobalServiceTypes(XMLib.Application target, List<Type> serviceTypes)
        {
        }

        public void OnRegistServices(XMLib.Application target)
        {
        }
    }
}