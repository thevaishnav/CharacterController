using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

#if UNITY_EDITOR
using KSRecs.Inputs;
using UnityEditor;
#endif



namespace KSRecs.Inputs
{
    [Serializable]
    public class AdvancedInput /*: List<UnitInput> */
    {
        [SerializeField] private List<UnitInput> combinations;
        
        public bool IsPressed()
        {
            foreach (UnitInput comb in combinations)
            {
                if (comb.IsPressed()) return true;
            }

            return false;
        }

        public bool GetPressedCombo(out UnitInput combo)
        {
            foreach (UnitInput comb in combinations)
            {
                if (comb.IsPressed())
                {
                    combo = comb;
                    return true;
                }
            }

            combo = null;
            return false;
        }
        
        public IEnumerator<UnitInput> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (UnitInput comb in combinations)
            {
                builder.Append($"{comb} || ");
            }
            return builder.ToString();
        }
    }
}


#if UNITY_EDITOR
namespace KSRecs.Editor
{
    [CustomPropertyDrawer(typeof(AdvancedInput))]
    public class AdvancedInputDrawer : PropertyDrawer
    {
        private SerializedProperty combinations;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (combinations == null)
            {
                combinations = property.FindPropertyRelative("combinations");
            }

            return EditorGUI.GetPropertyHeight(combinations);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            // EditorGUI.PropertyField(position, combinations, label);
            EditorGUI.PropertyField(position, combinations, label, true);
            EditorGUI.EndProperty();   
        }
    }
}
#endif
