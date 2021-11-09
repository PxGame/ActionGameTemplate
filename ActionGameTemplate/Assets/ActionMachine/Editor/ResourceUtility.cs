/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/9 21:15:05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// ResourceUtility
    /// </summary>
    public static class ResourceUtility
    {
        public static VisualTreeAsset LoadUXML(string name)
        {
            string path = string.Format("UXML/{0}", name);
            return Resources.Load<VisualTreeAsset>(path);
        }

        public static StyleSheet LoadUSS(string name)
        {
            string path = string.Format("USS/{0}", name);
            return Resources.Load<StyleSheet>(path);
        }
    }
}