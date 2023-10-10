using System;
using UnityEngine;
using UnityEngine.Events;

namespace KS.CharaCon
{
    /// <summary> Target object that the player can shoot </summary>
    public class Target : MonoBehaviour
    {
        /// <summary> Called when the player hit this object </summary>
        public static event Action<Target> OnTakeDamage;
        
        /// <summary> Called when this object runs out of health </summary>
        public static event Action<Target> OnDeath;
        
        [SerializeField, Tooltip("How much damage this object can take.")] private float health = 100f;
        [SerializeField, Tooltip("Event callback when this object takes damage")] private UnityEvent onTakeDamage;
        [SerializeField, Tooltip("Event callback when this object runs out of health")] private UnityEvent onDeath;
        
        /// <summary> Call this method to reduce the health of this object </summary>
        /// <param name="damage"> How much health to be reduced </param>
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
            else
            {
                OnTakeDamage?.Invoke(this);
                onTakeDamage.Invoke();
            }
        }

        /// <summary> Call this method to destroy this object </summary>
        public void Die()
        {
            Destroy(gameObject);
            OnDeath?.Invoke(this);
            onDeath.Invoke();
        }
    }
}