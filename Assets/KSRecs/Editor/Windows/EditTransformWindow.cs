using UnityEngine;
using UnityEditor;


namespace KSRecs.Editor
{
    public class EditTransformWindow : EditorWindow
    {
        private GameObject[] selection;
        private bool _editPosition = false;
        private bool _editRot = false;
        private bool _editScale = false;
        private Vector3 _positionOffset;
        private Vector3 _rotationOffset;
        private Vector3 _scaleOffset;

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
            _editPosition = EditorGUILayout.ToggleLeft("Position", _editPosition);
            _editRot = EditorGUILayout.ToggleLeft("Rotation", _editRot);
            _editScale = EditorGUILayout.ToggleLeft("Scale", _editScale);

            GUI.enabled = _editPosition;
            _positionOffset = EditorGUILayout.Vector3Field("Position", _positionOffset);
            GUI.enabled = _editRot;
            _rotationOffset = EditorGUILayout.Vector3Field("Rotation", _rotationOffset);
            GUI.enabled = _editScale;
            _scaleOffset = EditorGUILayout.Vector3Field("Scale", _scaleOffset);
            GUI.enabled = true;

            if (GUILayout.Button("Apply")) OffsetSelection();
            if (GUILayout.Button("Cancel")) this.Close();
        }

        private void OffsetSelection()
        {
            foreach (GameObject gameObject in selection)
            {
                if (_editPosition) gameObject.transform.position += _positionOffset;
                if (_editRot) gameObject.transform.Rotate(_rotationOffset);
                if (_editScale) gameObject.transform.localScale += _scaleOffset;
            }

            this.Close();
        }
    }
}