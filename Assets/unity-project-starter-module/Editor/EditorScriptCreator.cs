using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
namespace Yunst.ProjectStarter.Editor
{
    public class EditorScriptCreator
    {
        [MenuItem("Assets/Create/Scripting/Editor/Editor Window Script", false, 11)]
        public static void CreateEditorScript()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";

            if (!AssetDatabase.IsValidFolder(path))
                path = System.IO.Path.GetDirectoryName(path);

            string defaultName = "NewEditorWindowScript";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            ScriptableObject.CreateInstance<EditorWindowScriptAssetAction>(),
            System.IO.Path.Combine(path, defaultName + ".cs"),
            EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
            null
            );
        }

        
        internal class EditorWindowScriptAssetAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string classBaseName = System.IO.Path.GetFileNameWithoutExtension(pathName);

                string scriptContent =
$@"using UnityEngine;
using UnityEditor;
    
    namespace Yunst.ProjectStarter.Editor
    {{
        public class {classBaseName} : EditorWindow
        {{
            [MenuItem(""Window/{classBaseName}"")]
            public static void ShowWindow()
            {{
                GetWindow<{classBaseName}>(""{classBaseName}"");
            }}
    
            private void OnGUI()
            {{
                GUILayout.Label(""This is a custom editor window named {classBaseName}"", EditorStyles.boldLabel);
                // Add your custom GUI elements here
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

        [MenuItem("Assets/Create/Scripting/Editor/Build Processor Script", false, 12)]
        public static void CreateBuildProcessorScript()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";

            if (!AssetDatabase.IsValidFolder(path))
                path = System.IO.Path.GetDirectoryName(path);

            string defaultName = "NewBuildProcessorScript";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<BuildProcessorScriptAssetAction>(),
                System.IO.Path.Combine(path, defaultName + ".cs"),
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                null
            );
        }

        internal class BuildProcessorScriptAssetAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string classBaseName = System.IO.Path.GetFileNameWithoutExtension(pathName);

                string scriptContent =
$@"using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

    namespace Yunst.ProjectStarter.Editor
    {{
        public class {classBaseName} : IPreprocessBuildWithReport, IPostprocessBuildWithReport
        {{
            public int callbackOrder => 0;

            public void OnPreprocessBuild(BuildReport report)
            {{
                Debug.Log(""Pre-build process started for target: "" + report.summary.platform + "", path: "" + report.summary.outputPath);
                // Add your pre-build logic here
            }}

            public void OnPostprocessBuild(BuildReport report)
            {{
                Debug.Log(""Post-build process completed for target: "" + report.summary.platform + "", path: "" + report.summary.outputPath);
                // Add your post-build logic here
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
