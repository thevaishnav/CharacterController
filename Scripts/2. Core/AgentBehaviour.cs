using System;
using CCN.InputSystemWrapper;
using UnityEngine;


namespace CCN.Core
{
    /// <summary>
    /// Base Class for all the agent behaviours.
    /// An behaviour is a state which (when active) controls the behaviour of the agent.
    /// </summary>
    [Serializable]
    public abstract class AgentBehaviour : IInteractionProfileTarget
    {
        #region Field and Properties
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private string name;
        #endif
        
        [SerializeField, Tooltip("Unique identifier to identify this behaviour in Animator component. \n \"Behaviour ID\" will be set to id When this behaviour is enabled. \n Positive for user defined behaviors, negative for built-in behaviors, 0 for null behavior")]
        protected int id;
        
        [SerializeField, Tooltip("Multiply movement speed of the agent when this behaviour is enabled.")]
        protected float moveSpeedMultiplier = 1f;

        [SerializeField, Tooltip("Profile to start/stop this behaviour.")] 
        protected InteractionProfileBase interactionProfile;

        /// <summary> Unique identifier for this behaviour. When this behaviour is enabled, \"Behaviour ID\" in animator will be set to this parameter value. Positive for user defined behaviors, negative for built-in behaviors, 0 for no behavior  </summary>
        public int ID => id;
        
        /// <summary> Agent that is controlling this behaviour </summary>
        public Agent Agent { get; private set; }
        
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
            
            if (interactionProfile != null)
            {
                interactionProfile.DoTarget(this, agent);
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

        /// <summary> Called when the behaviour is added in the inspector </summary>
        /// <param name="agent"> Agent on which the behaviour is added </param>
        protected virtual void Reset(Agent agent)
        {
            #if UNITY_EDITOR
            name = GetType().Name;
            #endif
        }
        
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
        #endregion

        #region Interaction Profile
        bool IInteractionProfileTarget.IsInteracting(InteractionProfileBase profile) => IsEnabled;
        void IInteractionProfileTarget.StartInteraction(InteractionProfileBase profile) => TryEnable();
        void IInteractionProfileTarget.EndInteraction(InteractionProfileBase profile) => TryDisable();
        #endregion
    }
}