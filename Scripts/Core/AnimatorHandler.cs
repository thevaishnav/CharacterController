using UnityEngine;
// ReSharper disable InconsistentNaming

namespace CCN.Core
{
    /// <summary> Handles animation controllers </summary>
    internal class AnimatorHandler
    {
        #region Statics
        public static readonly float MinMoveSpeed = 0.005f;
        public static readonly float SqreMinMoveSpeed = MinMoveSpeed * MinMoveSpeed;
        private static readonly int ID_Speed = Animator.StringToHash("Speed");
        private static readonly int ID_IsMoving = Animator.StringToHash("Moving");
        private static readonly int ID_IsGrounded = Animator.StringToHash("Grounded");
        private static readonly int ID_BehaviourId = Animator.StringToHash("Behaviour ID");
        private static readonly int ID_UsingEquipped = Animator.StringToHash("Using Item ID");
        private static readonly int ID_EquippedId = Animator.StringToHash("Equipped Item ID");
        private static readonly int ID_ManagedInt1 = Animator.StringToHash("ManagedInt1");
        private static readonly int ID_ManagedInt2 = Animator.StringToHash("ManagedInt2");
        private static readonly int ID_ManagedFloat1 = Animator.StringToHash("ManagedFloat1");
        private static readonly int ID_ManagedFloat2 = Animator.StringToHash("ManagedFloat2");
        #endregion

        #region Fields and Properties
        private bool _isGrounded;
        private float _currentSpeed;
        private int _behaviourId;
        private int _equippedItemId;
        private int _usingItemId;
        private int _managedInt1;
        private int _managedInt2;
        private float _managedFloat1;
        private float _managedFloat2;
        private readonly Animator _animator;

        /// <summary> Is the Agent moving </summary>
        public bool IsMoving { get; private set; }

        /// <summary> Is the Agent touching the ground </summary>
        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                _animator.SetBool(ID_IsGrounded, value);
                _isGrounded = value;
            }
        }

        /// <summary> Movement speed of Agent </summary>
        public float Speed
        {
            get => this._currentSpeed;
            set
            {
                if (value > MinMoveSpeed)
                {
                    _animator.SetBool(ID_IsMoving, true);
                    _animator.SetFloat(ID_Speed, value);
                    IsMoving = true;
                    _currentSpeed = value;
                }
                else
                {
                    _animator.SetBool(ID_IsMoving, false);
                    _animator.SetFloat(ID_Speed, 0f);
                    IsMoving = false;
                    _currentSpeed = 0f;
                }
            }
        }

        /// <summary> Behaviour Index of currently enabled behaviour </summary>
        public int BehaviourId
        {
            get => _behaviourId;
            set
            {
                _animator.SetInteger(ID_BehaviourId, value);
                _behaviourId = value;
            }
        }
        
        /// <summary>
        /// ID of currently equipped ID.
        /// This will be set for 2 frames only. Because multiple items can be enabled at once.
        /// </summary>
        public int EquippedItemId
        {
            get => _equippedItemId;
            set
            {
                _animator.SetInteger(ID_EquippedId, value);
                _equippedItemId = value;
            }
        }
        
        /// <summary>
        ///  ID of currently equipped ID.
        /// This will be set for 2 frames only. Because multiple items can be enabled at once.
        /// </summary>
        public int UsingItemId
        {
            get => _usingItemId;
            set
            {
                _animator.SetInteger(ID_UsingEquipped, value);
                _usingItemId = value;
            }
        }
        
        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt1
        {
            get => _managedInt1;
            set
            {
                _animator.SetInteger(ID_ManagedInt1, value);
                _managedInt1 = value;
            }
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt2
        {
            get => _managedInt2;
            set
            {
                _animator.SetFloat(ID_ManagedInt2, value);
                _managedInt2 = value;
            }
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat1
        {
            get => _managedFloat1;
            set
            {
                _animator.SetFloat(ID_ManagedFloat1, value);
                _managedFloat1 = value;
            }
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat2
        {
            get => _managedFloat2;
            set
            {
                _animator.SetFloat(ID_ManagedFloat2, value);
                _managedFloat2 = value;
            }
        }
        #endregion
        
        public AnimatorHandler(Animator animator)
        {
            _animator = animator;
            _currentSpeed = 0f;
            IsMoving = false;
            animator.SetFloat(ID_Speed, 0f);
            animator.SetBool(ID_IsMoving, false);
        }
    }
}