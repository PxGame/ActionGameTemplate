/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/9 0:39:10
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ViewManager
    /// </summary>
    public class ViewManager : IManager
    {
        private Dictionary<int, EntityView> entity2view = new Dictionary<int, EntityView>();

        public void Initialize()
        {
        }

        public void Destory()
        {
            foreach (var item in entity2view)
            {
                DestoryEntityView(item.Value);
            }
            entity2view.Clear();
        }

        public EntityView Get(int entityId)
        {
            return entity2view.TryGetValue(entityId, out EntityView view) ? view : null;
        }

        private EntityView CreateEntityView(TransformData transformData, ViewData viewData)
        {
            GameObject preObj = Resources.Load<GameObject>(viewData.resourcePath);
            GameObject obj = GameObject.Instantiate(preObj);
            EntityView view = obj.GetComponent<EntityView>();

            view.transform.position = transformData.position;
            view.transform.rotation = transformData.rotation;

            return view;
        }

        private void DestoryEntityView(EntityView view)
        {
            if (view != null)
            {
                GameObject.Destroy(view.gameObject);
            }
        }

        public void Create(Entity entity)
        {
            ViewData viewData = entity.GetComponent<ViewData>();
            TransformData transformData = entity.GetComponent<TransformData>();
            if (viewData == null || viewData.isCreated || transformData == null) { return; }

            EntityView view = CreateEntityView(transformData, viewData);
            entity2view.Add(entity.id, view);
            viewData.isCreated = true;
        }

        public void Destory(Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            ViewData viewData = entity.GetComponent<ViewData>();
            if (viewData == null || !viewData.isCreated)
            {
                return;
            }

            EntityView view = entity2view[entity.id];
            entity2view.Remove(entity.id);
            DestoryEntityView(view);
            viewData.isCreated = false;
        }

        public void Update(Entity entity, TransformData transformData, ViewData viewData, TimeData timeData)
        {
            if (!entity2view.TryGetValue(entity.id, out EntityView view) || view == null)
            {
                return;
            }
            view.OnViewUpdate(entity, transformData, viewData, timeData);
        }
    }
}