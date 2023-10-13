using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CCN.Core;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class AbilitiesSearchProvider : ScriptableObject, ISearchWindowProvider
{
    private static List<Type> AllAbilityClasses;

    public Action<Type> OnClose;
    private static List<SearchTreeEntry> _treeEntries;

    static AbilitiesSearchProvider()
    {
        AllAbilityClasses = new List<Type>();
        var type = typeof(Ability);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type classType in assembly.GetTypes())
            {
                if (classType.IsSubclassOf(type) && !classType.IsAbstract)
                {
                    AllAbilityClasses.Add(classType);
                }
            }
        }
    }

    public static async void RefreshAbilitiesList(Agent agent)
    {
        await Task.Delay(10);
        _treeEntries = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Conditions"), 0)
        };

        foreach (Type type in AllAbilityClasses)
        {
            if (agent.HasAbility(type)) continue;

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