using System;
using System.Reflection;
using CCN.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CustomEditor(typeof(Agent))]
public class AgentEditor : Editor
{
    private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.PutDispProperty | System.Reflection.BindingFlags.PutRefDispProperty | System.Reflection.BindingFlags.ExactBinding | System.Reflection.BindingFlags.SuppressChangeType | System.Reflection.BindingFlags.OptionalParamBinding | System.Reflection.BindingFlags.IgnoreReturn;
    private static readonly Type TypeOfAbility = typeof(Ability);
    private Agent _agent;
    private SerializedProperty _abilities;

    private void OnEnable()
    {
        _agent = (Agent)target;
        _abilities = serializedObject.FindProperty("abilities");
        AbilitiesSearchProvider.RefreshAbilitiesList(_agent);
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        RemoveEmptyAbilities();
        if (EditorGUI.EndChangeCheck())
        {
            AbilitiesSearchProvider.RefreshAbilitiesList(_agent);
        }
    }

    private static void CallMethod(Type type, object obj, string methodName, params object[] options)
    {
        MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags);
        if (methodInfo == null) Debug.LogError($"Method {methodName} not found");
        else methodInfo?.Invoke(obj, options);
    }

    private void RemoveEmptyAbilities()
    {
        for (int i = 0; i < _abilities.arraySize; i++)
        {
            SerializedProperty ability = _abilities.GetArrayElementAtIndex(i);
            if (ability.managedReferenceValue == null)
            {
                _abilities.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                
                AbilitiesSearchProvider current = CreateInstance<AbilitiesSearchProvider>();
                current.OnClose = SetPropertyAbilityType;
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), current);
                return;
            }
        }
    }

    private void SetPropertyAbilityType(Type abilityType)
    {
        if (_agent.HasAbility(abilityType))
        {
            EditorUtility.DisplayDialog("Invalid", $"Ability of type {abilityType.Name} is already added to agent {_agent.gameObject}", "Okay");
            return;
        }

        Ability ability = (Ability)Activator.CreateInstance(abilityType);
        CallMethod(TypeOfAbility, ability, "Reset", _agent);

        if (Application.isPlaying)
        {
            CallMethod(TypeOfAbility, ability, "Init", _agent);
            if (_agent.enabled && _agent.gameObject.activeInHierarchy)
            {
                CallMethod(TypeOfAbility, ability, "OnPlayerEnabled");
                CallMethod(TypeOfAbility, ability, "OnAbilityEnabled");
            }
            else
            {
                CallMethod(TypeOfAbility, ability, "OnPlayerDisabled");
                CallMethod(TypeOfAbility, ability, "OnAbilityDisabled");
            }
        }

        _abilities.arraySize++;
        SerializedProperty property = _abilities.GetArrayElementAtIndex(_abilities.arraySize - 1);
        property.managedReferenceValue = ability;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_agent);
    }
}