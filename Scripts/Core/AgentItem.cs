using CCN.InputSystemWrapper;
using UnityEngine;

namespace CCN.Core
{
    public enum ItemSpot
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
    /// This item will be enabled only while its equipped.
    /// </summary>
    public abstract class AgentItem : MonoBehaviour, IInteractionProfileTarget
    {
        #region Fields & Properties
        [SerializeField, Tooltip("Unique identifier for this item.")]
        protected int id;

        [SerializeField, Tooltip("Which body part of agent should hold this item. One body part can only hold one item at a time")]
        protected ItemSpot spot;

        [SerializeField, Tooltip("Multiply movement speed of the agent when this item is equipped.")]
        protected float moveSpeedMultiplier = 1f;

        [SerializeField, Tooltip("Duration of equip animation")] 
        protected float equipAnimationDuration = 0f;

        [SerializeField, Tooltip("Duration of unequip animation")] 
        protected float unequipAnimationDuration = 0f;

        [SerializeField, Tooltip("Profile to equip/unequip this behaviour.")]
        protected InteractionProfileBase equipmentProfile;

        [SerializeField, Tooltip("Profile to equip/unequip this behaviour.")]
        protected InteractionProfileBase useProfile;

        [SerializeField, Tooltip("Should this item be used as soon as its equipped?")]
        protected bool autoUseWhenEquipped;

        /// <summary> Agent that is controlling this behaviour </summary>
        public Agent Agent { get; private set; }

        /// <summary> Is this item equipped </summary>
        public bool IsEquipped { get; private set; }
        
        /// <summary> Unique identifier for this item. </summary>
        public int ID => id;

        /// <summary> Which body part of agent should hold this item. One body part can only hold one item at a time </summary>
        public ItemSpot Spot => spot;

        /// <summary> Duration of equip animation </summary>
        public float EquipAnimationDuration => equipAnimationDuration;

        /// <summary> Duration of unequip animation </summary>
        public float UnequipAnimationDuration => unequipAnimationDuration;

        /// <summary> What should be the movement speed of Agent when this behaviour is enabled. </summary>
        public float MoveSpeedMultiplier => moveSpeedMultiplier;
        #endregion

        #region Abstract
        /// <summary> Use this item </summary>
        public abstract void Use();
        #endregion

        #region Funcionalities
        internal void Init(Agent agent)
        {
            Agent = agent;
            if(equipmentProfile) equipmentProfile.DoTarget(this, agent);
            if(useProfile) useProfile.DoTarget(this, agent);
        }

        /// <summary> if value: Equip,  else Unequip  </summary>
        internal void DoSetState(bool value)
        {
            IsEquipped = value;
            if (value)
            {
                OnEquip();
                if (autoUseWhenEquipped) Agent.UseItem(this);
            }
            else
            {
                OnUnequip();
            }
        }

        /// <returns> true if the item was equipped </returns>
        public bool TryEquip() => Agent.TryEquipItem(this);

        /// <returns> true if the item was unequipped </returns>
        public bool TryUnequip() => Agent.TryUnequipItem(this);

        /// <summary> Use this item if its equipped. </summary>
        public void TryUse() => Agent.UseItem(this);
        #endregion

        #region Virtual Members
        /// <returns> true if item is being used </returns>
        public virtual bool IsUsing() => false;
        
        /// <returns> true if item can be equipped </returns>
        public virtual bool CanEquip() => !IsEquipped;

        /// <returns> true if item can be unequipped </returns>
        public virtual bool CanUnequip() => IsEquipped;

        /// <summary> Callback when item is equipped </summary>
        protected virtual void OnEquip()
        {
        }

        /// <summary> Callback when item is unequipped </summary>
        protected virtual void OnUnequip()
        {
        }
        #endregion

        #region Interaction Profile
        bool IInteractionProfileTarget.IsInteracting(InteractionProfileBase profile)
        {
            if (profile == equipmentProfile) return IsEquipped;
            else if (profile == useProfile) return IsUsing();
            else return false;
        }

        void IInteractionProfileTarget.StartInteraction(InteractionProfileBase profile)
        {
            if (profile == equipmentProfile) TryEquip();
            else if (profile == useProfile) TryUse();
        }

        void IInteractionProfileTarget.EndInteraction(InteractionProfileBase profile)
        {
            if (profile == equipmentProfile) TryUnequip();
        }
        #endregion
    }
}