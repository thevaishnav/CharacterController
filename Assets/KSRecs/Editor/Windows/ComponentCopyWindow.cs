using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using KSRecs.Editor.Extensions;
using KSRecs.Extensions;
using Component = UnityEngine.Component;

namespace KSRecs.Editor
{
    public class ComponentCopyWindow : EditorWindow
    {
        static GameObject selectedObject;

        private Dictionary<string, bool> _components;
        private Vector2 componentsScrollPos;
        private Vector2 targetsScrollPos;
        private bool copyNonSerialized;
        List<GameObject> targets;
        List<string> changed;

        public static void Init()
        {
            if (Selection.activeGameObject != null)
            {
                selectedObject = Selection.activeGameObject;
                EditorWindow.GetWindow<ComponentCopyWindow>();
            }
        }

        private void OnEnable()
        {
            changed = new List<string>();
            targets = new List<GameObject>();

            UpdateList();
        }

        private void OnDestroy()
        {
            selectedObject = null;
        }

        private void OnGUI()
        {
            copyNonSerialized = EditorGUILayout.ToggleLeft("Copy NonSerialized Fields As Well", copyNonSerialized);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                DrawComponents();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                DrawTargets();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            GUI.enabled = targets.Count >= 1;
            if (GUILayout.Button("Copy Selected")) CopySelected();
            GUI.enabled = true;
        }

        private void DrawComponents()
        {
            changed = new List<string>();
            componentsScrollPos = GUILayout.BeginScrollView(componentsScrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            foreach (KeyValuePair<string, bool> pair in _components)
            {
                bool newVal = EditorGUILayout.ToggleLeft(pair.Key, pair.Value);
                if (newVal != pair.Value)
                {
                    changed.Add(pair.Key);
                }
            }

            GUILayout.EndScrollView();

            foreach (string component in changed)
            {
                _components[component] = !_components[component];
            }

            changed.Clear();
        }

        private void DrawTargets()
        {
            EditorGUILayout.LabelField("Target Objects:");
            targetsScrollPos = GUILayout.BeginScrollView(targetsScrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            {
                int index = 0;
                int deleteAt = -1;
                foreach (GameObject go in targets)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{index:00}. {go.name}");
                    if (GUILayout.Button("X"))
                    {
                        deleteAt = index;
                        break;
                    }

                    index++;
                    EditorGUILayout.EndHorizontal();
                }

                if (deleteAt >= 0)
                {
                    targets.RemoveAt(deleteAt);
                }
            }
            GUILayout.EndScrollView();

            GameObject toAdd = (GameObject) EditorGUILayout.ObjectField(null, typeof(GameObject), true);
            if (toAdd != null)
            {
                targets.Add(toAdd.gameObject);
            }
        }

        private void CopySelected()
        {
            if (!_components.Any(pair => pair.Value))
            {
                Debug.LogError("Select at least 1 component");
                return;
            }
            
            StringBuilder info = new StringBuilder();
            info.Append($"Copied components from: {selectedObject.name}");
            foreach (GameObject target in targets)
            {
                info.AppendLine($"    Copied to {target.name}:");
                foreach (Component component in selectedObject.GetComponents<Component>())
                {
                    string cName = component.ClassName();
                    if (_components.ContainsKey(cName) && _components[cName])
                    {
                        info.AppendLine($"        {component.CopyComponentTo(target, copyNonSerialized)}");
                    }
                }
                EditorUtility.SetDirty(target);
            }

            Debug.Log(info.ToString());
        }


        private void UpdateList()
        {
            List<string> comps = new List<string>();
            foreach (Component component in selectedObject.GetComponents<Component>())
            {
                comps.Add(component.ClassName());
            }

            comps.Sort();

            _components = new Dictionary<string, bool>();
            foreach (string s in comps)
            {
                _components.Add(s, false);
            }
        }
    }
}