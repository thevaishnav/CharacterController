using System;
using System.Collections.Generic;
using Omnix.CCN.Health;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Omnix.CCN.Collections
{

    [Serializable]
    public abstract class Collection<T>
    {
        [SerializeField] protected List<T> values;

        public T this[int index]
        {
            get => values[index];
            set => values[index] = value;
        }

        public int Count => values.Count;

        public void Add(T item) => values.Add(item);
        public void Remove(T item) => values.Remove(item);
        public bool Contains(T item) => values.Contains(item);
        public void Clear() => values.Clear();
        public void AddRange(IEnumerable<T> range) => values.AddRange(range);
        public IEnumerable<T> Loop() => values;
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Collection<>), true)]
    public class CollectionDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("values"), true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("values"), label, true);
        }
    }
    #endif

    
    [Serializable]
    public class HitBoxCollection : Collection<HitBox>
    {
        [SerializeField] private int hitCollisionCheckCount = 5;

        [NonSerialized] private Dictionary<Collider, HitBox> _map;
        [NonSerialized] private bool _hasMap = false;
        [NonSerialized] private RaycastHit[] _raycastHits;


        private void CreateMap()
        {
            _raycastHits = new RaycastHit[hitCollisionCheckCount];
            _map = new Dictionary<Collider, HitBox>();
            foreach (HitBox hitBox in values)
            {
                _map.Add(hitBox.Collider, hitBox);
            }

            _hasMap = true;
        }


        public bool TryGetHitBox(Collider hitCollider, Vector3 hitPosition, Vector3 hitDirection, out HitBox hitBox)
        {
            if (hitCollider == null)
            {
                hitBox = null;
                return false;
            }

            if (_hasMap == false)
            {
                CreateMap();
            }

            // If collider is present in map, that's it
            if (_map.TryGetValue(hitCollider, out hitBox))
            {
                return true;
            }

            // Its possible that the main collider is blocking raycast.
            // Therefore, redo raycast to check if there are some colliders underneath the original.
            float distance = hitCollider switch
            {
                CapsuleCollider capsule => capsule.radius,
                SphereCollider sphere => sphere.radius,
                _ => hitCollider.bounds.size.x * 0.5f
            };

            int hitCount = Physics.RaycastNonAlloc(hitPosition, hitDirection, _raycastHits, distance, ~0, QueryTriggerInteraction.Ignore);
            switch (hitCount)
            {
                case 0: return false;
                case 1: return _map.TryGetValue(_raycastHits[0].collider, out hitBox);
            }

            QuickSortHits(0, hitCount - 1);
            var index = 0;
            foreach (RaycastHit hit in _raycastHits)
            {
                if (_map.TryGetValue(hit.collider, out hitBox)) return true;
                index++;
                if (index >= hitCount) return false;
            }

            return false;
        }

        private void QuickSortHits(int low, int high)
        {
            // Base case: subarray has one or zero elements
            if (low >= high) return;

            // Choose a pivot element (here we use the last element)
            float pivot = _raycastHits[high].distance;

            // Partition the subarray around the pivot
            int i = low; // value of smaller element
            for (int j = low; j < high; j++)
            {
                // If current element is smaller than or equal to pivot
                if (_raycastHits[j].distance <= pivot)
                {
                    // Swap array[i] and array[j]
                    (_raycastHits[i], _raycastHits[j]) = (_raycastHits[j], _raycastHits[i]);
                    // Increment i
                    i++;
                }
            }

            // Swap array[i] and array[high]
            (_raycastHits[i], _raycastHits[high]) = (_raycastHits[high], _raycastHits[i]);

            // Recursively sort the left and right partitions
            QuickSortHits(low, i - 1);
            QuickSortHits(i + 1, high);
        }

        public static bool TryGet(HitBoxCollection collection, DamageInfo damage, out HitBox hitBox)
        {
            if (collection == null || collection.Count == 0)
            {
                hitBox = null;
                return false;
            }
            
            return collection.TryGetHitBox(damage.HitCollider, damage.Position, damage.Direction, out hitBox);
        }
    }

    [Serializable]
    public class AudioClipsCollection : Collection<AudioClip>
    {
        public void TryPlayRandom(AudioSource source)
        {
            if (values.Count == 0) return;

            if (TryGetClip(out var clip))
            {
                source.clip = clip;
                source.Play();
            }
        }

        private bool TryGetClip(out AudioClip clip)
        {
            if (values.Count == 1)
            {
                clip = values[0];
                return clip != null;
            }

            for (int _ = 0; _ < 3; _++)
            {
                clip = values[Random.Range(0, values.Count)];
                if (clip != null) return true;
            }

            clip = null;
            return false;
        }
    }

    [Serializable]
    public class GameObjectCollection : Collection<GameObject>
    {
        public void SetActive(bool value)
        {
            foreach (GameObject gameObject in values)
            {
                gameObject.SetActive(value);
            }
        }

        public void SpawnSelf(Vector3 position, Quaternion rotation, Transform parent)
        {
            foreach (GameObject gameObject in values)
            {
                Object.Instantiate(gameObject, position, rotation, parent);
            }   
        }

        public void DestroySelf()
        {
            foreach (GameObject gameObject in values)
            {
                Object.DestroyImmediate(gameObject);
            }

            values.Clear();
        }
    }

    [Serializable]
    public class ShieldCollection : Collection<Shield>
    {
        public float Damage(float totalAmount)
        {
            foreach (Shield shield in values)
            {
                totalAmount = shield.Damage(totalAmount);
            }

            return totalAmount;
        }

        public void Init()
        {
            foreach (Shield shield in values)
            {
                shield.Init();
            }
        }

        public void OnRespawn()
        {
            foreach (Shield shield in values)
            {
                shield.OnRespawn();
            }
        }
    }

}