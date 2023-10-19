using System;
using UnityEngine;

namespace CCN.Health
{
    [Serializable]
    public class HitBox
    {
        #if UNITY_EDITOR
        public string name;
        #endif

        [SerializeField, Tooltip("The collider that will receive the damage.")]
        private Collider collider;

        [SerializeField, Tooltip("How much should the damage be multiplied when the collider is damaged.")]
        private float damageMultiplier = 1f;

        [SerializeField, Tooltip("How much should the damage be multiplied when the collider is damaged.")]
        public DamageMarks damageMarks;

        
        public HitBox(Collider collider)
        {
            this.collider = collider;
        }

        /// <summary>The collider that will receive the damage.</summary>
        public Collider Collider => collider;

        /// <summary>How much should the damage be multiplied when the collider is damaged.</summary>
        public float DamageMultiplier => damageMultiplier;
    }
}