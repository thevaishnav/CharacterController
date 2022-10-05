using System.Collections;
using System.Collections.Generic;
using KSRecs.EnumerableExtensions;
using KSRecs.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace KSRecs.Examples
{
    public class CommonUtilsTest : MonoBehaviour
    {
        public static readonly string menuName = "SMMM";
        
        [SerializeField] private UnityFunc<bool> func1;
        [SerializeField] private UnityFunc<int> func2;

        void Start()
        {
        }
        
        /*void Start()
        {
            // List<List<List<int>>> list = new List<List<List<int>>>()
            // {
            //     new List<List<int>>()
            //     {
            //         new List<int>() { 1, 2, 3 },
            //         new List<int>(){10, 20, 30},
            //         new List<int>() { 100, 200, 300 }
            //     },
            //     
            //     new List<List<int>>()
            //     {
            //         new List<int>() { 4, 5, 6 },
            //         new List<int>() { 40, 50, 60},
            //         new List<int>() { 400, 500, 600 }
            //     },
            // };
            
            Debug.Log($"OUTPUT: {func.Invoke()}");
        }*/

        IEnumerator _enumerator()
        {
            Debug.Log("Next message in 5 seconds");
            yield return new WaitForSeconds(5);
            Debug.Log("Press A to move to next message");
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A));
            Debug.Log("Quick Press and hold S, next message wont show until you hold it");
            yield return new WaitForSeconds(1);
            yield return new WaitWhile(() => Input.GetKey(KeyCode.S));
            Debug.Log("Well Done");
            yield return null;
            Debug.Log("Now press Q to destroy the object");
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
        }
    }
}
