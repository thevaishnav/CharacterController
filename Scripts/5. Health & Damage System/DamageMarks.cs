using UnityEngine;

namespace CCN.Health
{
    public class DamageMarks : ScriptableObject
    {
        [SerializeField] private GameObject[] marks;

        public GameObject SpawnRandom(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (marks == null || marks.Length == 0) return null;

            GameObject go = marks[Random.Range(0, marks.Length)];
            return Instantiate(go, position, rotation, parent);
        }
    }
}