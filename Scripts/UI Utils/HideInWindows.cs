using UnityEngine;

namespace Omnix.CCN.Utils
{
    [DefaultExecutionOrder(-100)]
    public class HideInWindows : MonoBehaviour
    {
        [SerializeField] private bool destroy;
        
        private void Awake()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (destroy) DestroyImmediate(gameObject);
            else gameObject.SetActive(false);
            #endif
        }
    }
}