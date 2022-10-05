using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KSRecs.Editor
{
    public static class KSMenu
    {
        [MenuItem("KS/Clear Player Prefs", false, 1000)]
        public static void ClearPlayerPrefs() => PlayerPrefs.DeleteAll();

        [MenuItem("KS/Clear Selections", false, 1001)]
        public static void ClearSelection()
        {
            Selection.activeGameObject = null;
            Selection.activeObject = null;
            Selection.activeTransform = null;
        }
        
        [MenuItem("KS/Main Camera to Scene View", false, 1002)]
        public static void MainCameraToSceneView()
        {
            Transform sceneCam = EditorWindow.GetWindow<SceneView>().camera.transform;
            Transform gameCam = Camera.main.transform;
            gameCam.position = sceneCam.position;
            gameCam.rotation = sceneCam.rotation;
            gameCam.localScale = sceneCam.localScale;
        }
        
    }
}