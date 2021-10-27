/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/26 1:37:32
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// PhysicManager
    /// </summary>
    public class PhysicManager : IManager
    {
        private Dictionary<int, EntityPhysic> entity2physic = new Dictionary<int, EntityPhysic>();

        public void Destory()
        {
            foreach (var item in entity2physic)
            {
                DestoryEntityPhysic(item.Value);
            }
            entity2physic.Clear();
        }

        public void Initialize()
        {
        }

        public EntityPhysic Get(int entityId)
        {
            return entity2physic.TryGetValue(entityId, out EntityPhysic physic) ? physic : null;
        }

        public void Create(Entity entity)
        {
            if (entity == null || (entity.status & EntityStatus.PhysicCreated) != 0) { return; }

            PhysicData physicData = entity.GetComponent<PhysicData>();
            TransformData transformData = entity.GetComponent<TransformData>();
            if (physicData == null || transformData == null) { return; }

            EntityPhysic physic = CreateEntityPhysic(entity, transformData, physicData);
            entity2physic.Add(physic.entityId, physic);
            entity.status |= EntityStatus.PhysicCreated;
        }

        public void Destory(Entity entity)
        {
            if (entity == null || (entity.status & EntityStatus.PhysicCreated) == 0) { return; }

            entity.status &= ~EntityStatus.PhysicCreated;

            EntityPhysic physic = entity2physic[entity.id];
            entity2physic.Remove(entity.id);
            DestoryEntityPhysic(physic);
        }

        private EntityPhysic CreateEntityPhysic(Entity entity, TransformData transformData, PhysicData physicData)
        {
            GameObject obj = Game.resource.CreateGO(physicData.resourceTag);
            EntityPhysic physic = obj.GetComponent<EntityPhysic>();

            physic.entityId = entity.id;
            physic.transform.position = transformData.position;
            physic.transform.rotation = transformData.rotation;

            return physic;
        }

        private void DestoryEntityPhysic(EntityPhysic physic)
        {
            if (physic != null && physic.gameObject != null)
            {
                Game.resource.DestoryGO(physic.gameObject);
            }
        }
    }
}