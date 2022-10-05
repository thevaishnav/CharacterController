using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using KSRecs.Utils;
using UnityEditor;
#endif

namespace KSRecs.Utils
{
    [Serializable]
    public class Probabilities<TObject> where TObject : Object
    {
        [SerializeField] private List<TObject> objects;
        [SerializeField] private List<float> probs;

        public float this[TObject key]
        {
            get
            {
                if (objects.Contains(key)) return probs[objects.IndexOf(key)];
                return 0;
            }
        }
    }
}

#if UNITY_EDITOR
namespace KSRecs.Editor
{
    [CustomPropertyDrawer(typeof(Probabilities<>))]
    public class ProbabilitiesDrawer : PropertyDrawer
    {
        SerializedProperty objects;
        SerializedProperty probs;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (objects == null) objects = property.FindPropertyRelative("objects");
            if (probs == null) probs = property.FindPropertyRelative("probs");

            float single = EditorGUI.GetPropertyHeight(probs.GetArrayElementAtIndex(0));
            if (property.isExpanded) return single * objects.arraySize;
            return single;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label);

            if (!property.isExpanded) return;

            int probsSize = probs.arraySize;
            int spawsSize = objects.arraySize;

            if (spawsSize == 0)
            {
                probs.arraySize = spawsSize;
                return;
            }

            if (probsSize != spawsSize)
            {
                probs.arraySize = spawsSize;
                if (probsSize > spawsSize)
                {
                    float newLimit = 0;
                    for (int i = 0; i < spawsSize; i++)
                    {
                        newLimit += probs.GetArrayElementAtIndex(i).floatValue;
                    }


                    float fraction = newLimit / 100;
                    for (int i = 0; i < probs.arraySize; i++)
                    {
                        probs.GetArrayElementAtIndex(i).floatValue *= fraction;
                    }

                    Debug.Log("Updated Probs by SizeChange");
                }
            }

            float newMax = 100;
            for (int i = 0; i < probs.arraySize; i++)
            {
                SerializedProperty objec = objects.GetArrayElementAtIndex(i);
                if (objec.objectReferenceValue == null) continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(objec);
                float value = EditorGUILayout.Slider($"{objec.objectReferenceValue.name} [0-{newMax}]", probs.GetArrayElementAtIndex(i).floatValue, 0, newMax);
                EditorGUILayout.EndHorizontal();
                probs.GetArrayElementAtIndex(i).floatValue = value;
                newMax -= value;
            }

            if (newMax == 100)
            {
                probs.GetArrayElementAtIndex(0).floatValue = newMax;
            }
            else
            {
                probs.GetArrayElementAtIndex(probs.arraySize - 1).floatValue = newMax;
            }


            EditorGUI.EndProperty();
        }
    }
}
#endif