using System;
using CCN.Health.Collections;
using UnityEngine;

namespace CCN.Health
{
    [Serializable]
    public class Shield
    {
        [SerializeField, Tooltip("Health of the shield")]
        protected float value;

        [SerializeField, Tooltip("Fraction of damage that the shield will absorb until it breaks"), Range(0, 1)]
        protected float absorptionRate;

        [SerializeField, Tooltip("Should this shield be restored on respawn")]
        protected bool restoreOnRespawn;

        
        public float Value => value;
        protected float startValue;

        public Shield(float value, float absorptionRate, bool restoreOnRespawn)
        {
            this.value = value;
            this.absorptionRate = absorptionRate;
            this.restoreOnRespawn = restoreOnRespawn;
        }

        /// <summary> Init this object </summary>
        public virtual void Init()
        {
            startValue = value;
        }

        
        /// <summary> Damage the shield </summary>
        /// <param name="amount"> Amount Damage </param>
        /// <returns> Amount of damage left un-dealt (This damage will be applied to next shield or Damage Reciever) </returns>
        public virtual float Damage(float amount)
        {
            if (value <= 0) return amount;


            float shouldAbsorb = amount * absorptionRate;
            if (shouldAbsorb <= value)
            {
                amount -= value;
                value = 0;
            }
            else
            {
                amount -= shouldAbsorb;
                value -= shouldAbsorb;
            }

            return amount;
        }

        /// <summary>
        /// Called when the parent <see cref="DamageReceiver"/> is respawned
        /// </summary>
        public virtual void OnRespawn()
        {
            if (restoreOnRespawn) value = startValue;
        }
    }
}
