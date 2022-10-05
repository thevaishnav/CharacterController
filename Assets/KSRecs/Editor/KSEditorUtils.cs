using System;
using System.Collections.Generic;
using System.Linq;
using KSRecs.Editor.Extensions;
using KSRecs.UnityObjectExtensions;
using KSRecs.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KSRecs.Editor
{
    public class KSEditorUtils
    {
        private static void FindMissingScriptsInGo(GameObject g, bool searchChildren, out int goCount, out int componentsCount, out int missingCount)
        {
            goCount = 1;
            componentsCount = g.GetComponents<Component>().Length;
            missingCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
            if (searchChildren == false) return;

            foreach (Transform childT in g.transform)
            {
                FindMissingScriptsInGo(childT.gameObject, true, out int gc, out int cc, out int mc);
                goCount += gc;
                componentsCount += cc;
                missingCount += mc;
            }
        }

        public static Action<Object> DestroyAction
        {
            get
            {
                if (Application.isPlaying) return Object.Destroy;
                return Object.DestroyImmediate;
            }
        }

        public static string RemoveMissingComponents(GameObject[] gos, bool searchChildren)
        {
            // GameObject[] go = Selection.gameObjects;
            int goCount = 0;
            int componentsCount = 0;
            int missingCount = 0;

            foreach (var g in gos)
            {
                FindMissingScriptsInGo(g, searchChildren, out int gc, out int cc, out int mc);
                goCount += gc;
                componentsCount += cc;
                missingCount += mc;
                EditorUtility.SetDirty(g);
            }

            return $"{goCount} GameObjects Selected, ({componentsCount} Components) of which {missingCount} Components Deleted";
        }

        public static string RemoveAllComponents(GameObject target)
        {
            Action<Object> DesAct = DestroyAction;
            int count = 0;
            int loopCount = 0;
            bool wantMoreLoop = true;
            while (wantMoreLoop)
            {
                wantMoreLoop = false;
                foreach (Component component in target.GetComponents<Component>())
                {
                    if (component is Transform) continue;
                    if (!target.CanDestroy(component.GetType()))
                    {
                        wantMoreLoop = true;
                        continue;
                    }
                    DesAct(component);
                }

                loopCount++;
                if (loopCount >= 10)
                {
                    EditorUtility.SetDirty(target);
                    return "Cannot delete all components because of dependencies, try again.";
                }
            }

            EditorUtility.SetDirty(target);
            return $"Deleted {count} components from {target.name}";
        }

        public static void SortComponentsByName(GameObject target)
        {
            var components = target.GetComponents<Component>().ToList();
            components.RemoveAll(x => x.GetType().ToString() == "UnityEngine.Transform");

            for (int p = 0; p <= components.Count - 2; p++)
            {
                for (int i = 0; i <= components.Count - 2; i++)
                {
                    Component c1 = components[i];
                    Component c2 = components[i + 1];

                    string name1 = c1.GetType().ToString().Split('.').Last();
                    string name2 = c2.GetType().ToString().Split('.').Last();

                    if (string.Compare(name1, name2) == 1)
                    {
                        ComponentUtility.MoveComponentUp(c2);
                        components = target.GetComponents<Component>().ToList();
                        components.RemoveAll(x => x.GetType().ToString() == "UnityEngine.Transform");
                    }
                }
            }

            EditorUtility.SetDirty(target);
        }
        
        public static void RemoveDuplicateComponents(GameObject target)
        {
            Action<Object> DestAct = DestroyAction;
            List<Component> components = target.GetComponents<Component>().ToList();
            int index = 0;
            int count = 0;
            foreach (Component component in components)
            {
                while (components.LastIndexOf(component) != index)
                {
                    DestAct(component);
                    count++;
                }

                index++;
            }
            EditorUtility.SetDirty(target);
        }

        public static void RemoveComponents(GameObject target, Dictionary<string, bool> components, bool check)
        {
            Action<Object> DesAction = KSEditorUtils.DestroyAction;
            HashSet<string> destroyed = new HashSet<string>();
            int tries = 0;

            while (destroyed.Count < components.Count)
            {
                foreach (Component component in target.GetComponents<Component>())
                {
                    if (component is Transform) continue;
                    if (!target.CanDestroy(component.GetType())) continue;

                    string cName = component.ClassName();
                    if (!destroyed.Contains(cName) && components.ContainsKey(cName) && components[cName] == check)
                    {
                        DesAction(component);
                        destroyed.Add(cName);
                    }
                }

                tries++;
                if (tries > 10) break;
            }

            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}