using System;
using CCN.Core;
using UnityEngine;

namespace CCN.Abilities
{
    /// <summary> Jump ability for the player </summary>
    [Serializable]
    [DefaultAbilityId(-2)]
    [StartStopProfileInfo(StartStopProfile.Behaviour.ActiveWhilePressed, StartStopProfile.TriggerType.GetKey, "", KeyCode.Space)]
    public class JumpAbility : Ability
    {
        [SerializeField, Tooltip("Force (Impulse) applied to the player the moment player starts jump")]
        private Vector3 jumpForce = new Vector3(0f, 10f, 0f);

        [SerializeField, Tooltip("Force applied to player every frame while player holds jump button.")]
        private Vector3 persistantForce = new Vector3(0f, 0.1f, 0f);

        protected override void OnAbilityEnabled()
        {
            if (Agent.IsGrounded) Agent.AddForce(jumpForce, ForceMode.Impulse);
            Agent.EvFixedUpdate += AbilityUpdate;
        }

        protected override void OnAbilityDisabled()
        {
            Agent.EvFixedUpdate -= AbilityUpdate;
        }

        protected void AbilityUpdate()
        {
            Agent.AddForce(persistantForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}