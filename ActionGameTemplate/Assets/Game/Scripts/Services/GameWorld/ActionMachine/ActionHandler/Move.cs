/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/29 1:18:09
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.AM;

namespace AGT
{
    /// <summary>
    /// MoveConfig
    /// </summary>
    [Serializable]
    [ActionConfig(typeof(Move))]
    public class MoveConfig
    {
        public float moveSpeed;
    }

    /// <summary>
    /// Move
    /// </summary>
    public class Move : IActionHandler
    {
        public void Enter(ActionNode node)
        {
        }

        public void Exit(ActionNode node)
        {
        }

        public void Update(ActionNode node, float deltaTime)
        {
            MoveConfig config = (MoveConfig)node.config;

            Entity entity = Game.gw.entities.Get(node.entityId);
            InputData input = entity.GetComponent<InputData>();
            if ((input.status & InputStatus.Axis) != 0)
            {
                PhysicData physic = entity.GetComponent<PhysicData>();
                TransformData trans = entity.GetComponent<TransformData>();

                Vector3 velocity = input.GetDir() * config.moveSpeed;

                physic.velocity.x = velocity.x;
                physic.velocity.z = velocity.z;

                trans.rotation = input.GetRotation();
            }
        }
    }
}