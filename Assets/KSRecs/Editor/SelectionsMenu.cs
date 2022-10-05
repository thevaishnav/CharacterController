using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace KSRecs.Editor
{
    public static class SelectionsMenu
    {
        [MenuItem("GameObject/KS Selections/Remove Missing Reference Scripts", isValidateFunction: true)]
        [MenuItem("GameObject/KS Selections/Remove Duplicate Components", isValidateFunction: true)]
        [MenuItem("GameObject/KS Selections/Remove All Components", isValidateFunction: true)]
        [MenuItem("GameObject/KS Selections/Invert Selection", isValidateFunction: true)]
        [MenuItem("GameObject/KS Selections/Sort Components", isValidateFunction: true)]
        [MenuItem("KS/Selections/Remove Missing Reference Scripts", isValidateFunction: true)]
        [MenuItem("KS/Selections/Remove Duplicate Components", isValidateFunction: true)]
        [MenuItem("KS/Selections/Remove All Components", isValidateFunction: true)]
        [MenuItem("KS/Selections/Invert Selection", isValidateFunction: true)]
        [MenuItem("KS/Selections/Sort Components", isValidateFunction: true)]
        public static bool IsAnythingSelected() => Selection.gameObjects.Length > 0;

        [MenuItem("GameObject/KS Selections/Remove Multiple Components", isValidateFunction: true)]
        [MenuItem("GameObject/KS Selections/Copy Components To", isValidateFunction: true)]
        [MenuItem("KS/Selections/Remove Multiple Components", isValidateFunction: true)]
        [MenuItem("KS/Selections/Copy Components To", isValidateFunction: true)]
        public static bool IsSingleObjectSelected() => Selection.activeGameObject != null && Selection.gameObjects.Length == 1;

        [MenuItem("GameObject/KS Selections/Remove Missing Reference Scripts", priority = 0)]
        [MenuItem("KS/Selections/Remove Missing Reference Scripts", priority = 0)]
        public static void RemoveMissing()
        {
            string stats = KSEditorUtils.RemoveMissingComponents(Selection.gameObjects, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(stats);
        }

        [MenuItem("GameObject/KS Selections/Remove Duplicate Components", priority = 1)]
        [MenuItem("KS/Selections/Remove Duplicate Components", priority = 1)]
        public static void RemoveDuplicateComponents()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                KSEditorUtils.RemoveDuplicateComponents(gameObject);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/KS Selections/Remove Multiple Components", priority = 2)]
        [MenuItem("KS/Selections/Remove Multiple Components", priority = 2)]
        public static void RemoveMultipleComponents()
        {
            RemoveMultipleComponentsWindow.Init();
        }

        [MenuItem("GameObject/KS Selections/Remove All Components", priority = 3)]
        [MenuItem("KS/Selections/Remove All Components", priority = 3)]
        public static void RemoveAllComponents()
        {
            string stats = "Removed All Components:";
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                stats += $"\n{KSEditorUtils.RemoveAllComponents(gameObject)}";
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(stats);
        }

        [MenuItem("GameObject/KS Selections/Sort Components", priority = 4)]
        [MenuItem("KS/Selections/Sort Components", priority = 4)]
        public static void SortComponents()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                KSEditorUtils.SortComponentsByName(gameObject);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/KS Selections/Copy Components To", priority = 5)]
        [MenuItem("KS/Selections/Copy Components To", priority = 5)]
        public static void CopyComponents()
        {
            ComponentCopyWindow.Init();
        }


        [MenuItem("GameObject/KS Selections/Invert Selection", priority = 6)]
        [MenuItem("KS/Selections/Invert Selection", priority = 6)]
        public static void InvertSelection()
        {
            HashSet<GameObject> oldSelection = new HashSet<GameObject>();


            foreach (GameObject obj in Selection.gameObjects)
            {
                oldSelection.Add(obj);
            }

            int newSelCount = 0;
            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
            {
                if (!oldSelection.Contains(obj))
                {
                    newSelCount++;
                }
            }

            Object[] newSelection = new Object[newSelCount];
            newSelCount = 0;
            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
            {
                if (!oldSelection.Contains(obj))
                {
                    newSelection[newSelCount] = obj;
                    newSelCount++;
                }
            }
            
            Selection.objects = newSelection;
        }
        
        [MenuItem("GameObject/KS Selections/Edit Transform", priority = 7)]
        [MenuItem("KS/Selections/Edit Transform", priority = 7)]
        public static void EditTransform()
        {
            EditTransformWindow.Init();
        }
    }
}