using System.Collections;
using UnityEngine;


namespace KSRecs.Utils
{
    public class CommonUtilsObject : MonoBehaviour
    {
        public void StartCustomCoroutine(IEnumerator routine)
        {
            StartCoroutine(TheCoroutine(routine));
        }

        IEnumerator TheCoroutine(IEnumerator routine)
        {
            while (routine.MoveNext())
            {
                yield return routine.Current;
            }
            Destroy(gameObject);
        }
    }
}