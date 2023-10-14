using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCN.Core
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
        #endregion

        #region Private Fields
        /// <summary> Unity's built in CharacterController that is used to handle physics </summary>
        private CharacterController _buildInController;

        /// <summary> Animator handler, this object will handle all the animation parameters </summary>
        private AnimatorHandler _animatorHandler;

        // private Dictionary<ItemSpot, Item> _equippedItems;

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

        #region Unity Callback
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
            // _equippedItems = new Dictionary<ItemSpot, Item>();

            foreach (AgentBehaviour behaviour in behaviors)
            {
                behaviour.Init(this);
            }

            UpdateMovementSpeed();
        }

        private void Start()
        {
            _buildInController = GetComponent<CharacterController>();
            if (hideCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
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
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (direction.sqrMagnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                if (rotateAgentWithCamera) targetAngle += mCamera.eulerAngles.y;

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

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

            /*foreach (KeyValuePair<ItemSpot, Item> pair in _equippedItems)
            {
                if (pair.Value != null)
                {
                    _moveSpeed *= pair.Value.MoveSpeedMultiplier;
                }
            }*/
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

        /*#region Items
        private IEnumerator ResetUseAnimatorParameter(int expectedValue)
        {
            yield return null;
            yield return null;
            if (_animatorHandler.UsingItemId == expectedValue)
            {
                _animatorHandler.UsingItemId = 0;
            }
        }

        private IEnumerator EquipItem(Item item1, Item item, bool force)
        {
            Item alreadyEquippedAtSpot;
            if (force)
            {
                if (_equippedItems.TryGetValue(item.Spot, out alreadyEquippedAtSpot))
                {
                    if (alreadyEquippedAtSpot != null) alreadyEquippedAtSpot.OnUnequip();
                    item.OnEquip();
                    _equippedItems[item.Spot] = item;
                }
                else
                {
                    item.OnEquip();
                    _equippedItems.Add(item.Spot, item);
                }

                UpdateMovementSpeed();
                return true;
            }

            if (_equippedItems.TryGetValue(item.Spot, out alreadyEquippedAtSpot))
            {
                if (alreadyEquippedAtSpot != null && alreadyEquippedAtSpot.CanUnequip() == false) return false;

                alreadyEquippedAtSpot.OnUnequip();
                item.OnEquip();
                _equippedItems[item.Spot] = item;

                UpdateMovementSpeed();
                return true;
            }
            else
            {
                item.OnEquip();
                _equippedItems.Add(item.Spot, item);

                UpdateMovementSpeed();
                return true;
            }
        }

        public Item GetItem(ItemSpot spot)
        {
            if (_equippedItems.TryGetValue(spot, out Item item)) return item;
            return null;
        }

        public bool TryGetItem(ItemSpot spot, out Item item)
        {
            return _equippedItems.TryGetValue(spot, out item) && item != null;
        }

        public bool IsSpotOccupied(ItemSpot spot)
        {
            return _equippedItems.TryGetValue(spot, out Item item) && item != null;
        }
        
        public bool IsSpotEmpty(ItemSpot spot)
        {
            return !IsSpotOccupied(spot);
        }
        
        /*public void SetItemActiveInner(Item item, bool value)
        {
            if (value)
            {
                _equippedItems[item.Spot] = item;
                _animatorHandler.EquippedItemId = item.ID;
                StartCoroutine(ResetUseAnimatorParameter(true, item.ID));
            }
            else
            {
                _equippedItems.Remove(item.Spot);
                _animatorHandler.EquippedItemId = -item.ID;
                StartCoroutine(ResetUseAnimatorParameter(true, -item.ID));
            }

            item.DoSetState(value);
            UpdateMovementSpeed();
        }#1#

        public bool TryEquipItem(Item item, bool force = false)
        {
            _equippedItems.TryGetValue(item.Spot, out Item alreadyEquippedAtSpot);
            StartCoroutine(EquipItem(item, alreadyEquippedAtSpot, force));
            return force || alreadyEquippedAtSpot == null || alreadyEquippedAtSpot.CanUnequip();
        }

        public bool TryUnequipItem(Item item, bool force = false)
        {
            return true;
        }

        public void UseItem(Item item)
        {
            if (item.IsEnabled && _equippedItems.TryGetValue(item.Spot, out Item equipped) && equipped == item)
            {
                item.OnUse();
                _animatorHandler.UsingItemId = item.ID;
                StartCoroutine(ResetUseAnimatorParameter(false, item.ID));
            }
        }
        #endregion*/
    }
}