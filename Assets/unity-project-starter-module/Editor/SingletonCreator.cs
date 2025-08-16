using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace Yunst.ProjectStarter.Editor
{
    public class SingletonCreator
    {
        [MenuItem("Assets/Create/Scripting/Singleton/Singleton Script", false, 10)]
        public static void CreateSingleton()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";

            if (!AssetDatabase.IsValidFolder(path))
                path = System.IO.Path.GetDirectoryName(path);


            string defaultName = "NewSingleton";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<SingletonScriptAssetAction>(),
                System.IO.Path.Combine(path, defaultName + ".cs"),
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                null
            );
        }

        // Helper class for script creation
        internal class SingletonScriptAssetAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string classBaseName = System.IO.Path.GetFileNameWithoutExtension(pathName);

                string scriptContent =
$@"using UnityEngine;
using Yunst.ProjectStarter.Base;

namespace Yunst.ProjectStarter
{{
    public class {classBaseName} : Singleton<{classBaseName}>
    {{
        // Your singleton logic here

        // This Awake method is added to prevent overwriting the base awake logic unintentionally
        protected override void Awake()
        {{
            base.Awake();
            // Your awake logic here 
        }}
    }}
}}
";
                System.IO.File.WriteAllText(pathName, scriptContent);
                AssetDatabase.ImportAsset(pathName);
                Object asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
                if (asset != null)
                {
                    ProjectWindowUtil.ShowCreatedAsset(asset);
                }
            }
        }

        [MenuItem("Assets/Create/Scripting/Singleton/Persistent Singleton Script", false, 11)]
        public static void CreatePersistentSingleton()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";

            if (!AssetDatabase.IsValidFolder(path))
                path = System.IO.Path.GetDirectoryName(path);

            string defaultName = "NewPersistentSingleton";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<PersistentSingletonScriptAssetAction>(),
                System.IO.Path.Combine(path, defaultName + ".cs"),
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                null
            );
        }

        // Helper class for script creation
        internal class PersistentSingletonScriptAssetAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string classBaseName = System.IO.Path.GetFileNameWithoutExtension(pathName);

                string scriptContent =
$@"using UnityEngine;
using Yunst.ProjectStarter.Base;

namespace Yunst.ProjectStarter
{{
    public class {classBaseName} : PersistentSingleton<{classBaseName}>
    {{
        // Your singleton logic here

        // This Awake method is added to prevent overwriting the base awake logic unintentionally
        protected override void Awake()
        {{
            base.Awake();
            // Your awake logic here 
        }}
    }}
}}
";
                System.IO.File.WriteAllText(pathName, scriptContent);
                AssetDatabase.ImportAsset(pathName);
                Object asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
                if (asset != null)
                {
                    ProjectWindowUtil.ShowCreatedAsset(asset);
                }
            }
        }

    }
}