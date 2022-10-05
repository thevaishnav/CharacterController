using System;
using KSRecs.Editor.Extensions;
using UnityEngine;
#if UNITY_EDITOR
using KSRecs.Inputs;
using UnityEditor;
#endif

namespace KSRecs.Inputs
{
    [Serializable]
    public class UnitInput : ISerializationCallbackReceiver
    {
        [SerializeField] KeysModifier modifier;
        [SerializeField] KeyCode key;
        [SerializeField] CheckType keyCheckType;

        public KeysModifier Modifier => modifier;
        public KeyCode Key => key;
        public CheckType KeyCheckType => keyCheckType;
        
        private Func<bool> _isPressedModifier;
        private Func<KeyCode, bool> _isPressedKey;

        public void Init()
        {
            switch (modifier)
            {
                case (KeysModifier.None):
                {
                    _isPressedModifier = ModifierChecker.PressedNone;
                    break;
                }
                case (KeysModifier.Control):
                {
                    _isPressedModifier = ModifierChecker.PressedControl;
                    break;
                }
                case (KeysModifier.Alt):
                {
                    _isPressedModifier = ModifierChecker.PressedAlt;
                    break;
                }
                case (KeysModifier.Shift):
                {
                    _isPressedModifier = ModifierChecker.PressedShift;
                    break;
                }
                case (KeysModifier.ControlShift):
                {
                    _isPressedModifier = ModifierChecker.PressedControlShift;
                    break;
                }
                case (KeysModifier.AltShift):
                {
                    _isPressedModifier = ModifierChecker.PressedAltShift;
                    break;
                }
                case (KeysModifier.ControlAlt):
                {
                    _isPressedModifier = ModifierChecker.PressedControlAlt;
                    break;
                }
                case (KeysModifier.ControlAltShift):
                {
                    _isPressedModifier = ModifierChecker.PressedControlAltShift;
                    break;
                }
                default:
                {
                    _isPressedModifier = ModifierChecker.PressedNone;
                    break;
                }
            }

            if (keyCheckType == CheckType.Down) _isPressedKey = Input.GetKeyDown;
            if (keyCheckType == CheckType.Hold) _isPressedKey = Input.GetKey;
            if (keyCheckType == CheckType.Release) _isPressedKey = Input.GetKeyUp;
        }

        public bool IsPressed()
        {
            #if UNITY_EDITOR
            {
                Init();
            }
            #endif
            return _isPressedModifier() && _isPressedKey(key);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Init();
        }
        
        public override string ToString()
        {
            return $"{modifier}+{key} ({keyCheckType})";
        }
    }
}


#if UNITY_EDITOR
namespace KSRecs.Editor
{
    [CustomPropertyDrawer(typeof(UnitInput))]
    public class UnitInputDrawer : PropertyDrawer
    {
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            float gap = position.width / 3f;
            Rect rect = new Rect(position.x, position.y, (position.width/3f) - 3f, position.height);
            EditorGUI.PropertyField(rect, property.FindPropertyChild("modifier"), GUIContent.none);
            rect.x += gap;
            EditorGUI.PropertyField(rect, property.FindPropertyChild("key"), GUIContent.none);
            rect.x += gap;
            EditorGUI.PropertyField(rect, property.FindPropertyChild("keyCheckType"), GUIContent.none);
            EditorGUI.EndProperty();   
        }
    }
}
#endif