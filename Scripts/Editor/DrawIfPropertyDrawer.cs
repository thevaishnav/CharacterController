using KS.CharaCon.Utils;
using UnityEditor;
using UnityEngine;

namespace KS.CharaCon.Editor
{
    [CustomPropertyDrawer(typeof(DrawIfEnumEqualAttribute))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {
        private DrawIfEnumEqualAttribute drawIf;
        private SerializedProperty compaired;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (drawIf == null) drawIf = (DrawIfEnumEqualAttribute)attribute;
            if (compaired == null) compaired = property.serializedObject.FindProperty(drawIf.enumName);

            if (drawIf.CompairTo(compaired.enumValueIndex))
                return base.GetPropertyHeight(property, label);
            return 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (drawIf.CompairTo(compaired.enumValueIndex))
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
