using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using KSRecs.Editor.Extensions;
using KSRecs.UnityObjectExtensions;
using Component = UnityEngine.Component;

namespace KSRecs.Editor
{
    public class EditTransformWindow : EditorWindow
    {
        private GameObject[] selection;
        private bool position = false;
        private bool rotation = false;
        private bool scale = false;
        private Vector3 positionOffset;
        private Vector3 rotationOffset;
        private Vector3 scaleOffset;

        public static void Init()
        {
            
            if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
            {
                EditTransformWindow window = GetWindow<EditTransformWindow>(utility: true, focus: true, title: "Edit Transform Window");
                if (window.selection == null)
                {
                    window.ShowModal();
                }
            }
        }

        private void OnEnable()
        {
            selection = Selection.gameObjects;
        }
        
        private void OnDestroy()
        {
            selection = null;
        }

        private void OnGUI()
        {
            position = EditorGUILayout.ToggleLeft("Position", position);
            rotation = EditorGUILayout.ToggleLeft("Rotation", rotation);
            scale = EditorGUILayout.ToggleLeft("Scale", scale);

            GUI.enabled = position;
            positionOffset = EditorGUILayout.Vector3Field("Position", positionOffset);
            GUI.enabled = rotation;
            rotationOffset = EditorGUILayout.Vector3Field("Rotation", rotationOffset);
            GUI.enabled = scale;
            scaleOffset = EditorGUILayout.Vector3Field("Scale", scaleOffset);
            GUI.enabled = true;

            if (GUILayout.Button("Apply")) OffsetSelection();
            if (GUILayout.Button("Cancel")) this.Close();
        }

        private void OffsetSelection()
        {
            foreach (GameObject gameObject in selection)
            {
                if (position) gameObject.transform.position += positionOffset;
                if (rotation) gameObject.transform.Rotate(rotationOffset);
                if (scale) gameObject.transform.localScale += scaleOffset;
            }

            this.Close();
        }
    }
}