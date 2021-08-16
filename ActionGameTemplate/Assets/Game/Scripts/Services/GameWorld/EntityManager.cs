/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:26:28
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.Extensions;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace AGT
{
    /// <summary>
    /// EntityManager
    /// </summary>
    public class EntityManager : IManager, IEnumerable<Entity>
    {
        public static readonly int NoneID = 0;
        private int nextId = 1;

        private LinkedList<Entity> entities = new LinkedList<Entity>();

        private Dictionary<int, Entity> id2entity = new Dictionary<int, Entity>();

        private List<int> _createIds = new List<int>();
        private List<int> _destoryIds = new List<int>();

        public IReadOnlyList<int> createIds => _createIds;
        public IReadOnlyList<int> destoryIds => _destoryIds;

        public Entity Create()
        {
            Entity entity = ObjectUtility.PopObject<Entity>();
            entity.id = nextId++;

            id2entity.Add(entity.id, entity);
            _createIds.Add(entity.id);

            return entity;
        }

        public void Initialize()
        {
            DebugTool.AddPage("实体", OnDebugPage);
        }

        public void Destory()
        {
            DebugTool.RemovePage("实体");
        }

        public void Destory(Entity entity)
        {
            if (entity == null || (entity.status & EntityStatus.Destoryed) != 0) { return; }
            entity.status |= EntityStatus.Destoryed;
            _destoryIds.Add(entity.id);
        }

        public void Destory(int entityId)
        {
            Entity entity = Get(entityId);
            Destory(entity);
        }

        public Entity Get(int id)
        {
            return id2entity.TryGetValue(id, out Entity entity) ? entity : null;
        }

        public void ApplyInitialize()
        {
            if (_createIds.Count <= 0) { return; }
            foreach (var id in _createIds)
            {
                Entity entity = id2entity[id];
                entities.AddLast(entity);
                entity.Initialize();
                entity.status |= EntityStatus.Inited;
            }
            _createIds.Clear();
        }

        public void ApplyDestory()
        {
            if (_destoryIds.Count <= 0) { return; }
            foreach (var id in _destoryIds)
            {
                Entity entity = id2entity[id];
                if ((entity.status & EntityStatus.Inited) != 0)
                {
                    entity.Destory();
                    entities.Remove(entity);
                }
                id2entity.Remove(entity.id);
                ObjectUtility.PushObject(entity);
            }
            _destoryIds.Clear();
        }

        #region Entity Foreach

        public IEnumerable<(Entity, T1)> Foreach<T1>()
            where T1 : class, IComponentData, new()
        {
            foreach (var entity in entities)
            {
                T1 cmp1 = entity.GetComponent<T1>();

                if (cmp1 == null) { continue; }

                yield return (entity, cmp1);
            }
        }

        public IEnumerable<(Entity, T1, T2)> Foreach<T1, T2>()
            where T1 : class, IComponentData, new()
            where T2 : class, IComponentData, new()
        {
            foreach (var entity in entities)
            {
                T1 cmp1 = entity.GetComponent<T1>();
                if (cmp1 == null) { continue; }

                T2 cmp2 = entity.GetComponent<T2>();
                if (cmp2 == null) { continue; }

                yield return (entity, cmp1, cmp2);
            }
        }

        public IEnumerable<(Entity, T1, T2, T3)> Foreach<T1, T2, T3>()
            where T1 : class, IComponentData, new()
            where T2 : class, IComponentData, new()
            where T3 : class, IComponentData, new()
        {
            foreach (var entity in entities)
            {
                T1 cmp1 = entity.GetComponent<T1>();
                if (cmp1 == null) { continue; }

                T2 cmp2 = entity.GetComponent<T2>();
                if (cmp2 == null) { continue; }

                T3 cmp3 = entity.GetComponent<T3>();
                if (cmp3 == null) { continue; }

                yield return (entity, cmp1, cmp2, cmp3);
            }
        }

        #endregion Entity Foreach

        #region IEnumerable<Entity>

        public IEnumerator<Entity> GetEnumerator() => entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => entities.GetEnumerator();

        #endregion IEnumerable<Entity>

        #region Entity GUI

#if UNITY_EDITOR

        private Vector2 debugScrollPos = Vector2.zero;
        private Vector2 debugScrollPos2 = Vector2.zero;
        private int debugEntityId = 0;
        private bool debugEnableEdit = false;
        private int debugCreateCnt = 1;

        private void OnDebugPage()
        {
            debugCreateCnt = EditorGUILayout.IntField("创建数量", debugCreateCnt);
            if (GUILayout.Button("创建实体"))
            {
                for (int i = 0; i < debugCreateCnt; i++)
                {
                    Entity entity = Create();
                    entity.AddComponent<TagData>();
                    entity.AddComponent<ActionMachineData>();
                    entity.AddComponent<InputData>();
                    var tran = entity.AddComponent<TransformData>();
                    var view = entity.AddComponent<ViewData>();
                    entity.AddComponent<PhysicData>();
                    entity.AddComponent<TimeData>();

                    tran.position = UnityEngine.Random.insideUnitSphere * 10;
                    tran.rotation = UnityEngine.Random.rotation;

                    view.resourcePath = "Cube";
                }
            }

            List<int> ids = ListPool<int>.Pop();
            ids.AddRange(id2entity.Keys);
            ids.Sort();

            GUILayout.BeginHorizontal();
            using (var scroll = new EditorGUILayout.ScrollViewScope(debugScrollPos, GUILayout.Width(200)))
            {
                foreach (var id in ids)
                {
                    Entity entity = id2entity[id];

                    GUILayout.BeginHorizontal("ProfilerDetailViewBackground");
                    Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                    EditorGUI.DrawRect(rect, GetStatueStyle(entity));

                    EditorGUILayout.LabelField(entity.id.ToString(), GUILayout.Width(50));

                    if (GUILayout.Button("查看"))
                    {
                        debugEntityId = id;
                        EntityView view = Game.gw.views.Get(id);
                        if (view != null)
                        {
                            Selection.activeObject = view.gameObject;

                            if (SceneView.lastActiveSceneView != null)
                            {
                                SceneView.lastActiveSceneView.LookAt(view.gameObject.transform.position);
                            }
                        }
                    }

                    if (GUILayout.Button("销毁"))
                    {
                        Destory(entity);
                    }

                    GUILayout.EndHorizontal();
                }

                debugScrollPos = scroll.scrollPosition;
            }

            GUILayout.BeginVertical();
            debugEnableEdit = EditorGUILayoutEx.DrawObject("启用编辑", debugEnableEdit);
            using (var scroll = new EditorGUILayout.ScrollViewScope(debugScrollPos2, GUI.skin.box))
            {
                Entity entity = Get(debugEntityId);
                if (entity != null)
                {
                    using (var disable = new EditorGUI.DisabledGroupScope(!debugEnableEdit))
                    {
                        EditorGUILayoutEx.DrawObject(string.Empty, entity);
                    }
                }
                debugScrollPos2 = scroll.scrollPosition;
            }
            GUILayout.EndVertical();

            ListPool<int>.Push(ids);
            GUILayout.EndHorizontal();

            return;

            Color GetStatueStyle(Entity entity)
            {
                if ((entity.status & EntityStatus.Destoryed) != 0) { return Color.red; }
                if ((entity.status & EntityStatus.InitedAndViewCreated) == EntityStatus.InitedAndViewCreated) { return new Color(0, 0.74f, 0); }
                if ((entity.status & EntityStatus.Inited) != 0) { return Color.blue; }
                return Color.white;
            }
        }

        [ObjectDrawer(typeof(Entity))]
        public static object EntityDrawer(GUIContent title, object obj, Type type, object[] attrs)
        {
            Entity entity = obj as Entity;

            EditorGUILayoutEx.DrawObject("ID", entity.id);
            EditorGUILayoutEx.DrawObject("Status", entity.status);

            foreach (var cmpPair in entity.components)
            {
                EditorGUILayoutEx.DrawObject(cmpPair.Key.GetSimpleName(), cmpPair.Value, cmpPair.Key);
            }

            return obj;
        }

#endif

        #endregion Entity GUI
    }

    public static class ObjectUtility
    {
        private static MultiTypeObjectPool<IComponentData> _componentPool;
        private static MultiTypeObjectPool<Entity> _entityPool;

        static ObjectUtility()
        {
            _componentPool = new MultiTypeObjectPool<IComponentData>()
            {
                onNew = t => t.Reset(),
                onPush = t => t.Reset()
            };
            _entityPool = new MultiTypeObjectPool<Entity>()
            {
                onNew = t => t.Reset(),
                onPush = t => t.Reset()
            };
        }

        public static T PopComponent<T>() where T : class, IComponentData, new() => _componentPool.Pop<T>();

        public static void PushComponent<T>(T component) where T : class, IComponentData => _componentPool.Push(component);

        public static T PopObject<T>() where T : Entity, new() => _entityPool.Pop<T>();

        public static void PushObject<T>(T entity) where T : Entity => _entityPool.Push(entity);
    }
}