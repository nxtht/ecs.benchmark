using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchmark
{
    public class BenchmarkSelector : MonoBehaviour
    {
        private const float TextPhysicalSize = 0.3f;
        private const float SpacePhysicalSize = 0.08f;
        private const string SceneSuffix = "BenchmarkSetupScene";
    
        private List<SceneData> _sceneData;

        private void Start()
        {
            _sceneData = new List<SceneData>();
            var count = SceneManager.sceneCountInBuildSettings;
            for (var i = 0; i < count; i++)
            {
                var scName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (scName.EndsWith(SceneSuffix))
                {
                    _sceneData.Add(
                        new SceneData
                        {
                            SceneName = scName,
                            SceneLabel = scName.Substring(0, scName.Length - SceneSuffix.Length)
                        });
                }
            }
        }

        private void OnGUI()
        {
            var fontSize = (int) (Screen.dpi * TextPhysicalSize);
            var spaceSize = (int) (Screen.dpi * SpacePhysicalSize);
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.FlexibleSpace();
        
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUI.skin.button.fontSize = fontSize;
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = fontSize;
            GUILayout.Label($"SELECT BENCHMARK");
            GUILayout.Space(spaceSize);
            foreach (var scene in _sceneData)
            {
                if (GUILayout.Button(scene.SceneLabel))
                {
                    SceneManager.LoadScene(scene.SceneName);
                }

                GUILayout.Space(spaceSize);
            }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }

        private class SceneData
        {
            public string SceneName;
            public string SceneLabel;
        }
    }
}