using System;
using KS.CharaCon.Attributes;
using UnityEngine;

namespace KS.CharaCon.Abilities
{
    /// <summary> Jump ability for the player </summary>
    [Serializable]
    [DefaultAbilityIndex(-2)]
    [DefaultAbilityStartType(KeyCode.Space)]
    [DefaultAbilityEndType(KeyCode.Space)]
    public class JumpAbility : Ability
    {
        [SerializeField, Tooltip("Force (Impulse) applied to the player the moment player starts jump")]
        private Vector3 jumpForce = new Vector3(0f, 10f, 0f);

        [SerializeField, Tooltip("Force applied to player every frame while player holds jump button.")]
        private Vector3 persistantForce = new Vector3(0f, 0.1f, 0f);

        protected override void OnAbilityEnabled()
        {
            if (Controller.IsGrounded) Controller.AddForce(jumpForce, ForceMode.Impulse);
            Controller.EvFixedUpdate += AbilityUpdate;
        }

        protected override void OnAbilityDisabled()
        {
            Controller.EvFixedUpdate -= AbilityUpdate;
        }

        protected void AbilityUpdate()
        {
            Controller.AddForce(persistantForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}