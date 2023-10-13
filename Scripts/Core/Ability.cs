using System;
using System.Reflection;
using UnityEngine;


namespace CCN.Core
{
    /// <summary>
    /// Base Class for all the character abilities.
    /// An ability is a state which (when active) controls the behaviour of the character.
    /// </summary>
    [Serializable]
    public abstract class Ability : IStartStopProfileTarget
    {
        #region Field and Properties
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private string name;
        #endif
        
        [SerializeField, Tooltip("Unique identifier for this ability. When this ability is enabled, \"Ability ID\" in animator will be set to this parameter value. Positive for user defined abilities, negative for built-in abilities, 0 for nullAbility")]
        private int abilityId;
        
        [SerializeField, Tooltip("What should be the movement speed of the character when this ability is enabled. Keep it &lt;=0 if you wish to use default speed (set in Agent component) ")]
        private float targetSpeed;

        [SerializeField, Tooltip("Profile to start/stop this ability.")] 
        private StartStopProfile startStopProfile;

        [SerializeField, Tooltip("Should this ability try to start in Awake")]
        private bool tryStartInAwake;

        /// <summary> Unique identifier for this ability. When this ability is enabled, \"Ability Index\" in animator will be set to this parameter value. Positive for custom abilities, negative for built-in abilities, 0 for nullAbility  </summary>
        public int AbilityId => abilityId;

        /// <summary> Current Acceleration of the player </summary>
        public Vector3 Acceleration => Agent.Acceleration;

        /// <summary> Current Velocity of the player </summary>
        public Vector3 Velocity => Agent.Velocity;

        /// <summary> Current speed of the player </summary>
        public float Speed => Agent.Speed;

        /// <summary> Is player moving </summary>
        public bool IsMoving => Agent.IsMoving;

        /// <summary> Is player touching the ground </summary>
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

        /// <summary> What should be the movement speed of player when this ability is enabled. </summary>
        public float TargetSpeed => targetSpeed;

        /// <summary> Is this ability enabled </summary>
        public bool IsEnabled { get; private set; }

        /// <summary> Agent that is controlling this ability </summary>
        public Agent Agent { get; private set; }
        #endregion

        #region Functionalities
        /// <summary> Initialize this ability </summary>
        /// <param name="agent"> parent of this ability </param>
        internal void Init(Agent agent)
        {
            Agent = agent;
            IsEnabled = false;
            
            if (tryStartInAwake)
            {
                Agent.TryEnableAbility(this);
            }

            if (startStopProfile != null)
            {
                startStopProfile.SetTarget(this);
            }
            
            Agent.EvEnabled += OnPlayerEnabled;
            Agent.EvDisabled += OnPlayerDisabled;
        }

        /// <summary> Sets value of <see cref="IsEnabled"/> variable and calls relevant callbacks.</summary>
        /// <param name="value"> Value to set </param>
        internal void DoEnable(bool value)
        {
            IsEnabled = value;
            if (value) OnAbilityEnabled();
            else OnAbilityDisabled();
        }
        
        
        /// <summary> Try to enable this ability. </summary>
        /// <returns> true if the ability was enabled </returns>
        public bool TryEnable() => Agent.TryEnableAbility(this);

        /// <summary> Try to disable this ability. </summary>
        /// <returns> true if the ability was disabled </returns>
        public bool TryDisable() => Agent.TryDisableAbility(this);
        
        /// <summary> Try to enable this ability. </summary>
        /// <param name="force"> Should this ability be enabled even if the currently enabled ability blocks this ability enable. </param>
        /// <returns> true if the ability was enabled </returns>
        public bool TryEnable(bool force) => Agent.TryEnableAbility(this, force);

        /// <summary> Try to disable this ability. </summary>
        /// <param name="force"> Should this ability be disabled even if the currently enabled ability blocks this ability disable. </param>
        /// <returns> true if the ability was disabled </returns>
        public bool TryDisable(bool force) => Agent.TryDisableAbility(this, force);
        #endregion

        #region Event Callbacks
        /// <summary> Event callback to check if an ability can be enabled if this ability is enabled </summary>
        /// <param name="abilityAboutToStart"> Ability that will be started if returned true </param>
        public virtual bool ShouldBlockAbilityStart(Ability abilityAboutToStart) => false;

        /// <summary> Event callback to check if an ability can be disabled if this ability is enabled </summary>
        /// <param name="abilityAboutToStop"> Ability that will be stoppped if returned true </param>
        public virtual bool ShouldBlockAbilityStop(Ability abilityAboutToStop) => false;
        
        /// <summary> Event callback when the player GameObject is enabled </summary>
        protected virtual void OnPlayerEnabled()
        {
        }

        /// <summary> Event callback when the player GameObject is disabled </summary>
        protected virtual void OnPlayerDisabled()
        {
        }

        /// <summary> Event callback when the ability is enabled </summary>
        protected virtual void OnAbilityEnabled()
        {
        }

        /// <summary> Event callback when the ability is disabled </summary>
        protected virtual void OnAbilityDisabled()
        {
        }
        
        protected virtual void Reset(Agent agent)
        {
            // Assign Default values
            Type type = GetType();
            name = type.Name;
            
            DefaultAbilityId defId = type.GetCustomAttribute<DefaultAbilityId>();
            if (defId != null)
            {
                abilityId = defId.value;
            }

            DefaultMoveSpeed moveSpeed = type.GetCustomAttribute<DefaultMoveSpeed>();
            if (moveSpeed != null)
            {
                targetSpeed = moveSpeed.speed;
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