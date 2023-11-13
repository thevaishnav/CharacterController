using System;
using System.Reflection;
using Omnix.CCN.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Omnix.CCN.EditorSpace
{
    [CustomEditor(typeof(Agent))]
    public class Ed_Agent : UnityEditor.Editor
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Default | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType | BindingFlags.OptionalParamBinding | BindingFlags.IgnoreReturn;

        private static readonly Type TypeOfBehaviour = typeof(AgentBehaviour);
        private Agent _agent;
        private SerializedProperty _abilities;

        private void OnEnable()
        {
            _agent = (Agent)target;
            _abilities = serializedObject.FindProperty("behaviors");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();
            RemoveEmptyBehaviour();
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }

        private static void CallMethod(Type type, object obj, string methodName, params object[] options)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, BINDING_FLAGS);
            if (methodInfo == null) Debug.LogError($"Method {methodName} not found");
            else methodInfo?.Invoke(obj, options);
        }

        private void RemoveEmptyBehaviour()
        {
            for (int i = 0; i < _abilities.arraySize; i++)
            {
                SerializedProperty behaviour = _abilities.GetArrayElementAtIndex(i);
                // Debug.Log($"{i} => <{behaviour.managedReferenceFullTypename}>");
                if (string.IsNullOrEmpty(behaviour.managedReferenceFullTypename))
                {
                    _abilities.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();

                    BehavioursSearchProvider current = CreateInstance<BehavioursSearchProvider>();
                    current.agent = _agent;
                    current.OnClose = SetPropertyBehaviourType;
                    SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), current);
                    return;
                }
            }
        }

        private void SetPropertyBehaviourType(Type type)
        {
            if (_agent.HasBehavior(type))
            {
                EditorUtility.DisplayDialog("Invalid", $"Behaviour of type {type.Name} is already added to agent {_agent.gameObject}", "Okay");
                return;
            }

            AgentBehaviour behaviour = (AgentBehaviour)Activator.CreateInstance(type);
            CallMethod(TypeOfBehaviour, behaviour, "Reset", _agent);
            if (Application.isPlaying)
            {
                CallMethod(TypeOfBehaviour, behaviour, "Init", _agent);
                if (_agent.enabled && _agent.gameObject.activeInHierarchy)
                {
                    CallMethod(TypeOfBehaviour, behaviour, "OnAgentEnabled");
                    CallMethod(TypeOfBehaviour, behaviour, "OnBehaviourEnabled");
                }
                else
                {
                    CallMethod(TypeOfBehaviour, behaviour, "OnAgentDisabled");
                    CallMethod(TypeOfBehaviour, behaviour, "OnBehaviourDisabled");
                }
            }

            _abilities.arraySize++;
            SerializedProperty property = _abilities.GetArrayElementAtIndex(_abilities.arraySize - 1);
            property.managedReferenceValue = behaviour;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_agent);
        }
    }
}