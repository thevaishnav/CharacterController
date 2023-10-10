using System;
using System.Collections.Generic;
using System.Reflection;
using KS.CharaCon.Attributes;
using KS.CharaCon.Utils;
using UnityEngine;


namespace KS.CharaCon
{
    /// <summary>
    /// Base Class for all the character abilities.
    /// An ability is a state which (when active) controls the behaviour of the character.
    /// </summary>
    [Serializable]
    public abstract class Ability
    {
        #region Helpers
        /// <summary> What events this ability should subscribe to. </summary>
        [Flags]
        private enum Subscriptions
        {
            Default,
            OnAbilityEnabled,
            OnAbilityDisabled,
            AbilityUpdate,
            AbilityFixedUpdate,
            AbilityTriggerEntered,
            AbilityTriggerStay,
            AbilityTriggerExit,
            AbilityCollisionEnter,
            AbilityCollisionStay,
            AbilityCollisionExit,
            OnPlayerEnabled,
            OnPlayerDisabled,
            PlayerUpdate,
            PlayerFixedUpdate,
            PlayerTriggerEntered,
            PlayerTriggerStay,
            PlayerTriggerExit,
            PlayerCollisionEnter,
            PlayerCollisionStay,
            PlayerCollisionExit,
        }

        /// <summary> What method map to which events. </summary>
        private static readonly Dictionary<string, Subscriptions> AllMethodFlags = new Dictionary<string, Subscriptions>
        {
            { nameof(OnAbilityEnabled), Subscriptions.OnAbilityEnabled },
            { nameof(OnAbilityDisabled), Subscriptions.OnAbilityDisabled },
            { nameof(AbilityUpdate), Subscriptions.AbilityUpdate },
            { nameof(AbilityFixedUpdate), Subscriptions.AbilityFixedUpdate },
            { nameof(AbilityTriggerEntered), Subscriptions.AbilityTriggerEntered },
            { nameof(AbilityTriggerStay), Subscriptions.AbilityTriggerStay },
            { nameof(AbilityTriggerExit), Subscriptions.AbilityTriggerExit },
            { nameof(AbilityCollisionEnter), Subscriptions.AbilityCollisionEnter },
            { nameof(AbilityCollisionStay), Subscriptions.AbilityCollisionStay },
            { nameof(AbilityCollisionExit), Subscriptions.AbilityCollisionExit },

            { nameof(OnPlayerEnabled), Subscriptions.OnPlayerEnabled },
            { nameof(OnPlayerDisabled), Subscriptions.OnPlayerDisabled },
            { nameof(PlayerUpdate), Subscriptions.PlayerUpdate },
            { nameof(PlayerFixedUpdate), Subscriptions.PlayerFixedUpdate },
            { nameof(PlayerTriggerEntered), Subscriptions.PlayerTriggerEntered },
            { nameof(PlayerTriggerStay), Subscriptions.PlayerTriggerStay },
            { nameof(PlayerTriggerExit), Subscriptions.PlayerTriggerExit },
            { nameof(PlayerCollisionEnter), Subscriptions.PlayerCollisionEnter },
            { nameof(PlayerCollisionStay), Subscriptions.PlayerCollisionStay },
            { nameof(PlayerCollisionExit), Subscriptions.PlayerCollisionExit },
        };
        #endregion

        #region Field and Properties
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private string name;
        #endif

        [SerializeField, Tooltip("Unique identifier for this ability. When this ability is enabled, \"Ability Index\" in animator will be set to this parameter value. Positive for custom abilities, negative for built-in abilities, 0 for nullAbility")]
        private int abilityIndex;

        [SerializeField, Tooltip("How you want to start this ability. Automatic means it will start when the controller is activated, manual means you will start it from code, or you can choose to start it on keypress. ")]
        private AbilityStartType startType;

        [SerializeField, Tooltip("keyCode of the key which will start this ability when pressed ")]
        private KeyCode startKeyCode;

        [SerializeField, Tooltip("How you want to end this ability. Automatic means it will end when another ability is started, manual means you will end it from code, or you can choose to start it on keypress. ")]
        private AbilityEndType endType;

        [SerializeField, Tooltip("keyCode of the key which will end this ability when released")]
        private KeyCode endKeyCode;

        [SerializeField, Tooltip("What should be the movement speed of the character when this ability is enabled. Keep it &lt;=0 if you wish to use default speed (set in PlayerMovement component) ")]
        private float targetSpeed;

        /// <summary> What events this ability should subscribe to. </summary>
        [SerializeField, HideInInspector]
        private Subscriptions eventFlags;

        /// <summary> Unique identifier for this ability. When this ability is enabled, \"Ability Index\" in animator will be set to this parameter value. Positive for custom abilities, negative for built-in abilities, 0 for nullAbility  </summary>
        public int AbilityIndex => abilityIndex;
        
        /// <summary> Current Acceleration of the player </summary>
        public Vector3 Acceleration => Controller.Acceleration;

        /// <summary> Current Velocity of the player </summary>
        public Vector3 Velocity => Controller.Velocity;

        /// <summary> Current speed of the player </summary>
        public float Speed => Controller.Speed;

        /// <summary> Is player moving </summary>
        public bool IsMoving => Controller.IsMoving;

        /// <summary> Is player touching the ground </summary>
        public bool IsGrounded => Controller.IsGrounded;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt1 => Controller.ManagedInt1;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt2 => Controller.ManagedInt2;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat1 => Controller.ManagedFloat1;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat2 => Controller.ManagedFloat2;

        /// <summary> Mass used by physics system </summary>
        public float Mass
        {
            get => Controller.Mass;
            set => Controller.Mass = value;
        }

        /// <summary> Gravity used by physics system </summary>
        public Vector3 Gravity
        {
            get => Controller.Gravity;
            set => Controller.Gravity = value;
        }

        /// <summary> What should be the <see cref="PlayerMovement.defaultMoveSpeed"/> of player when this ability is enabled. </summary>
        public float TargetSpeed => targetSpeed;

        /// <summary> Is this ability enabled </summary>
        public bool IsEnabled { get; private set; }

        /// <summary> Controller that is controlling this ability </summary>
        public PlayerMovement Controller { get; private set; }
        #endregion

        #region Functionalities
        protected Ability()
        {
            #if UNITY_EDITOR
            name = GetType().Name;
            #endif

            Type type = GetType();
            DefaultAbilityIndex defIndex = type.GetCustomAttribute<DefaultAbilityIndex>();
            if (defIndex != null)
            {
                abilityIndex = defIndex.Index;
            }

            DefaultAbilityStartType abilityStartType = type.GetCustomAttribute<DefaultAbilityStartType>();
            if (abilityStartType != null)
            {
                startType = abilityStartType.StartType;
                startKeyCode = abilityStartType.StartKeyCode;
            }

            DefaultAbilityEndType abilityEndType = type.GetCustomAttribute<DefaultAbilityEndType>();
            if (abilityEndType != null)
            {
                endType = abilityEndType.EndType;
                endKeyCode = abilityEndType.EndKeyCode;
            }

            DefaultMoveSpeed moveSpeed = type.GetCustomAttribute<DefaultMoveSpeed>();
            if (moveSpeed != null)
            {
                targetSpeed = moveSpeed.Speed;
            }
        }

        /// <summary> Check if this ability needs to be enabled based on the Input </summary>
        private void CheckEnable()
        {
            if (Input.GetKeyDown(startKeyCode))
            {
                TryEnable(true);
            }
        }

        /// <summary> Check if this ability needs to be disabled based on the Input </summary>
        private void CheckDisable()
        {
            if (Input.GetKeyUp(endKeyCode))
            {
                TryDisable(true);
            }
        }

        /// <summary> Subscribe all "Ability____" methods to respective events </summary>
        private void SubAllEvents()
        {
            if (eventFlags.HasFlag(Subscriptions.AbilityUpdate)) Controller.EvUpdate += AbilityUpdate;
            if (eventFlags.HasFlag(Subscriptions.AbilityFixedUpdate)) Controller.EvFixedUpdate += AbilityFixedUpdate;
            if (eventFlags.HasFlag(Subscriptions.AbilityTriggerEntered)) Controller.EvTriggerEntered += AbilityTriggerEntered;
            if (eventFlags.HasFlag(Subscriptions.AbilityTriggerStay)) Controller.EvTriggerStay += AbilityTriggerStay;
            if (eventFlags.HasFlag(Subscriptions.AbilityTriggerExit)) Controller.EvTriggerExit += AbilityTriggerExit;
            if (eventFlags.HasFlag(Subscriptions.AbilityCollisionEnter)) Controller.EvCollisionEnter += AbilityCollisionEnter;
            if (eventFlags.HasFlag(Subscriptions.AbilityCollisionStay)) Controller.EvCollisionStay += AbilityCollisionStay;
            if (eventFlags.HasFlag(Subscriptions.AbilityCollisionExit)) Controller.EvCollisionExit += AbilityCollisionExit;
        }

        /// <summary> Unsubscribe "Ability____" methods from respective events </summary>
        private void UnSubAllEvents()
        {
            if (eventFlags.HasFlag(Subscriptions.AbilityUpdate)) Controller.EvUpdate -= AbilityUpdate;
            if (eventFlags.HasFlag(Subscriptions.AbilityFixedUpdate)) Controller.EvFixedUpdate -= AbilityFixedUpdate;
            if (eventFlags.HasFlag(Subscriptions.AbilityTriggerEntered)) Controller.EvTriggerEntered -= AbilityTriggerEntered;
            if (eventFlags.HasFlag(Subscriptions.AbilityTriggerStay)) Controller.EvTriggerStay -= AbilityTriggerStay;
            if (eventFlags.HasFlag(Subscriptions.AbilityTriggerExit)) Controller.EvTriggerExit -= AbilityTriggerExit;
            if (eventFlags.HasFlag(Subscriptions.AbilityCollisionEnter)) Controller.EvCollisionEnter -= AbilityCollisionEnter;
            if (eventFlags.HasFlag(Subscriptions.AbilityCollisionStay)) Controller.EvCollisionStay -= AbilityCollisionStay;
            if (eventFlags.HasFlag(Subscriptions.AbilityCollisionExit)) Controller.EvCollisionExit -= AbilityCollisionExit;
        }

        /// <summary> Sets value of <see cref="IsEnabled"/> variable and subscribe/unsubscribe from event callbacks </summary>
        /// <param name="value"> Value to set </param>
        internal void DoEnable(bool value)
        {
            IsEnabled = value;
            if (value)
            {
                // If this ability is being enabled
                // Unsubscribe the method which checks whether this ability should be enabled;
                if (startType == AbilityStartType.KeyDown) Controller.EvUpdate -= CheckEnable;

                // Subscribe the method which checks whether this ability should be disabled;
                if (endType == AbilityEndType.KeyUp) Controller.EvUpdate += CheckDisable;

                OnAbilityEnabled();
                SubAllEvents();
            }
            else
            {
                // If this ability is being disabled
                // Subscribe the method which checks whether this ability should be enabled;
                if (startType == AbilityStartType.KeyDown) Controller.EvUpdate += CheckEnable;

                // Unsubscribe the method which checks whether this ability should be disabled;
                if (endType == AbilityEndType.KeyUp) Controller.EvUpdate -= CheckDisable;

                OnAbilityDisabled();
                UnSubAllEvents();
            }
        }

        /// <summary> Initialize this ability </summary>
        /// <param name="playerMovement"> parent of this ability </param>
        internal void Init(PlayerMovement playerMovement)
        {
            Controller = playerMovement;

            if (startType == AbilityStartType.Automatic)
            {
                Controller.TryEnableAbility(this);
            }
            else
            {
                IsEnabled = false;
                if (startType == AbilityStartType.KeyDown)
                {
                    Controller.EvUpdate += CheckEnable;
                }
            }
        }

        /// <summary> Try to enable this ability. </summary>
        /// <param name="force"> Should this ability be enabled even if the currently enabled ability blocks this ability enable. </param>
        /// <returns> true if the ability was enabled </returns>
        public bool TryEnable(bool force = false) => Controller.TryEnableAbility(this, force);

        /// <summary> Try to disable this ability. </summary>
        /// <param name="force"> Should this ability be disabled even if the currently enabled ability blocks this ability disable. </param>
        /// <returns> true if the ability was disabled </returns>
        public bool TryDisable(bool force = false) => Controller.TryDisableAbility(this, force);
        #endregion

        #region Event Callbacks
        /// <summary> Event callback to check if an ability can be enabled if this ability is enabled </summary>
        /// <param name="abilityAboutToStart"> Ability that will be started if returned true </param>
        public virtual bool ShouldBlockAbilityStart(Ability abilityAboutToStart) => false;

        /// <summary> Event callback to check if an ability can be disabled if this ability is enabled </summary>
        /// <param name="abilityAboutToStop"> Ability that will be stoppped if returned true </param>
        public virtual bool ShouldBlockAbilityStop(Ability abilityAboutToStop) => false;

        /// <summary> Event callback when the ability is enabled </summary>
        protected virtual void OnAbilityEnabled() { }

        /// <summary> Event callback when the ability is disabled </summary>
        protected virtual void OnAbilityDisabled() { }

        /// <summary> Update event which is only called while ability is enabled </summary>
        protected virtual void AbilityUpdate() { }

        /// <summary> FixedUpdate event which is only called while ability is enabled </summary>
        protected virtual void AbilityFixedUpdate() { }

        /// <summary> LateUpdate event which is only called while ability is enabled </summary>
        protected virtual void AbilityLateUpdate() { }

        /// <summary> TriggerEntered event which is only called while ability is enabled </summary>
        protected virtual void AbilityTriggerEntered(Collider other) { }

        /// <summary> TriggerStay event which is only called while ability is enabled </summary>
        protected virtual void AbilityTriggerStay(Collider other) { }

        /// <summary> TriggerExit event which is only called while ability is enabled </summary>
        protected virtual void AbilityTriggerExit(Collider other) { }

        /// <summary> CollisionEnter event which is only called while ability is enabled </summary>
        protected virtual void AbilityCollisionEnter(Collision collision) { }

        /// <summary> CollisionStay event which is only called while ability is enabled </summary>
        protected virtual void AbilityCollisionStay(Collision collision) { }

        /// <summary> CollisionExit event which is only called when the ability is enabled </summary>
        protected virtual void AbilityCollisionExit(Collision collision) { }

        /// <summary> Event callback when the player GameObject is enabled </summary>
        protected virtual void OnPlayerEnabled() { }

        /// <summary> Event callback when the player GameObject is disabled </summary>
        protected virtual void OnPlayerDisabled() { }

        /// <summary> Update event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerUpdate() { }

        /// <summary> FixedUpdate event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerFixedUpdate() { }

        /// <summary> LateUpdate event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerLateUpdate() { }

        /// <summary> TriggerEntered event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerTriggerEntered() { }

        /// <summary> TriggerStay event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerTriggerStay() { }

        /// <summary> TriggerExit event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerTriggerExit() { }

        /// <summary> CollisionEnter event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerCollisionEnter() { }

        /// <summary> CollisionStay event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerCollisionStay() { }

        /// <summary> CollisionExit event which is called regardless of ability is enabled or disabled </summary>
        protected virtual void PlayerCollisionExit() { }
        #endregion

        #region Editor
        #if UNITY_EDITOR
        /// <summary> EDITOR ONLY METHOD. Called when the ability is loaded in Editor </summary>
        internal void __EditorModeCreated__()
        {
            eventFlags = Subscriptions.Default;
            foreach (Subscriptions subscriptions in this.ChildOverrides(AllMethodFlags))
            {
                eventFlags |= subscriptions;
            }
        }
        #endif
        #endregion

    }
}