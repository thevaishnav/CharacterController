using System;
using System.Reflection;
using UnityEngine;


namespace CCN.Core
{
    /// <summary>
    /// Base Class for all the agent behaviours.
    /// An behaviour is a state which (when active) controls the behaviour of the agent.
    /// </summary>
    [Serializable]
    public abstract class AgentBehaviour : IStartStopProfileTarget
    {
        #region Field and Properties
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private string name;
        #endif
        
        [SerializeField, Tooltip("Unique identifier for this behaviour. When this behaviour is enabled, \"Behaviour ID\" in animator will be set to this parameter value. Positive for user defined behaviors, negative for built-in behaviors, 0 for no behavior")]
        private int id;
        
        [SerializeField, Tooltip("Multiply movement speed of the agent when this behaviour is enabled.")]
        private float moveSpeedMultiplier = 1f;

        [SerializeField, Tooltip("Profile to start/stop this behaviour.")] 
        private StartStopProfile startStopProfile;

        [SerializeField, Tooltip("Should this behaviour try to start in Awake")]
        private bool tryStartInAwake;

        /// <summary> Unique identifier for this behaviour. When this behaviour is enabled, \"Behaviour ID\" in animator will be set to this parameter value. Positive for user defined behaviors, negative for built-in behaviors, 0 for no behavior  </summary>
        public int ID => id;

        
        
        /// <summary> Agent that is controlling this behaviour </summary>
        public Agent Agent { get; private set; }
        
        /// <summary> Current Acceleration of the Agent </summary>
        public Vector3 Acceleration => Agent.Acceleration;

        /// <summary> Current Velocity of the Agent </summary>
        public Vector3 Velocity => Agent.Velocity;

        /// <summary> Current speed of the Agent </summary>
        public float Speed => Agent.Speed;

        /// <summary> Is Agent moving </summary>
        public bool IsMoving => Agent.IsMoving;

        /// <summary> Is Agent touching the ground </summary>
        public bool IsGrounded => Agent.IsGrounded;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt1 => Agent.ManagedInt1;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt2 => Agent.ManagedInt2;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat1 => Agent.ManagedFloat1;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat2 => Agent.ManagedFloat2;

        /// <summary> Mass used by physics system </summary>
        public float Mass
        {
            get => Agent.mass;
            set => Agent.mass = value;
        }

        /// <summary> Gravity used by physics system </summary>
        public Vector3 Gravity
        {
            get => Agent.gravity;
            set => Agent.gravity = value;
        }

        /// <summary> What should be the movement speed of Agent when this behaviour is enabled. </summary>
        public float MoveSpeedMultiplier => moveSpeedMultiplier;

        /// <summary> Is this behaviour enabled </summary>
        public bool IsEnabled { get; private set; }
        #endregion

        #region Functionalities
        /// <summary> Initialize this behaviour </summary>
        /// <param name="agent"> parent of this behaviour </param>
        internal void Init(Agent agent)
        {
            Agent = agent;
            IsEnabled = false;
            
            if (tryStartInAwake)
            {
                Agent.TryEnableBehaviour(this);
            }

            if (startStopProfile != null)
            {
                startStopProfile.SetTarget(this);
            }
            
            Agent.EvEnabled += OnAgentEnabled;
            Agent.EvDisabled += OnAgentDisabled;
        }

        /// <summary> Sets value of <see cref="IsEnabled"/> variable and calls relevant callbacks.</summary>
        /// <param name="value"> Value to set </param>
        internal void DoSetState(bool value)
        {
            IsEnabled = value;
            if (value) OnBehaviourEnabled();
            else OnBehaviourDisabled();
        }
        
        /// <summary> Try to enable this behaviour. </summary>
        /// <returns> true if the behaviour was enabled </returns>
        public bool TryEnable() => Agent.TryEnableBehaviour(this);

        /// <summary> Try to disable this behaviour. </summary>
        /// <returns> true if the behaviour was disabled </returns>
        public bool TryDisable() => Agent.TryDisableBehavior(this);
        
        /// <summary> Try to enable this behaviour. </summary>
        /// <param name="force"> Should this behaviour be enabled even if the currently enabled behaviour blocks this behaviour enable. </param>
        /// <returns> true if the behaviour was enabled </returns>
        public bool TryEnable(bool force) => Agent.TryEnableBehaviour(this, force);

        /// <summary> Try to disable this behaviour. </summary>
        /// <param name="force"> Should this behaviour be disabled even if the currently enabled behaviour blocks this behaviour disable. </param>
        /// <returns> true if the behaviour was disabled </returns>
        public bool TryDisable(bool force) => Agent.TryDisableBehavior(this, force);
        
        
        /// <summary> Event callback to check if an behaviour can be enabled if this behaviour is enabled </summary>
        /// <param name="behaviourAboutToStart"> Behaviour that will be started if returned true </param>
        public virtual bool ShouldBlockBehaviorStart(AgentBehaviour behaviourAboutToStart) => false;

        /// <summary> Event callback to check if an behaviour can be disabled if this behaviour is enabled </summary>
        /// <param name="behaviourAboutToStop"> Behaviour that will be stoppped if returned true </param>
        public virtual bool ShouldBlockBehaviorStop(AgentBehaviour behaviourAboutToStop) => false;
        
        /// <summary> Event callback when the Agent GameObject is enabled </summary>
        protected virtual void OnAgentEnabled()
        {
        }

        /// <summary> Event callback when the Agent GameObject is disabled </summary>
        protected virtual void OnAgentDisabled()
        {
        }

        /// <summary> Event callback when the behaviour is enabled </summary>
        protected virtual void OnBehaviourEnabled()
        {
        }

        /// <summary> Event callback when the behaviour is disabled </summary>
        protected virtual void OnBehaviourDisabled()
        {
        }
        
        protected virtual void Reset(Agent agent)
        {
            // Assign Default values
            Type type = GetType();
            name = type.Name;
            
            DefaultId defId = type.GetCustomAttribute<DefaultId>();
            if (defId != null)
            {
                id = defId.value;
            }

            DefaultMoveSpeedMultiplier moveSpeedMultiplier = type.GetCustomAttribute<DefaultMoveSpeedMultiplier>();
            if (moveSpeedMultiplier != null)
            {
                this.moveSpeedMultiplier = moveSpeedMultiplier.speed;
            }
            
            StartStopProfileInfo profileInfo = type.GetCustomAttribute<StartStopProfileInfo>();
            if (profileInfo != null)
            {
                startStopProfile = new GameObject($"{type.Name} Profile").AddComponent<StartStopProfile>();
                startStopProfile.transform.SetParent(agent.transform);
                startStopProfile.SetInfo(profileInfo);
            }
        }
        #endregion
    }
}