using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Yunst.ProjectStarter.Editor
{
    public class SceneUtilities : EditorWindow
    {
        [MenuItem("Window/Scene Utilities")]
        public static void ShowWindow()
        {
            GetWindow<SceneUtilities>("Scene Utilities");
        }

        private Vector2 scrollPos;

        private void OnGUI()
        {
            GUILayout.Label("This is a custom editor window named Scene Utilities", EditorStyles.boldLabel);

            GUILayout.Space(10);

            var scenes = EditorBuildSettings.scenes;

            // --- Scene open/enable/disable controls ---
            GUILayout.Space(10);
            GUILayout.Label("Scene Controls:", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Scene Name");
            GUILayout.Label("Open Additive", GUILayout.Width(110));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            for (int i = 0; i < scenes.Length; i++)
            {
                var scene = scenes[i];
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);

                EditorGUILayout.BeginHorizontal();

                using (new EditorGUI.DisabledScope(!scene.enabled))
                {
                    // Button to open scene (single)
                    if (GUILayout.Button(sceneName))
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(scene.path);
                        }
                    }
                    // Button to open scene additively
                    if (GUILayout.Button("Open Additive", GUILayout.Width(110)))
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Add Current Scene to Build Settings"))
            {
                SceneUtilitiesHelper.AddCurrentSceneToBuildSettings();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Remove Current Scene from Build Settings"))
            {
                SceneUtilitiesHelper.RemoveCurrentSceneFromBuildSettings();
            }
        }
    }

    public static class SceneUtilitiesHelper
    {
        public static void AddCurrentSceneToBuildSettings()
        {
            var currentScene = EditorSceneManager.GetActiveScene();
            if (currentScene.IsValid() && !IsSceneInBuildSettings(currentScene))
            {
                AddSceneToBuildSettings(currentScene);
            }
        }

        public static void RemoveCurrentSceneFromBuildSettings()
        {
            var currentScene = EditorSceneManager.GetActiveScene();
            if (currentScene.IsValid() && IsSceneInBuildSettings(currentScene))
            {
                RemoveSceneFromBuildSettings(currentScene);
            }
        }

        private static bool IsSceneInBuildSettings(Scene scene)
        {
            var scenes = EditorBuildSettings.scenes;
            foreach (var s in scenes)
            {
                if (s.path == scene.path)
                {
                    return true;
                }
            }
            return false;
        }

        private static void AddSceneToBuildSettings(Scene scene)
        {
            var scenePath = scene.path;
            var newScene = new EditorBuildSettingsScene(scenePath, true);
            var scenes = EditorBuildSettings.scenes.ToList();
            scenes.Add(newScene);
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static void RemoveSceneFromBuildSettings(Scene scene)
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            scenes.RemoveAll(s => s.path == scene.path);
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
