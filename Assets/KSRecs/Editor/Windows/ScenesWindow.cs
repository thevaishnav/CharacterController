using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

namespace KSRecs.Editor
{
    class ScenesWindow : EditorWindow
    {
        private static string _dataPathGuids => Application.persistentDataPath + "\\Scenes.dat";

        static GUILayoutOption miniButtonWidth => GUILayout.Width(25);
        private Dictionary<string, string> buildScenes = new Dictionary<string, string>();
        private Dictionary<string, string> addedScenes = new Dictionary<string, string>();
        private Vector2 scrollPos;
        private static GUIStyle _buttonStyleInactive;
        private static GUIStyle _buttonStyleActive;

        [MenuItem("KS/Window/Scenes", priority = 2)]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            EditorWindow.GetWindow(typeof(ScenesWindow), false, "Scene").Show();
        }

        private void OnEnable()
        {
            ReloadScenesList();
            LoadData();
            UpdateButtonStyles();
        }

        void UpdateButtonStyles()
        {
            if (_buttonStyleInactive == null)
            {
                _buttonStyleInactive = new GUIStyle(EditorStyles.toolbarButton);
                _buttonStyleInactive.alignment = TextAnchor.MiddleLeft;
            }
            if (_buttonStyleActive == null)
            {
                _buttonStyleActive = new GUIStyle(EditorStyles.toolbarButton);
                _buttonStyleActive.alignment = TextAnchor.MiddleLeft;
                _buttonStyleActive.normal.textColor = Color.green;
            }
        }
        
        static bool DrawSceneButton(KeyValuePair<string, string> scene, string active, bool isGameNotPlaying, bool isAdded, GUILayoutOption megaWidth)
        {
            if (scene.Value == active)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("@", _buttonStyleInactive, miniButtonWidth))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scene.Value));
                }

                GUILayout.Label(scene.Key, _buttonStyleActive, megaWidth);
                if (isGameNotPlaying && GUILayout.Button("▶", _buttonStyleInactive, miniButtonWidth)) EditorApplication.isPlaying = true;
                if (isAdded && GUILayout.Button("X", _buttonStyleInactive, miniButtonWidth))
                {
                    return true;
                }
                GUILayout.EndHorizontal();
                return false;
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("@", _buttonStyleInactive, miniButtonWidth)) EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scene.Value));

            if (GUILayout.Button(scene.Key, _buttonStyleInactive, megaWidth))
            {
                if (isGameNotPlaying)
                {
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(scene.Value);
                }
                else
                {
                    SceneManager.LoadScene(AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.Value).name);
                }
            }

            if (isGameNotPlaying && GUILayout.Button("▶", _buttonStyleInactive, miniButtonWidth))
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                EditorSceneManager.OpenScene(scene.Value);
                EditorApplication.isPlaying = true;
            }

            if (isAdded && GUILayout.Button("X", _buttonStyleInactive, miniButtonWidth))
            {
                return true;
            }

            GUILayout.EndHorizontal();
            return false;
        }

        static string NameFromPath(string path)
        {
            string theName = path.Substring(path.LastIndexOf('/') + 1);
            theName = theName.Substring(0, theName.Length - 6);
            return theName;
        }

        public void SaveData()
        {
            string[] toSave = new string[this.addedScenes.Count];
            try
            {
                int count = 0;
                foreach (KeyValuePair<string, string> pair in addedScenes)
                {
                    toSave[count] = AssetDatabase.GUIDFromAssetPath(pair.Value).ToString();
                    count++;
                }


                FileInfo fileInfo = new FileInfo(_dataPathGuids);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                if (!fileInfo.Exists)
                {
                    fileInfo.Create();
                }

                File.WriteAllText(fileInfo.FullName, JsonConvert.SerializeObject(toSave));
            }
            catch
            {
                Debug.LogError("Failed to save data");
            }
        }

        public void LoadData()
        {
            addedScenes = new Dictionary<string, string>();
            if (!File.Exists(_dataPathGuids)) return;

            try
            {
                string[] allGuids = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(_dataPathGuids));
                foreach (string guid in allGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(new GUID(guid));
                    addedScenes.Add(NameFromPath(path), path);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to Load Data {e.Message}");
            }
        }

        public void OnGUI()
        {
            if (GUILayout.Button("Refresh List"))
            {
                ReloadScenesList();
                return;
            }
            UpdateButtonStyles(); 
            
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(position.width));
            string active = EditorSceneManager.GetActiveScene().path;
            bool isGameNotPlaying = !Application.isPlaying;

            GUILayoutOption megaWidth;
            if (isGameNotPlaying) megaWidth = GUILayout.Width(this.position.width - 78);
            else megaWidth = GUILayout.Width(this.position.width - 53);

            if (buildScenes.Count != 0)
            {
                EditorGUILayout.LabelField("Build Scenes");
                foreach (KeyValuePair<string, string> scene in buildScenes)
                {
                    DrawSceneButton(scene, active, isGameNotPlaying, false, megaWidth);
                }
            }

            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("Added Scenes");
            SceneAsset asset = (SceneAsset)EditorGUILayout.ObjectField(null, typeof(SceneAsset), false, GUILayout.Width(position.width - 20));
            if (asset != null)
            {
                string path = AssetDatabase.GetAssetPath(asset);
                addedScenes.Add(NameFromPath(path), path);
                SaveData();
            }

            if (addedScenes.Count == 0)
            {
                GUILayout.EndScrollView();
                return;
            }

            if (isGameNotPlaying) megaWidth = GUILayout.Width(this.position.width - 103);
            else megaWidth = GUILayout.Width(this.position.width - 53);
            foreach (KeyValuePair<string, string> scene in addedScenes)
            {
                bool shouldDelete = DrawSceneButton(scene, active, isGameNotPlaying, true, megaWidth);
                if (shouldDelete)
                {
                    addedScenes.Remove(scene.Key);
                    SaveData();
                    break;
                }
            }
            GUILayout.EndScrollView();
        }

        public void ReloadScenesList()
        {
            buildScenes.Clear();
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
            {
                if (S.enabled && S.path != "")
                {
                    buildScenes.Add(NameFromPath(S.path), S.path);
                }
            }
        }
    }
}