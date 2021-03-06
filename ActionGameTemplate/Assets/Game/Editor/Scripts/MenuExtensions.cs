/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/25 1:30:50
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// MenuExtensions
    /// </summary>
    public class MenuExtensions
    {
        public const string Namespace = "AGT";

        [MenuItem("Assets/Create/AGT/Mono 脚本", priority = 0)]
        public static void CreateMonoScript()
        {
            ScriptCreator.CreateFile("MonoScript", Mono_cs, defaultNS: Namespace);
        }

        [MenuItem("Assets/Create/AGT/Simple 脚本", priority = 0)]
        public static void CreateSimpleScript()
        {
            ScriptCreator.CreateFile("Script", Simple_cs, defaultNS: Namespace);
        }

        [MenuItem("Assets/Create/AGT/ActionHandler 脚本", priority = 0)]
        public static void CreateActionHandlerScript()
        {
            ScriptCreator.CreateFile("ActionHandler", ActionHandler_cs, defaultNS: Namespace);
        }

        public const string Mono_cs = "/*\n * 作者：#AUTHOR#\n * 联系方式：#CONTACT#\n * 文档: #DOC#\n * 创建时间: #CREATEDATE#\n */\n\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing XMLib;\n\nnamespace #NS#\n{\n    /// <summary>\n    /// #SCRIPTNAME#\n    /// </summary>\n    public class #SCRIPTNAME# : MonoBehaviour \n    {\n    }\n}";
        public const string Simple_cs = "/*\n * 作者：#AUTHOR#\n * 联系方式：#CONTACT#\n * 文档: #DOC#\n * 创建时间: #CREATEDATE#\n */\n\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing XMLib;\n\nnamespace #NS#\n{\n    /// <summary>\n    /// #SCRIPTNAME#\n    /// </summary>\n    public class #SCRIPTNAME# \n    {\n    }\n}";
        public const string ActionHandler_cs = "/*\n * 作者：#AUTHOR#\n * 联系方式：#CONTACT#\n * 文档: #DOC#\n * 创建时间: #CREATEDATE#\n*/\n\nusing System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing XMLib;\nusing XMLib.AM;\n\nusing FPPhysics;\nusing Vector2 = FPPhysics.Vector2;\nusing Vector3 = FPPhysics.Vector3;\nusing Quaternion = FPPhysics.Quaternion;\nusing Matrix4x4 = FPPhysics.Matrix4x4;\nusing UVector2 = UnityEngine.Vector2;\nusing UVector3 = UnityEngine.Vector3;\nusing UQuaternion = UnityEngine.Quaternion;\nusing UMatrix4x4 = UnityEngine.Matrix4x4;\nusing Mathf = FPPhysics.FPUtility;\nusing Single = FPPhysics.Fix64;\nusing MathUtility = FPPhysics.FPUtility;\n\nnamespace #NS#\n{\n    /// <summary>\n    /// #SCRIPTNAME#Config\n    /// </summary>\n    [Serializable]\n    [ActionConfig(typeof(#SCRIPTNAME#))]\n    public class #SCRIPTNAME#Config\n    {\n\n    }\n\n    /// <summary>\n    /// #SCRIPTNAME#\n    /// </summary>\n    public class #SCRIPTNAME# : IActionHandler<IActionController, Single>\n    {\n        //public class Data\n        //{\n        //}\n\n        public void Enter(ActionNode<IActionController, Single> node)\n        {\n            //#SCRIPTNAME#Config config = (#SCRIPTNAME#Config)node.config;\n            //IActionMachine<IActionController, Single> machine = node.actionMachine;\n            //IActionController controller = node.actionMachine.controller;\n            //node.data = new Data();\n        }\n\n        public void Exit(ActionNode<IActionController, Single> node)\n        {\n            //#SCRIPTNAME#Config config = (#SCRIPTNAME#Config)node.config;\n            //IActionMachine<IActionController, Single> machine = node.actionMachine;\n            //IActionController controller = node.actionMachine.controller;\n            //Data data = (Data)node.data;\n        }\n\n        public void Update(ActionNode<IActionController, Single> node, Single deltaTime)\n        {\n            //#SCRIPTNAME#Config config = (#SCRIPTNAME#Config)node.config;\n            //IActionMachine<IActionController, Single> machine = node.actionMachine;\n            //IActionController controller = node.actionMachine.controller;\n            //Data data = (Data)node.data;\n        }\n    }\n}\n";
    }
}