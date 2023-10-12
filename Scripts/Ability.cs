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
        #region Field and Properties
        [SerializeField, HideInInspector] private string name; // used in editor

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
            get => Controller.mass;
            set => Controller.mass = value;
        }

        /// <summary> Gravity used by physics system </summary>
        public Vector3 Gravity
        {
            get => Controller.gravity;
            set => Controller.gravity = value;
        }

        /// <summary> What should be the movement speed of player when this ability is enabled. </summary>
        public float TargetSpeed => targetSpeed;

        /// <summary> Is this ability enabled </summary>
        public bool IsEnabled { get; private set; }

        /// <summary> Controller that is controlling this ability </summary>
        public PlayerMovement Controller { get; private set; }
        #endregion

        #region Functionalities
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
            }
            else
            {
                // If this ability is being disabled
                // Subscribe the method which checks whether this ability should be enabled;
                if (startType == AbilityStartType.KeyDown) Controller.EvUpdate += CheckEnable;

                // Unsubscribe the method which checks whether this ability should be disabled;
                if (endType == AbilityEndType.KeyUp) Controller.EvUpdate -= CheckDisable;

                OnAbilityDisabled();
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

            Controller.EvEnabled += OnPlayerEnabled;
            Controller.EvDisabled += OnPlayerDisabled;
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
        #endregion

        public void Reset()
        {
            // Assign Default values
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
    }
}