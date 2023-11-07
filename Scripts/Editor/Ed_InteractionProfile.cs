using CCN.InputSystemWrapper;
using UnityEditor;
using UnityEngine;

namespace CCN.EditorSpace
{
    [CustomEditor(typeof(InteractionProfileBase), true)]
    public class Ed_InteractionProfile : Editor
    {
        private static readonly GUIContent SingleTargetInfo = new GUIContent("This type of profile will only be able to command one target at a time.\n This takes less processing power than Multi Target Profile.\n The target can be an ability, item or agent.");
        private static readonly GUIContent MultiTargetInfo = new GUIContent("This type of profile can command one or more target at a time.\n This takes more processing power than Single Target Profile.\n The target can an ability, item or agent.");
        private static readonly GUIContent ContentTryStartInAwake = new GUIContent("Try Start In Awake");
        private static readonly GUIContent ContentPCMode = new GUIContent("Mode");
        private static readonly GUIContent ContentPCInputType = new GUIContent("Input Type");
        private static readonly GUIContent ContentPCButtonName = new GUIContent("Button Name", "Name of Button for Input.GetButton method");
        private static readonly GUIContent ContentPCAxisNameHorizontal = new GUIContent("Horizontal Axis Name", "Name of the Axis for Input.GetAxis method");
        private static readonly GUIContent ContentPCAxisNameVertical = new GUIContent("Vertical Axis Name", "Name of the Axis for Input.GetAxis method");
        private static readonly GUIContent ContentPCAxisThreshold = new GUIContent("Tolerance", "Max deviation (from 0) that can be ignored");
        private static readonly GUIContent ContentPCKeycode = new GUIContent("Keycode", "Keycode for Input.GetKey method");
        private static readonly GUIContent ContentUIMode = new GUIContent("Mode");
        private static readonly GUIContent ContentUIAxisIntensity = new GUIContent("Intensity", "Max value for axis");
        private static readonly GUIContent ContentUIAxisThreshold = new GUIContent("Tolerance", "Max deviation (from 0) that can be ignored");


        private SerializedProperty _tryStartInAwake;
        private SerializedProperty _pcMode;
        private SerializedProperty _pcInputType;
        private SerializedProperty _pcTriggerName;
        private SerializedProperty _pcTriggerName2;
        private SerializedProperty _pcAxisThreshold;
        private SerializedProperty _pcKeycode;
        private SerializedProperty _uiMode;
        private SerializedProperty _uiAxisIntensity;
        private SerializedProperty _uiAxisThreshold;
        private GUIStyle _headerStyle;
        private GUIStyle _infoStyle;


        private void OnEnable()
        {
            _tryStartInAwake = serializedObject.FindProperty("tryStartInAwake");
            _pcMode = serializedObject.FindProperty("pcMode");
            _pcInputType = serializedObject.FindProperty("pcInputType");
            _pcTriggerName = serializedObject.FindProperty("pcTriggerName");
            _pcTriggerName2 = serializedObject.FindProperty("pcTriggerName2");
            _pcAxisThreshold = serializedObject.FindProperty("pcAxisThreshold");
            _pcKeycode = serializedObject.FindProperty("pcKeycode");
            _uiMode = serializedObject.FindProperty("uiMode");
            _uiAxisIntensity = serializedObject.FindProperty("uiAxisIntensity");
            _uiAxisThreshold = serializedObject.FindProperty("uiAxisThreshold");
        }

        public override void OnInspectorGUI()
        {
            if (_headerStyle == null)
            {
                _headerStyle = new GUIStyle(EditorStyles.largeLabel);
                _headerStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (_infoStyle == null)
            {
                _infoStyle = new GUIStyle(EditorStyles.label);
                _infoStyle.alignment = TextAnchor.MiddleCenter;
            }

            Rect pos1 = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 2);
            Rect pos2 = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 5);

            if (target.GetType() == typeof(InteractionProfileSingleTarget))
            {
                EditorGUI.DrawRect(pos1, new Color(0f, 1f, 0f, 0.4f));
                EditorGUI.LabelField(pos1, "Single Target Profile", _headerStyle);
                EditorGUI.DrawRect(pos2, new Color(0f, 1f, 0f, 0.25f));
                EditorGUI.LabelField(pos2, SingleTargetInfo, _infoStyle);
            }
            else
            {
                EditorGUI.DrawRect(pos1, new Color(0f, 0.6f, 1f, 0.4f));
                EditorGUI.LabelField(pos1, "Multi Target Profile", _headerStyle);
                EditorGUI.DrawRect(pos2, new Color(0f, 0.6f, 1f, 0.25f));
                EditorGUI.LabelField(pos2, MultiTargetInfo, _infoStyle);
            }

            bool guiEnabled = GUI.enabled;
            GUI.enabled = !Application.isPlaying;
            EditorGUILayout.Space(20f);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_tryStartInAwake, ContentTryStartInAwake);

            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Keyboard Inputs", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_pcMode, ContentPCMode);
            if (_pcMode.intValue != 0)
            {
                EditorGUILayout.PropertyField(_pcInputType, ContentPCInputType);
                EditorGUI.indentLevel++;
                switch (_pcInputType.intValue)
                {
                    case 0:
                        EditorGUILayout.PropertyField(_pcKeycode, ContentPCKeycode);
                        break;
                    case 1:
                        EditorGUILayout.PropertyField(_pcTriggerName, ContentPCButtonName);
                        break;
                    case 2:
                        EditorGUILayout.PropertyField(_pcTriggerName, ContentPCAxisNameHorizontal);
                        EditorGUILayout.PropertyField(_pcTriggerName2, ContentPCAxisNameVertical);
                        EditorGUILayout.PropertyField(_pcAxisThreshold, ContentPCAxisThreshold);
                        break;
                }

                EditorGUI.indentLevel--;
            }


            EditorGUILayout.Space(20f);
            EditorGUILayout.LabelField("UI Inputs", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_uiMode, ContentUIMode);
            if (_uiMode.intValue >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_uiAxisIntensity, ContentUIAxisIntensity);
                EditorGUILayout.PropertyField(_uiAxisThreshold, ContentUIAxisThreshold);
                EditorGUI.indentLevel--;
            }

            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
            GUI.enabled = guiEnabled;
        }
    }
}