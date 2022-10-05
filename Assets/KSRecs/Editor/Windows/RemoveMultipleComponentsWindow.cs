using System.Collections.Generic;
using KSRecs.Editor.Extensions;
using KSRecs.UnityObjectExtensions;
using UnityEditor;
using UnityEngine;

namespace KSRecs.Editor
{
    public class RemoveMultipleComponentsWindow : EditorWindow
    {
        private Dictionary<string, bool> _components;
        private GameObject selectedObject;
        private Vector2 scrollPos;

        public static void Init()
        {
            EditorWindow.GetWindow<RemoveMultipleComponentsWindow>();
        }
        
        public void OnEnable()
        {
            selectedObject = Selection.activeGameObject;
            UpdateList();
        }

        private void UpdateList()
        {
            List<string> comps = new List<string>();
            foreach (Component component in selectedObject.GetComponents<Component>())
            {
                if (component is Transform) continue;
                
                comps.Add(component.ClassName());
            }
            comps.Sort();
            
            _components = new Dictionary<string, bool>();
            foreach (string s in comps)
            {
                _components.Add(s, false);
            }            
        }

        private void OnGUI()
        {
            List<string> changed = new List<string>();
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
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

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove Selected")) KSEditorUtils.RemoveComponents(selectedObject, _components, true);
            if (GUILayout.Button("Keep Selected")) KSEditorUtils.RemoveComponents(selectedObject, _components, false);
            EditorGUILayout.EndHorizontal();
            changed.Clear();
        }

    }
}