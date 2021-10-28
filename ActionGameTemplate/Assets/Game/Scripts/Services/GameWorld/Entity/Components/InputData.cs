/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/26 15:13:05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// InputData
    /// </summary>
    public class InputData : IComponentData
    {
        public InputStatus status;
        public byte axisValue;

        public void Reset()
        {
            status = InputStatus.None;
            axisValue = 0;
        }

        public void Append(InputData other)
        {
            this.status |= other.status;
            this.axisValue = other.axisValue;

            if ((other.status & InputStatus.Axis) == 0)
            {//矫正，如果最后没有输入摇杆，则去除已存的摇杆状态，否者axisValue可能被误使用，导致方向错误
                this.status &= ~InputStatus.Axis;
                this.axisValue = byte.MaxValue;
            }
        }

        /// <summary>
        /// 去除单帧的指令
        /// </summary>
        /// <param name="data"></param>
        public void RemoveOnceKeyCode()
        {
            this.status &= ~(InputStatus.Attack | InputStatus.Dash | InputStatus.Jump);
        }

        public void SetAxisFromDir(Vector2 dir)
        {
            float angle = dir == Vector2.zero ? 0 : Vector2.SignedAngle(dir, Vector2.up);
            this.axisValue = ByteAngle.AngleToByte(angle);
        }

        public float GetAngle()
        {
            return ByteAngle.ByteToAngle(this.axisValue);
        }

        public Vector3 GetDir()
        {
            float angle = this.GetAngle();
            return (Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward).normalized;
        }

        public Quaternion GetRotation()
        {
            float angle = this.GetAngle();
            return Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}