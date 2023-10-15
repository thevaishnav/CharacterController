using System;
using CCN.Core;
using UnityEngine;
using CCN.InputSystemWrapper;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCN.Behaviours
{
    /// <summary> Jump behaviour for the Agent </summary>
    [Serializable]
    public class Jump : AgentBehaviour
    {
        [SerializeField, Tooltip("Force (Impulse) applied to the Agent the moment player starts jump")]
        private Vector3 jumpForce = new Vector3(0f, 10f, 0f);

        [SerializeField, Tooltip("Force applied to Agent every frame while player holds jump button.")]
        private Vector3 persistantForce = new Vector3(0f, 0.1f, 0f);

        protected override void Reset(Agent agent)
        {
            base.Reset(agent);
            
            id = -2;
            #if UNITY_EDITOR
            interactionProfile = AssetDatabase.LoadAssetAtPath<InteractionProfileBase>(AssetDatabase.GUIDToAssetPath("effcf7ea72703c746a81db8f2c16f836"));
            #endif
        }

        protected override void OnBehaviourEnabled()
        {
            Debug.Log("Enabled");
            if (Agent.IsGrounded) Agent.AddForce(jumpForce, ForceMode.Impulse);
            Agent.EvFixedUpdate += BehaviourUpdate;
        }

        protected override void OnBehaviourDisabled()
        {
            Debug.Log("Disabled");
            Agent.EvFixedUpdate -= BehaviourUpdate;
        }

        protected void BehaviourUpdate()
        {
            Debug.Log("Update");
            Agent.AddForce(persistantForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}