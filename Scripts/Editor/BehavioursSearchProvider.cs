using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CCN.Core;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class BehavioursSearchProvider : ScriptableObject, ISearchWindowProvider
{
    private static List<Type> AllBehaviourClasses;

    public Action<Type> OnClose;
    private static List<SearchTreeEntry> _treeEntries;

    static BehavioursSearchProvider()
    {
        AllBehaviourClasses = new List<Type>();
        var type = typeof(AgentBehaviour);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type classType in assembly.GetTypes())
            {
                if (classType.IsSubclassOf(type) && !classType.IsAbstract)
                {
                    AllBehaviourClasses.Add(classType);
                }
            }
        }
    }

    public static async void RefreshBehavioursList(Agent agent)
    {
        await Task.Delay(10);
        _treeEntries = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Conditions"), 0)
        };

        foreach (Type type in AllBehaviourClasses)
        {
            if (agent.HasBehavior(type)) continue;

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