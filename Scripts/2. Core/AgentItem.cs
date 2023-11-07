using CCN.InputSystemWrapper;
using UnityEngine;

namespace CCN.Core
{
    public enum ItemSlot
    {
        Head = 0,
        Face = 1,
        Neck = 2,
        LeftShoulder = 3,
        RightShoulder = 4,
        LeftForearm = 5,
        RightForearm = 6,
        LeftHand = 7,
        RightHand = 8,
        LeftThigh = 9,
        RightThigh = 10,
        LeftLeg = 11,
        RightLeg = 12,
        LeftFoot = 13,
        RightFoot = 14,
    }


    /// <summary>
    /// Base class for all items.
    /// This item will be enabled only while its equipped. (Unless the end user enables it by code)
    /// </summary>
    public abstract class AgentItem : MonoBehaviour, IInteractionProfileTarget
    {
        #region ToolTips
        private const string TT_ID = "Unique identifier to identify this item in Animator component. \n \"Equipped Item ID\" will be set to +id while equipping this item, and -id while unequipping this item. \n \"Using Item ID\" will be set to +id while equipping this item";
        private const string TT_SLOT = "Which body part of agent should hold this item. This has nothing to do with object actual position related to agent. One body part can only hold one item at a time.";
        private const string TT_EQUIPMENT_PROFILE = "Profile to equip/unequip this behaviour.";
        private const string TT_USE_PROFILE = "Profile to equip/unequip this behaviour.";
        private const string TT_ACTIVATE_WITH_ITEM = "Can be null. GameObject that will be active only when this item is equipped.";
        private const string TT_MOVE_SPEED_MULTIPLIER = "Multiply movement speed of the agent when this item is equipped.";
        private const string TT_EQUIP_ANIMATION_DURATION = "Duration of equip animation";
        private const string TT_UNEQUIP_ANIMATION_DURATION = "Duration of unequip animation";
        private const string TT_AUTO_USE_WHEN_EQUIPPED = "Should this item be used as soon as its equipped?";
        #endregion

        #region Fields & Properties
        // @formatter:off
        [Header("Item Info")]
        [SerializeField, Tooltip(TT_ID)]                         protected int id;
        [SerializeField, Tooltip(TT_SLOT)]                       protected ItemSlot slot;
        [SerializeField, Tooltip(TT_EQUIPMENT_PROFILE)]          protected InteractionProfileBase equipmentProfile;
        [SerializeField, Tooltip(TT_USE_PROFILE)]                protected InteractionProfileBase useProfile;
        [SerializeField, Tooltip(TT_ACTIVATE_WITH_ITEM)]         protected GameObject activateWithItem;
        [SerializeField, Tooltip(TT_MOVE_SPEED_MULTIPLIER)]      protected float moveSpeedMultiplier = 1f;
        [SerializeField, Tooltip(TT_EQUIP_ANIMATION_DURATION)]   protected float equipAnimationDuration = 0f;
        [SerializeField, Tooltip(TT_UNEQUIP_ANIMATION_DURATION)] protected float unequipAnimationDuration = 0f;
        [SerializeField, Tooltip(TT_AUTO_USE_WHEN_EQUIPPED)]     protected bool autoUseWhenEquipped;
        // @formatter:off
        
        /// <summary> Agent that is controlling this behaviour </summary>
        public Agent Agent { get; private set; }

        /// <summary> Is this item equipped </summary>
        public bool IsEquipped
        {
            get => enabled;
            private set
            {
                if (activateWithItem != null) activateWithItem.SetActive(value);
                enabled = value;
            }
        }

        /// <summary> Is this item equipped </summary>
        public bool IsUsing { get; private set; }
        
        /// <summary> Unique identifier for this item. </summary>
        public int ID => id;

        /// <summary> Which body part of agent should hold this item. One body part can only hold one item at a time </summary>
        public ItemSlot Slot => slot;

        /// <summary> Duration of equip animation </summary>
        public float EquipAnimationDuration => equipAnimationDuration;

        /// <summary> Duration of unequip animation </summary>
        public float UnequipAnimationDuration => unequipAnimationDuration;

        /// <summary> What should be the movement speed of Agent when this behaviour is enabled. </summary>
        public float MoveSpeedMultiplier => moveSpeedMultiplier;
        #endregion

        #region Abstract
        /// <summary> Callback when item is equipped </summary>
        protected abstract void Equip();

        /// <summary> Callback when item is unequipped </summary>
        protected abstract void Unequip();

        /// <summary> Use this item </summary>
        protected abstract void StartUse();
        
        /// <summary> Use this item </summary>
        protected abstract void StopUse();
        #endregion

        #region Funcionalities
        public virtual void Init(Agent agent)
        {
            Agent = agent;
            IsEquipped = false;
            if(equipmentProfile) equipmentProfile.DoTarget(this, agent);
            if(useProfile) useProfile.DoTarget(this, agent);
        }
        
        /// <returns> true if the item was equipped </returns>
        public bool TryEquip()
        {
            if (Agent.TryEquipItem(this) == false) return false;

            IsEquipped = true;
            Equip();
            if (autoUseWhenEquipped) TryStartUse();
            return true;
        }

        /// <returns> true if the item was unequipped </returns>
        public bool TryUnequip()
        {
            if (Agent.TryUnequipItem(this) == false) return false;
            if (IsUsing) TryStopUse();

            IsEquipped = false;
            Unequip();
            return true;
        }

        /// <summary> Use this item if its equipped. </summary>
        public bool TryStartUse()
        {
            if (Agent.StartItemUse(this) == false) return false;
            
            IsUsing = true;
            StartUse();
            return true;
        }

        /// <summary> Use this item if its equipped. </summary>
        public bool TryStopUse()
        {
            if (Agent.StopItemUse(this) == false)
            {
                Debug.Log($"Failed to stop item use: {name}");
                return false;
            }

            IsUsing = false;
            StopUse();
            return true;
        }
        #endregion

        #region Virtual Members
        protected virtual void Awake()
        {
            enabled = false;
        }

        /// <returns> true if item can be equipped </returns>
        public virtual bool CanEquip() => !IsEquipped;

        /// <returns> true if item can be unequipped </returns>
        public virtual bool CanUnequip() => IsEquipped;
        #endregion

        #region Interaction Profile
        /// <summary> In-case if the child class has its own interaction profile </summary>
        /// <returns> true if this item is interacting with respect to given profile </returns>
        protected virtual bool IsInteractingWithProfile(InteractionProfileBase profile)
        {
            if (profile == equipmentProfile) return IsEquipped;
            if (profile == useProfile) return IsUsing;
            return false;
        }

        /// <summary> In-case if the child class has its own interaction profile </summary>
        /// <returns>
        /// true if the profile belongs to parent class interaction.
        /// So if parent class method returns true, child classes can simply exit out of function.
        /// </returns>
        protected virtual bool StartInteractionWithProfile(InteractionProfileBase profile)
        {
            if (profile == equipmentProfile)
            {
                TryEquip();
                return true;
            }

            if (profile == useProfile)
            {
                TryStartUse();
                return true;
            }
            return false;
        }

        /// <summary> In-case if the child class has its own interaction profile </summary>
        /// <returns>
        /// true if the profile belongs to parent class interaction.
        /// So if parent class method returns true, child classes can simply exit out of function.
        /// </returns>
        protected virtual bool EndInteractionWithProfile(InteractionProfileBase profile)
        {
            if (profile == equipmentProfile)
            {
                TryUnequip();
                return true;
            }

            if (profile == useProfile)
            {
                TryStopUse();
                return true;
            }
            return false;
        }
        
        
        bool IInteractionProfileTarget.IsInteracting(InteractionProfileBase profile) => IsInteractingWithProfile(profile);
        void IInteractionProfileTarget.StartInteraction(InteractionProfileBase profile) => StartInteractionWithProfile(profile);
        void IInteractionProfileTarget.EndInteraction(InteractionProfileBase profile) => EndInteractionWithProfile(profile);
        #endregion
    }
}