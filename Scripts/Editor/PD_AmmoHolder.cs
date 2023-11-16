using Omnix.CCN.Items;
using Omnix.Editor;
using UnityEditor;
using UnityEngine;

namespace Omnix.CCN.EditorSpace
{
    [CustomPropertyDrawer(typeof(AmmoInfo))]
    public class PD_AmmoHolder : PropertyDrawer
    {
        private static GUIContent CONTENT_ALLOW = new GUIContent("Allow Reload");
        private static GUIContent CONTENT_BEFORE = new GUIContent("Before Shot");
        private static GUIContent CONTENT_AFTER = new GUIContent("After Shot");
        private static GUIContent CONTENT_FULL_MAG = new GUIContent("If Mag Is Full");
        
        private static float HeightOf(float lineCount) => EditorGUIUtility.singleLineHeight * lineCount;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded == false) return HeightOf(1f);;
            return HeightOf(6f);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.DrawRect(position, new Color(0f, 1f, 0.85f, 0.11f));
            position.x += 20f;
            position.width -= 30f;
            
            var positioner = new OmnixLayout(position, 1f);
            
            property.isExpanded = EditorGUI.Foldout(positioner, property.isExpanded, label);
            if (property.isExpanded == false) return;
            DrawDirectControl(property, positioner);
        }

        private void DrawDirectControl(SerializedProperty property, OmnixLayout positioner)
        {
            EditorGUI.PropertyField(positioner, property.FindPropertyRelative("ReloadType"));
            EditorGUI.PropertyField(positioner, property.FindPropertyRelative("MagSize"));
            EditorGUI.PropertyField(positioner, property.FindPropertyRelative("CurrentMagCount"));
            EditorGUI.PropertyField(positioner, property.FindPropertyRelative("CurrentAmmo"));

            {
                SerializedProperty beforeShot = property.FindPropertyRelative("AllowReloadBeforeShot");
                SerializedProperty afterShot = property.FindPropertyRelative("AllowReloadAfterShot");
                SerializedProperty ifMagIsFull = property.FindPropertyRelative("AllowReloadIfMagIsFull");

                positioner.BeginHorizontal(3, CONTENT_ALLOW);
                beforeShot.boolValue = EditorGUI.ToggleLeft(positioner, CONTENT_BEFORE, beforeShot.boolValue);
                afterShot.boolValue = EditorGUI.ToggleLeft(positioner, CONTENT_AFTER, afterShot.boolValue);
                ifMagIsFull.boolValue = EditorGUI.ToggleLeft(positioner, CONTENT_FULL_MAG, ifMagIsFull.boolValue);
            }
        }
    }
}