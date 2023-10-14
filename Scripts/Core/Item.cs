using UnityEngine;

/*namespace CCN.Core
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


    /// <summary> This item will be enabled only when its being used. </summary>
    public abstract class Item : MonoBehaviour, IStartStopProfileTarget
    {
        [SerializeField, Tooltip("Unique identifier for this item.")]
        private int id;

        [SerializeField, Tooltip("Which body part of agent should hold this item. One body part can only hold one item at a time")]
        private ItemSpot spot;
        
        [SerializeField, Tooltip("Multiply movement speed of the agent when this item is equipped.")]
        private float moveSpeedMultiplier = 1f;

        [SerializeField, Tooltip("Profile to equip/unequip this behaviour.")]
        private StartStopProfile startStopProfile;

        [SerializeField, Tooltip("Should this item be used as soon as its equipped?")] 
        private bool useWhenEquipped;

        /// <summary>
        /// Unique identifier for this item.
        /// </summary>
        public int ID => id;

        /// <summary> Which body part of agent should hold this item. One body part can only hold one item at a time </summary>
        public ItemSpot Spot => spot;

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

        /// <summary> Is this item equipped </summary>
        public bool IsEnabled { get; private set; }


        /// <summary> Try to enable this behaviour. </summary>
        /// <returns> true if the behaviour was enabled </returns>
        public bool TryEnable() => Agent.TryEquipItem(this);

        /// <summary> Try to disable this behaviour. </summary>
        /// <returns> true if the behaviour was disabled </returns>
        public bool TryDisable() => Agent.TryUnequipItem(this);

        
        public void DoSetState(bool value)
        {
            IsEnabled = value;
            if (value)
            {
                OnEquip();
                if (useWhenEquipped)
                {
                    Agent.UseItem(this);
                }
            }
            else OnUnequip();
        }
        

        public abstract bool CanEquip();
        public abstract bool CanUnequip();
        public abstract bool IsInUse();
        public abstract void OnEquip();
        public abstract void OnUnequip();
        public abstract void OnUse();

    }
}*/