using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// [CustomEditor(typeof(AbilitiesHandler))]
public class AbilitiesHandlerEditor : Editor
{
    private SerializedProperty allAbilities;
    private int selectedAbility;
    protected List<string> allAbisNames;
    
    void OnEnable()
    {
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        /*foreach (string abisName in allAbisNames)
        {
            EditorGUILayout.LabelField(abisName);
        }

        if (GUILayout.Button("++"))
        {
            allAbilities.arraySize += 1;
        }*/
    }
}
