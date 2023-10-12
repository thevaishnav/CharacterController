using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KS.CharaCon;
using KS.CharaCon.Utils;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    private PlayerMovement _playerMovement;
    
    private void OnEnable()
    {
        _playerMovement = (PlayerMovement)target;
        AbilitiesSearchProvider.RefreshAbilitiesList(_playerMovement);
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (_playerMovement.__EditorModeRemoveEmptyAbility__())
        {
            EditorUtility.SetDirty(_playerMovement);
            AbilitiesSearchProvider current = CreateInstance<AbilitiesSearchProvider>();
            current.OnClose = AddAbility;
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), current);
        }
    }

    private void AddAbility(Type type)
    {
        if (_playerMovement.__EditorModeAddAbility__(type))
        {
            EditorUtility.SetDirty(_playerMovement);
            AbilitiesSearchProvider.RefreshAbilitiesList(_playerMovement);
        }
    }
}


public class AbilitiesSearchProvider : ScriptableObject, ISearchWindowProvider
{
    public Action<Type> OnClose;
    private static List<SearchTreeEntry> _treeEntries;

    public static async void RefreshAbilitiesList(PlayerMovement playerMovement)
    {
        await Task.Delay(10);
        _treeEntries = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Conditions"), 0)
        };

        foreach (Type type in typeof(Ability).AllChildClasses())
        {
            if (playerMovement.HasAbility(type)) continue;
            
            SearchTreeEntry item = new SearchTreeEntry(new GUIContent(type.Name));
            item.level = 1;
            item.userData = type;
            _treeEntries.Add(item);
        }
    }
    
    
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) => _treeEntries;

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        OnClose?.Invoke((Type)searchTreeEntry.userData);
        return true;
    }
}
