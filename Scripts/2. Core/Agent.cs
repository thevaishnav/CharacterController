using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omnix.CCN.InputSystemWrapper;
using UnityEngine;

namespace Omnix.CCN.Core
{
    [RequireComponent(typeof(CharacterController))]
    [DefaultExecutionOrder(-100)]
    public class Agent : MonoBehaviour
    {
        #region Events
        /// <summary> Event fired when the Agent script is enabled </summary>
        public event Action EvEnabled;

        /// <summary> Event fired when the Agent script is disabled </summary>
        public event Action EvDisabled;

        /// <summary> Event fired every Update </summary>
        public event Action EvUpdate;

        /// <summary> Event fired every FixedUpdate </summary>
        public event Action EvFixedUpdate;

        /// <summary> Event fired every OnTriggerEntered </summary>
        public event Action<Collider> EvTriggerEntered;

        /// <summary> Event fired every OnTriggerStay </summary>
        public event Action<Collider> EvTriggerStay;

        /// <summary> Event fired every OnTriggerExit </summary>
        public event Action<Collider> EvTriggerExit;

        /// <summary> Event fired every OnCollisionEnter </summary>
        public event Action<Collision> EvCollisionEnter;

        /// <summary> Event fired every OnCollisionStay </summary>
        public event Action<Collision> EvCollisionStay;

        /// <summary> Event fired every OnCollisionExit </summary>
        public event Action<Collision> EvCollisionExit;
        #endregion

        #region Serialized Fields
        [Header("References")] [SerializeField, Tooltip("Agent Animator")]
        private Animator animator;

        [SerializeField, Tooltip("Agent Camera")]
        private Transform mCamera;

        [SerializeField] private InteractionProfileBase moveProfile;
        
        [Header("Controls")] [SerializeField, Tooltip("Default Movement speed of the Agent")]
        private float defaultMoveSpeed = 5f;

        [SerializeField, Tooltip("How fast the Agent is allowed to turn")]
        private float turnTime = 0.1f;

        [SerializeField, Tooltip("Should the cursor be visible")]
        private bool hideCursor;

        [SerializeField, Tooltip("If true Agent will always look in the direction of camera")]
        private bool rotateAgentWithCamera = true;

        [Header("Physics")] [SerializeField, Tooltip("Physics layer for ground check")]
        private LayerMask groundLayer;

        [SerializeField, Tooltip("Ground distance form the center of Agent GameObject")]
        private Vector3 groundOffset;

        [SerializeField, Tooltip("Radius of an imaginary sphere, if the ground is inside this sphere then Agent is considered to be grounded")]
        private float checkRadius = 0.4f;

        [SerializeField, Tooltip("Force of gravity applied on Agent every frame")]
        public Vector3 gravity = new Vector3(0f, -9.8f, 0f);

        [SerializeField, Tooltip("Mass of Agent")]
        public float mass = 100f;

        [SerializeReference, Tooltip("Behaviours this Agent has")]
        private AgentBehaviour[] behaviors;

        [SerializeField] 
        private List<AgentItem> items; 
        #endregion

        #region Private Fields
        /// <summary> Unity's built in CharacterController that is used to handle physics </summary>
        private CharacterController _buildInController;

        /// <summary> Animator handler, this object will handle all the animation parameters </summary>
        private AnimatorHandler _animatorHandler;

        private Dictionary<ItemSlot, AgentItem> _equippedItems;

        /// <summary> Angle by which the Agent is rotated in this frame </summary>
        private Vector3 _frameRotDelta;

        /// <summary> Behaviour that is currently enabled </summary>
        private AgentBehaviour _currentBehaviour;

        /// <summary> Actual movement speed of the Agent </summary>
        private float _moveSpeed;

        /// <summary> Intermediate variable used to smoothen agent rotation </summary>
        private float _turnSmoothVelocity;

        /// <summary> Current Acceleration of the Agent </summary>
        private Vector3 _acceleration;

        /// <summary> Current Velocity of the Agent </summary>
        private Vector3 _velocity;

        public Vector3 GroundCheckPosition => transform.TransformPoint(groundOffset);
        #endregion

        #region Public Fields
        /// <summary> Current Acceleration of the Agent </summary>
        public Vector3 Acceleration => _acceleration;

        /// <summary> Current Velocity of the Agent </summary>
        public Vector3 Velocity => _velocity;

        /// <summary> Current speed of the Agent </summary>
        public float Speed => _animatorHandler.Speed;

        /// <summary> Is Agent moving </summary>
        public bool IsMoving => _animatorHandler.IsMoving;

        /// <summary> Is Agent touching the ground </summary>
        public bool IsGrounded => _animatorHandler.IsGrounded;

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt1
        {
            get => _animatorHandler.ManagedInt1;
            set => _animatorHandler.ManagedInt1 = value;
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt2
        {
            get => _animatorHandler.ManagedInt2;
            set => _animatorHandler.ManagedInt2 = value;
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat1
        {
            get => _animatorHandler.ManagedFloat1;
            set => _animatorHandler.ManagedFloat1 = value;
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat2
        {
            get => _animatorHandler.ManagedFloat2;
            set => _animatorHandler.ManagedFloat2 = value;
        }

        public int BehaviourId => _animatorHandler.BehaviourId;
        #endregion

        #region Unity
        private void OnEnable() => EvEnabled?.Invoke();
        private void OnDisable() => EvDisabled?.Invoke();
        private void OnTriggerEnter(Collider other) => EvTriggerEntered?.Invoke(other);
        private void OnTriggerStay(Collider other) => EvTriggerStay?.Invoke(other);
        private void OnTriggerExit(Collider other) => EvTriggerExit?.Invoke(other);
        private void OnCollisionEnter(Collision collision) => EvCollisionEnter?.Invoke(collision);
        private void OnCollisionStay(Collision collision) => EvCollisionStay?.Invoke(collision);
        private void OnCollisionExit(Collision collision) => EvCollisionExit?.Invoke(collision);
        
        private void Awake()
        {
            _moveSpeed = defaultMoveSpeed;
            _acceleration = Vector3.zero;
            _velocity = Vector3.zero;
            _animatorHandler = new AnimatorHandler(animator);
            _equippedItems = new Dictionary<ItemSlot, AgentItem>();
        }

        private void Start()
        {
            _buildInController = GetComponent<CharacterController>();
            if (hideCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
            foreach (AgentBehaviour behaviour in behaviors)
            {
                behaviour.Init(this);
            }

            foreach (AgentItem item in items)
            {
                item.Init(this);
            }
            
            UpdateMovementSpeed();
        }

        private void FixedUpdate()
        {
            EvFixedUpdate?.Invoke();
            _animatorHandler.IsGrounded = Physics.CheckSphere(GroundCheckPosition, checkRadius, groundLayer);

            if (_animatorHandler.IsGrounded && _velocity.y < 0) _velocity.y = -2f;

            _velocity += _acceleration * Time.fixedDeltaTime;
            _acceleration = gravity;

            Vector3 pos = transform.position;
            _buildInController.Move(_velocity * Time.fixedDeltaTime); // Distance moved may not be same as the distance provided here, if the agent is held back by a collider
            // ReSharper disable once Unity.InefficientPropertyAccess
            _animatorHandler.Speed = (transform.position - pos).magnitude / Time.fixedDeltaTime;

            if (_frameRotDelta.sqrMagnitude > 0.01f)
            {
                transform.Rotate(_frameRotDelta);
                _frameRotDelta = Vector3.zero;
            }
        }

        private void Update()
        {
            EvUpdate?.Invoke();
            MoveByInputs();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = GroundCheckPosition;
            Gizmos.DrawWireSphere(center, checkRadius);
            Gizmos.DrawSphere(center, 0.005f);
        }
        #endregion

        #region Movements
        /// <summary> Add a force to Agent GameObject </summary>
        /// <param name="value"> Amount of force to be added </param>
        /// <param name="forceMode"> Nature of force to be added </param>
        public void AddForce(Vector3 value, ForceMode forceMode = ForceMode.Force)
        {
            if (value.sqrMagnitude <= AnimatorHandler.SqreMinMoveSpeed) return;
            switch (forceMode)
            {
                case ForceMode.Force:
                    _acceleration += value / mass;
                    break;
                case ForceMode.Acceleration:
                    _acceleration += value;
                    break;
                case ForceMode.Impulse:
                    _velocity += value / mass;
                    break;
                case ForceMode.VelocityChange:
                    _velocity += value;
                    break;
            }
        }

        /// <summary> Add a torque to Agent GameObject </summary>
        /// <param name="value"> Amount of torque to be added </param>
        /// <param name="forceMode"> Nature of torque to be added </param>
        public void AddTorque(Vector3 value, ForceMode forceMode = ForceMode.Force)
        {
            switch (forceMode)
            {
                case ForceMode.Force:
                    _frameRotDelta += value / mass;
                    break;
                case ForceMode.Acceleration:
                    _frameRotDelta += value;
                    break;
                case ForceMode.Impulse:
                    _acceleration += value / (mass * Time.deltaTime); // divide by Time.deltaTime here so as to make this force FrameRate Independent (Remember will multiply it by Time.deltaTime to calculate Velocity)
                    break;
                case ForceMode.VelocityChange:
                    _frameRotDelta += value;
                    break;
            }
        }

        /// <summary> Move the Agent GameObject according to inputs bu user </summary>
        private void MoveByInputs()
        {
            Vector2 d = moveProfile.GetAxisValue();

            float targetAngle = Mathf.Atan2(d.x, d.y) * Mathf.Rad2Deg;
            if (rotateAgentWithCamera)
            {
                if (rotateAgentWithCamera) targetAngle += mCamera.eulerAngles.y;
                
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            
            
            if (d.sqrMagnitude > 0.1f)
            {
                Vector3 move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * _moveSpeed;
                _velocity.x = move.x;
                _velocity.z = move.z;
            }
            else
            {
                _velocity.x = 0;
                _velocity.z = 0;
                _animatorHandler.Speed = 0f;
            }
        }
        
        /// <summary> Update actual move speed of agent </summary>
        private void UpdateMovementSpeed()
        {
            _moveSpeed = defaultMoveSpeed;
            if (_currentBehaviour != null) _moveSpeed *= _currentBehaviour.MoveSpeedMultiplier;

            foreach (KeyValuePair<ItemSlot, AgentItem> pair in _equippedItems)
            {
                if (pair.Value != null)
                {
                    _moveSpeed *= pair.Value.MoveSpeedMultiplier;
                }
            }
        }
        #endregion

        #region Behaviour
        /// <summary> Set state of an behaviour </summary>
        /// <param name="behaviour"> Behaviour of which state is to be set </param>
        /// <param name="value"> Should this behaviour be enabled or disabled </param>
        private void SetBehaviourActiveInner(AgentBehaviour behaviour, bool value)
        {
            if (value)
            {
                _currentBehaviour = behaviour;
                _animatorHandler.BehaviourId = behaviour.ID;
            }
            else
            {
                _currentBehaviour = null;
                _animatorHandler.BehaviourId = 0;
            }

            behaviour.DoSetState(value);
            UpdateMovementSpeed();
        }

        /// <summary> Checks if the behaviour is loaded to be used </summary>
        /// <typeparam name="T"> Type of behaviour to be checked </typeparam>
        /// <returns> true if the behaviour is loaded </returns>
        public bool HasBehavior<T>() where T : AgentBehaviour => GetBehavior<T>() != null;

        /// <summary> Checks if the behaviour is loaded to be used </summary>
        /// <param name="t"> Type of behaviour to be checked </param>
        /// <returns> true if the behaviour is loaded </returns>
        public bool HasBehavior(Type t) => GetBehavior(t) != null;

        /// <summary> Checks if the behaviour is loaded to be used </summary>
        /// <param name="behaviour">Behaviour to be checked</param>
        /// <returns> true if the behaviour is loaded </returns>
        public bool HasBehavior(AgentBehaviour behaviour) => GetBehavior(behaviour.GetType()) == behaviour;

        /// <summary> Try to enable an behaviour. </summary>
        /// <param name="behaviour"> Behaviour to be enabled </param>
        /// <param name="force"> Should this behaviour be enabled even if the currently enabled behaviour blocks this behaviour enable. </param>
        /// <returns> true if the behaviour was enabled </returns>
        public bool TryEnableBehaviour(AgentBehaviour behaviour, bool force = false)
        {
            if (!this.HasBehavior(behaviour)) return false;

            bool currentBehaviourIsNull = _currentBehaviour == null;
            if (force || currentBehaviourIsNull || _currentBehaviour.ShouldBlockBehaviorStart(behaviour) == false)
            {
                if (currentBehaviourIsNull == false) SetBehaviourActiveInner(_currentBehaviour, false);
                SetBehaviourActiveInner(behaviour, true);
                return true;
            }

            return false;
        }

        /// <summary> Try to disable an behaviour. </summary>
        /// <param name="behaviour">Behaviour to be disable</param>
        /// <param name="force"> Should this behaviour be disabled even if the currently enabled behaviour blocks this behaviour disable. </param>
        /// <returns> true if the behaviour was disabled </returns>
        public bool TryDisableBehavior(AgentBehaviour behaviour, bool force = false)
        {
            if (!HasBehavior(behaviour) || !behaviour.IsEnabled) return false;

            if (force || (_currentBehaviour != null && !_currentBehaviour.ShouldBlockBehaviorStop(behaviour)))
            {
                SetBehaviourActiveInner(behaviour, false);
                return true;
            }

            return false;
        }

        /// <summary> Get an behaviour of the given type </summary>
        /// <typeparam name="T"> type of behaviour </typeparam>
        /// <returns> behaviour instance if that behaviour is loaded, null otherwise </returns>
        public T GetBehavior<T>() where T : AgentBehaviour
        {
            foreach (AgentBehaviour behaviour in behaviors)
            {
                if (behaviour is T abiT) return abiT;
            }

            return null;
        }

        /// <summary> Get an behaviour of the given type </summary>
        /// <param name="behaviorType"> type of behaviour </param>
        /// <returns> Behaviour instance if that behaviour is loaded, null if the behaviour is not loaded of given type if not subclass of <see cref="AgentBehaviour"/> </returns>
        public AgentBehaviour GetBehavior(Type behaviorType)
        {
            if (!behaviorType.IsSubclassOf(typeof(AgentBehaviour))) return null;
            if (behaviors == null || behaviors.Length == 0) return null;

            foreach (AgentBehaviour behaviour in behaviors)
            {
                if (behaviour == null) continue;
                if (behaviour.GetType() == behaviorType)
                {
                    return behaviour;
                }
            }

            return null;
        }

        /// <summary> Try Get an behaviour of the given type </summary>
        /// <param name="behavior"> output parameter, this will be set to behaviour instance </param>
        /// <typeparam name="T"> type of behaviour </typeparam>
        /// <returns> true if behaviour is found, null otherwise </returns>
        public bool TryGetBehavior<T>(out T behavior) where T : AgentBehaviour
        {
            behavior = GetBehavior<T>();
            return behavior == null;
        }

        /// <summary> Try Get an behaviour of the given type </summary>
        /// <param name="behaviorType"> type of behaviour </param>
        /// <param name="behaviour"> output parameter, this will be set to behaviour instance </param>
        /// <returns> true if behaviour is found, null otherwise </returns>
        public bool TryGetBehavior(Type behaviorType, out AgentBehaviour behaviour)
        {
            behaviour = GetBehavior(behaviorType);
            return behaviour == null;
        }
        #endregion

        #region Items
       
        /// <summary> Unequip one item and equip another. </summary>
        /// <remarks> Assumes that both belong to same slot</remarks>
        private IEnumerator EquipItem([CanBeNull] AgentItem equip, [CanBeNull] AgentItem unequip)
        {
            if (unequip != null)
            {
                _animatorHandler.EquippedItemId = -unequip.ID;
                _equippedItems.Remove(unequip.Slot);
                if (unequip.UnequipAnimationDuration >= 0.001f) yield return new WaitForSeconds(unequip.UnequipAnimationDuration);
            }

            if (equip != null)
            {
                _animatorHandler.EquippedItemId = equip.ID;
                _equippedItems[equip.Slot] = equip;
                if (equip.EquipAnimationDuration >= 0.001f) yield return new WaitForSeconds(equip.EquipAnimationDuration);    
            }
            _animatorHandler.EquippedItemId = 0;
        }

        public void RegisterItem(AgentItem item)
        {
            item.enabled = false;
            items.Add(item);
        }

        public IEnumerable<AgentItem> GetAllItemsForSlot(ItemSlot slot)
        {
            foreach (AgentItem item in items)
            {
                if (item == null) continue;
                if (item.Slot == slot)
                {
                    yield return null;
                }
            }
            yield break;
        }

        public bool TryGetItem(ItemSlot slot, Type type, out AgentItem item)
        {
            foreach (AgentItem tempItem in GetAllItemsForSlot(slot))
            {
                if (tempItem.GetType() == type)
                {
                    item = tempItem;
                    return true;
                }
            }

            item = null;
            return false;
        }

        public bool TryGetItem<T>(ItemSlot slot, out T item)
        where T : AgentItem
        {
            Type type = typeof(T);
            foreach (AgentItem tempItem in GetAllItemsForSlot(slot))
            {
                if (tempItem.GetType() == type)
                {
                    item = tempItem as T;
                    return item != null;
                }
            }

            item = null;
            return false;
        }
        
        [CanBeNull]
        public AgentItem GetEquippedItem(ItemSlot slot)
        {
            if (_equippedItems.TryGetValue(slot, out AgentItem item)) return item;
            return null;
        }

        public bool TryGetEquippedItem(ItemSlot slot, out AgentItem item)
        {
            return _equippedItems.TryGetValue(slot, out item) && item != null;
        }

        public bool IsSpotOccupied(ItemSlot slot)
        {
            return TryGetEquippedItem(slot, out AgentItem _);
        }
        
        public bool IsSpotEmpty(ItemSlot slot)
        {
            return !TryGetEquippedItem(slot, out AgentItem _);
        }
        
        public bool TryEquipItem(AgentItem item, bool force = false)
        {
            if (item == null) return false;
            if (force == false && item.CanEquip() == false) return false;

            bool hasItem = TryGetEquippedItem(item.Slot, out AgentItem alreadyAtSpot);
            if (force == false && hasItem && alreadyAtSpot.CanUnequip() == false)
            {
                return false;
            }

            StartCoroutine(EquipItem(item, alreadyAtSpot));
            return true;
        }

        public bool TryUnequipSpot(ItemSlot slot, bool force = false)
        {
            bool hasItem = TryGetEquippedItem(slot, out AgentItem item);
            if (hasItem == false) return false;
            if (force == false && item.CanUnequip() == false) return false;

            StartCoroutine(EquipItem(null, item));
            return true;
        }
        
        public bool TryUnequipItem(AgentItem item, bool force = false)
        {
            bool hasItem = TryGetEquippedItem(item.Slot, out AgentItem alreadyAtSpot);
            if (hasItem == false || item != alreadyAtSpot) return false;
            if (force == false && item.CanUnequip() == false) return false;

            StartCoroutine(EquipItem(null, alreadyAtSpot));
            return true;
        }

        public bool StartItemUse(AgentItem item)
        {
            if (item == null || item.IsEquipped == false) return false;
            if (_equippedItems.TryGetValue(item.Slot, out AgentItem equipped) == false) return false;
            if (equipped != item) return false;
            
            _animatorHandler.UsingItemId = item.ID;
            return true;
        }
        
        public bool StopItemUse(AgentItem item)
        {
            if (item == null || item.IsEquipped == false) return false;
            if (_equippedItems.TryGetValue(item.Slot, out AgentItem equipped) == false) return false;
            if (equipped != item) return false;

            if (_animatorHandler.UsingItemId == item.ID)
            {
                _animatorHandler.UsingItemId = 0;
            } 
            return true;
        }
        #endregion
    }
}