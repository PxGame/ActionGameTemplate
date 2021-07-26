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
        public ModuleManager manager { get; set; }

        private GameInput.PlayerActions playerInput;

        public bool isJump => playerInput.Jump.triggered;
        public bool isDash => playerInput.Dash.triggered;
        public bool isAttack => playerInput.Attack.triggered;
        public bool isAxis => playerInput.Axis.phase == InputActionPhase.Started;
        public Vector2 axisValue => playerInput.Axis.ReadValue<Vector2>();

        public void Destory()
        {
            SuperLog.Log("InputModule Destory");
            Game.gw.RemoveDebugPage(debugPageName);

            playerInput.Disable();
        }

        public void Initialize()
        {
            SuperLog.Log("InputModule Initialize");

            playerInput = Game.input.GetPlayerActions();
            playerInput.Enable();

            Game.gw.AddDebugPage(debugPageName, OnDebugGUI);
        }

        public void LogicUpdate()
        {
            Entity entity = Game.gw.GetPlayerEntity();
            if (entity == null) { return; }

            InputData input = entity.GetOrAddComponent<InputData>();
        }

        public void ViewUpdate()
        {
            DebugViewUpdate();
        }

        #region Debug GUI

#if UNITY_EDITOR

        private string debugPageName = "Input";
        private StringBuilder debugText = new StringBuilder();
        private int debugIndex = 0;
        private Vector2 debugAxisValue;
        private InputActionPhase debugAxisPhase;
        private bool debugIsAxis;

        [Conditional("UNITY_EDITOR")]
        private void DebugViewUpdate()
        {
            debugIsAxis = isAxis;
            debugAxisValue = axisValue;
            debugAxisPhase = playerInput.Axis.phase;

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

        private void OnDebugGUI()
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