using UnityEngine;
namespace KSRecs.Monos
{
    public class LogActivation : MonoBehaviour
    {
        [SerializeField] private bool allowLogs = true;
        private void Awake()
        {
            Debug.unityLogger.logEnabled = allowLogs;
        }

    }
}