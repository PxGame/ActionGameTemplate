/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:24:53
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using XMLib;
using System.Text;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace AGT
{
    [Flags]
    public enum InputStatus
    {
        None = 0b0,
        Axis = 0b1,
        Attack = 0b10,
        Jump = 0b100,
        Dash = 0b1000
    }

    /// <summary>
    /// InputModule
    /// </summary>
    public class InputModule : IModule
    {
        private GameInput.PlayerActions playerActions;

        public bool isJump => playerActions.Jump.triggered;
        public bool isDash => playerActions.Dash.triggered;
        public bool isAttack => playerActions.Attack.triggered;
        public bool isAxis => playerActions.Axis.phase == InputActionPhase.Started;
        public Vector2 axisValue => playerActions.Axis.ReadValue<Vector2>();

        public override void Destory()
        {
            //SuperLog.Log("InputModule Destory");
            playerActions.Disable();

            DebugTool.RemovePage("输入");
            DebugTool.RemoveUpdate(OnDebugUpdate);
        }

        public override void Initialize()
        {
            //SuperLog.Log("InputModule Initialize");

            playerActions = InputService.GetPlayerActions();
            playerActions.Enable();

            DebugTool.AddPage("输入", OnDebugPage);
            DebugTool.AddUpdate(OnDebugUpdate);
        }

        public override void LogicUpdate()
        {
        }

        public override void ViewUpdate()
        {
            Entity entity = GameWorld.GetPlayerEntity();
            if (entity == null) { return; }

            InputData input = entity.GetOrAddComponent<InputData>();

            if (isJump)
            {
                input.status |= InputStatus.Jump;
            }
            if (isDash)
            {
                input.status |= InputStatus.Dash;
            }
            if (isAttack)
            {
                input.status |= InputStatus.Attack;
            }
            if (isAxis)
            {
                input.status |= InputStatus.Axis;
                input.SetAxisFromDir(axisValue);
            }
        }

        #region Debug GUI

#if UNITY_EDITOR

        private StringBuilder debugText = new StringBuilder();
        private int debugIndex = 0;
        private Vector2 debugAxisValue;
        private InputActionPhase debugAxisPhase;
        private bool debugIsAxis;

        private void OnDebugUpdate()
        {
            if (!playerActions.enabled) { return; }

            debugIsAxis = isAxis;
            debugAxisValue = axisValue;
            debugAxisPhase = playerActions.Axis.phase;

            if (isAttack)
            {
                debugText.Insert(0, $"{debugIndex++} : is Attack\n");
            }
            if (isDash)
            {
                debugText.Insert(0, $"{debugIndex++} : is Dash\n");
            }
            if (isJump)
            {
                debugText.Insert(0, $"{debugIndex++} : is Jump\n");
            }
        }

        private void OnDebugPage()
        {
            EditorGUILayout.TextField("Axis Phase", debugAxisPhase.ToString());
            EditorGUILayout.Toggle("Is Axis", debugIsAxis);
            EditorGUILayout.Vector2Field("Axis Value:", debugAxisValue);
            if (GUILayout.Button("清空日志"))
            {
                debugText.Clear();
            }

            int maxText = 512;
            if (debugText.Length > maxText)
            {
                debugText.Remove(maxText, debugText.Length - maxText);
            }
            GUILayout.TextArea(debugText.ToString());
        }

#endif

        #endregion Debug GUI
    }
}