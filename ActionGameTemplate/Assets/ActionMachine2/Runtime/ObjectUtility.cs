/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/9/9 23:47:45
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace XMLib
{
    using SerializationSurrogate;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// ObjectUtility
    /// </summary>
    public static class ObjectUtility
    {
        public static T DeepCopy<T>(this T target)
        {
            T result = default;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = Provider.CreateBinaryFormatter();
                bf.Serialize(ms, target);
                ms.Seek(0, SeekOrigin.Begin);
                result = (T)bf.Deserialize(ms);
            }
            return result;
        }

        public static byte[] ToBytes<T>(this T target)
        {
            byte[] result = default;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = Provider.CreateBinaryFormatter();
                bf.Serialize(ms, target);
                result = ms.ToArray();
            }
            return result;
        }

        public static T FromBytes<T>(byte[] buffer) => (T)FromBytes(buffer);

        public static object FromBytes(byte[] buffer)
        {
            object result = default;
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                BinaryFormatter bf = Provider.CreateBinaryFormatter();
                result = bf.Deserialize(ms);
            }
            return result;
        }
    }

    namespace SerializationSurrogate
    {
        public static class Provider
        {
            private static SurrogateSelector surrogateSelector;

            public static BinaryFormatter CreateBinaryFormatter()
            {
                BinaryFormatter formatter = new BinaryFormatter(GetSelector(), new StreamingContext(StreamingContextStates.All));
                return formatter;
            }

            private static SurrogateSelector GetSelector()
            {
                if (surrogateSelector != null)
                {
                    return surrogateSelector;
                }

#if UNITY_EDITOR
                var types = UnityEditor.TypeCache.GetTypesDerivedFrom<ISerializationSurrogateEx>();
#else
                Assembly assembly = Assembly.GetExecutingAssembly();
                var types = from t in assembly.GetTypes()
                            where t.IsClass && typeof(ISerializationSurrogateEx).IsAssignableFrom(t)
                            select t;
#endif

                surrogateSelector = new SurrogateSelector();
                foreach (var type in types)
                {
                    ISerializationSurrogateEx sse = Activator.CreateInstance(type) as ISerializationSurrogateEx;
                    surrogateSelector.AddSurrogate(sse.targetType, new StreamingContext(StreamingContextStates.All), sse);
                }

                return surrogateSelector;
            }
        }

        internal interface ISerializationSurrogateEx : ISerializationSurrogate
        {
            Type targetType { get; }
        }

        internal sealed class Vector2SS : ISerializationSurrogateEx
        {
            public Type targetType => typeof(Vector2);

            public void GetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            {
                Vector2 target = (Vector2)obj;
                info.AddValue("x", target.x);
                info.AddValue("y", target.y);
            }

            public object SetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
            {
                Vector2 target = (Vector2)obj;
                target.x = info.GetSingle("x");
                target.y = info.GetSingle("y");
                return target;
            }
        }

        internal sealed class Vector3SS : ISerializationSurrogateEx
        {
            public Type targetType => typeof(Vector3);

            public void GetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            {
                Vector3 target = (Vector3)obj;
                info.AddValue("x", target.x);
                info.AddValue("y", target.y);
                info.AddValue("z", target.z);
            }

            public object SetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
            {
                Vector3 target = (Vector3)obj;
                target.x = info.GetSingle("x");
                target.y = info.GetSingle("y");
                target.z = info.GetSingle("z");
                return target;
            }
        }

        internal sealed class Vector4SS : ISerializationSurrogateEx
        {
            public Type targetType => typeof(Vector4);

            public void GetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            {
                Vector4 target = (Vector4)obj;
                info.AddValue("x", target.x);
                info.AddValue("y", target.y);
                info.AddValue("z", target.z);
                info.AddValue("w", target.w);
            }

            public object SetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
            {
                Vector4 target = (Vector4)obj;
                target.x = info.GetSingle("x");
                target.y = info.GetSingle("y");
                target.z = info.GetSingle("z");
                target.w = info.GetSingle("w");
                return target;
            }
        }

        internal sealed class ColorSS : ISerializationSurrogateEx
        {
            public Type targetType => typeof(Color);

            public void GetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            {
                Color target = (Color)obj;
                info.AddValue("r", target.r);
                info.AddValue("g", target.g);
                info.AddValue("b", target.b);
                info.AddValue("a", target.a);
            }

            public object SetObjectData(object obj, SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
            {
                Color target = (Color)obj;
                target.r = info.GetSingle("r");
                target.g = info.GetSingle("g");
                target.b = info.GetSingle("b");
                target.a = info.GetSingle("a");
                return target;
            }
        }
    }
}