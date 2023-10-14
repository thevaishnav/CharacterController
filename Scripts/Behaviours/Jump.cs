using System;
using CCN.Core;
using UnityEngine;

namespace CCN.Behaviours
{
    /// <summary> Jump behaviour for the Agent </summary>
    [Serializable]
    [DefaultId(-2)]
    [StartStopProfileInfo(StartStopProfile.Behaviour.ActiveWhilePressed, StartStopProfile.TriggerType.GetKey, "", KeyCode.Space)]
    public class Jump : AgentBehaviour
    {
        [SerializeField, Tooltip("Force (Impulse) applied to the Agent the moment player starts jump")]
        private Vector3 jumpForce = new Vector3(0f, 10f, 0f);

        [SerializeField, Tooltip("Force applied to Agent every frame while player holds jump button.")]
        private Vector3 persistantForce = new Vector3(0f, 0.1f, 0f);

        protected override void OnBehaviourEnabled()
        {
            if (Agent.IsGrounded) Agent.AddForce(jumpForce, ForceMode.Impulse);
            Agent.EvFixedUpdate += BehaviourUpdate;
        }

        protected override void OnBehaviourDisabled()
        {
            Agent.EvFixedUpdate -= BehaviourUpdate;
        }

        protected void BehaviourUpdate()
        {
            Agent.AddForce(persistantForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}