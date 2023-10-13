using System;
using UnityEngine;

namespace CCN.Health
{
    [Serializable]
    public struct DamageInfo
    {
        /// <summery> How much damage to add according to dealer. This does not account for DamageMultiplier of the hitBox. </summery>
        public float rawAmount { get; private set; }

        /// <summery> Damage dealer. </summery>
        public readonly DamageDealer dealer;

        /// <summery> Location (in this object) where damage should be dealt. </summery>
        public readonly Vector3 position;

        /// <summery> Direction of impact </summery>
        public readonly Vector3 direction;

        /// <summery> Force of impact </summery>
        public readonly Vector3 force;

        /// <summery> Collider (Child of this object) which received the damage </summery>
        public readonly Collider hitCollider;

        public readonly HitBox hitBox;

        /// <summary> How much damage to actually deal. This is damage after multiplying the rawAmount with the DamageMultiplier of the hitBox. </summary>
        public float Amount
        {
            get
            {
                return hitBox == null ? rawAmount : rawAmount * hitBox.DamageMultiplier;
            }
            set
            {
                if (hitBox == null) rawAmount -= value;
                else rawAmount -= value / hitBox.DamageMultiplier;
            }
        }

        public DamageInfo(float rawAmount, DamageDealer dealer, Vector3 position = new Vector3(), Vector3 force = new Vector3(), Collider hitCollider = null, HitBox hitBox = null)
        {
            this.rawAmount = rawAmount;
            this.dealer = dealer;
            this.position = position;
            this.force = force;
            this.direction = force.normalized;
            this.hitCollider = hitCollider;
            this.hitBox = hitBox;
        }
    }
}