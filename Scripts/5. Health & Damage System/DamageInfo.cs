using System;
using UnityEngine;

namespace CCN.Health
{
    [Serializable]
    public struct DamageInfo
    {
        /// <summery> How much damage to add according to dealer. This does not account for DamageMultiplier of the hitBox. </summery>
        public float RawAmount { get; private set; }

        /// <summery> Damage dealer. </summery>
        public readonly IDamageDealer Dealer;

        /// <summery> Location (in this object) where damage should be dealt. </summery>
        public readonly Vector3 Position;

        /// <summery> Direction of impact </summery>
        public readonly Vector3 Direction;

        /// <summery> Force of impact </summery>
        public readonly Vector3 Force;

        /// <summery> Collider (Child of this object) which received the damage </summery>
        public readonly Collider HitCollider;

        public readonly HitBox HitBox;

        /// <summary> How much damage to actually deal. This is damage after multiplying the rawAmount with the DamageMultiplier of the hitBox. </summary>
        public float Amount
        {
            get
            {
                return HitBox == null ? RawAmount : RawAmount * HitBox.DamageMultiplier;
            }
            set
            {
                if (HitBox == null) RawAmount -= value;
                else RawAmount -= value / HitBox.DamageMultiplier;
            }
        }

        public DamageInfo(float rawAmount, IDamageDealer dealer, Vector3 position = new Vector3(), Vector3 force = new Vector3(), Collider hitCollider = null, HitBox hitBox = null)
        {
            this.RawAmount = rawAmount;
            this.Dealer = dealer;
            this.Position = position;
            this.Force = force;
            this.Direction = force.normalized;
            this.HitCollider = hitCollider;
            this.HitBox = hitBox;
        }
    }
}