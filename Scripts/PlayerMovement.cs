using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KS.CharaCon
{
    [RequireComponent(typeof(CharacterController))]
    [DefaultExecutionOrder(-100)]
    public class PlayerMovement : MonoBehaviour
    {
        #region Events
        /// <summary> Event fired when the PlayerMovement script is enabled </summary>
        internal event Action EvEnabled;

        /// <summary> Event fired when the PlayerMovement script is disabled </summary>
        internal event Action EvDisabled;

        /// <summary> Event fired every Update </summary>
        internal event Action EvUpdate;

        /// <summary> Event fired every FixedUpdate </summary>
        internal event Action EvFixedUpdate;

        /// <summary> Event fired every LateUpdate </summary>
        internal event Action EvLateUpdate;

        /// <summary> Event fired every OnTriggerEntered </summary>
        internal event Action<Collider> EvTriggerEntered;

        /// <summary> Event fired every OnTriggerStay </summary>
        internal event Action<Collider> EvTriggerStay;

        /// <summary> Event fired every OnTriggerExit </summary>
        internal event Action<Collider> EvTriggerExit;

        /// <summary> Event fired every OnCollisionEnter </summary>
        internal event Action<Collision> EvCollisionEnter;

        /// <summary> Event fired every OnCollisionStay </summary>
        internal event Action<Collision> EvCollisionStay;

        /// <summary> Event fired every OnCollisionExit </summary>
        internal event Action<Collision> EvCollisionExit;
        #endregion

        #region Serialized Fields
        [Header("Controls")] [SerializeField, Tooltip("Default Movement speed of the player, Note that if the enabled ability defines value for \"targetSpeed\" then that value will be used")]
        private float moveSpeed = 5f;

        [SerializeField, Tooltip("Turn time of the player, how fast the player is allowed to turn")]
        private float turnTime = 0.1f;

        /// <summary> Mass used by physics system </summary>
        [SerializeField, Tooltip("Mass of player")]
        public float Mass = 100f;

        [SerializeField, Tooltip("Should the cursor be visible")]
        private bool hideCursor;

        [SerializeField, Tooltip("If true player will always look in the direction of camera")]
        private bool rotatePlayerWithCamera = true;

        /// <summary> Gravity used by physics system </summary>
        [SerializeField, Tooltip("Force of gravity applied on player every frame")]
        public Vector3 Gravity = new Vector3(0f, -9.8f, 0f);

        [Header("Ground Check")] [SerializeField, Tooltip("Physics layer for ground check")]
        private LayerMask groundLayer;

        [SerializeField, Tooltip("Ground distance form the center of player GameObject")]
        private Vector3 groundOffset;

        [SerializeField, Tooltip("Radius of an imaginary sphere, if the ground is inside this sphere then player is considered to be grounded")]
        private float checkRadius = 0.4f;

        [Header("References")] [SerializeField, Tooltip("Player Animator")]
        private Animator animator;

        [SerializeField, Tooltip("Player Camera")]
        private Transform mCamera;

        [SerializeReference, Space(20f), Tooltip("Abilities this player has")]
        private Ability[] abilities;
        #endregion

        #region Private Fields
        /// <summary> Unity's built in CharacterController that is used to handle physics </summary>
        private CharacterController _buildInController;

        /// <summary> Animator handler, this object will handle all the animation parameters </summary>
        private AnimatorHandler _animatorHandler;

        /// <summary> Angle by which the player is rotated in this frame </summary>
        private Vector3 _frameRotDelta;

        /// <summary> Ability that is currently enabled </summary>
        private Ability _currentAbility;

        /// <summary> Actual movement speed of the player </summary>
        private float _moveSpeed;

        /// <summary> Intermediate variable used to smoothen character rotation </summary>
        private float _turnSmoothVelocity;

        /// <summary> Current Acceleration of the player </summary>
        private Vector3 _acceleration;

        /// <summary> Current Velocity of the player </summary>
        private Vector3 _velocity;

        public Vector3 GroundCheckPosition => transform.TransformPoint(groundOffset);
        #endregion

        #region Public Fields
        /// <summary> Current Acceleration of the player </summary>
        public Vector3 Acceleration => _acceleration;

        /// <summary> Current Velocity of the player </summary>
        public Vector3 Velocity => _velocity;

        /// <summary> Current speed of the player </summary>
        public float Speed => _animatorHandler.Speed;

        /// <summary> Is player moving </summary>
        public bool IsMoving => _animatorHandler.IsMoving;

        /// <summary> Is player touching the ground </summary>
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

        public int AbilityIndex => _animatorHandler.AbilityIndex;
        #endregion

        #region Unity Callback
        private void OnEnable() => EvEnabled?.Invoke();
        private void OnDisable() => EvDisabled?.Invoke();

        private void Awake()
        {
            _acceleration = Vector3.zero;
            _velocity = Vector3.zero;
            _animatorHandler = new AnimatorHandler(animator);

            _moveSpeed = float.MinValue;
            foreach (Ability ability in abilities)
            {
                ability.Init(this);
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
        }

        private void Update()
        {
            EvUpdate?.Invoke();
            MoveByInputs();
        }

        private void LateUpdate()
        {
            EvLateUpdate?.Invoke();

            if (_animatorHandler.IsGrounded && _velocity.y < 0) _velocity.y = -2f;

            _velocity += _acceleration * Time.deltaTime;
            _acceleration = Gravity;

            Vector3 pos = transform.position;
            _buildInController.Move(_velocity * Time.deltaTime);
            _animatorHandler.Speed = (transform.position - pos).magnitude / Time.deltaTime;

            if (_frameRotDelta.sqrMagnitude > 0.01f)
            {
                transform.Rotate(_frameRotDelta);
                _frameRotDelta = Vector3.zero;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = GroundCheckPosition;
            Gizmos.DrawWireSphere(center, checkRadius);
            Gizmos.DrawSphere(center, 0.005f);
        }

        private void OnTriggerEnter(Collider other) => EvTriggerEntered?.Invoke(other);
        private void OnTriggerStay(Collider other) => EvTriggerStay?.Invoke(other);
        private void OnTriggerExit(Collider other) => EvTriggerExit?.Invoke(other);
        private void OnCollisionEnter(Collision collision) => EvCollisionEnter?.Invoke(collision);
        private void OnCollisionStay(Collision collision) => EvCollisionStay?.Invoke(collision);
        private void OnCollisionExit(Collision collision) => EvCollisionExit?.Invoke(collision);
        #endregion

        #region Methods
        /// <summary> Move the player GameObject according to inputs bu user </summary>
        private void MoveByInputs()
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (direction.sqrMagnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                if (rotatePlayerWithCamera) targetAngle += mCamera.eulerAngles.y;

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

        /// <summary> Set state of an ability </summary>
        /// <param name="ability"> Ability of which state is to be set </param>
        /// <param name="value"> Should this ability be enabled or disabled </param>
        private void SetAbilityActiveInner(Ability ability, bool value)
        {
            if (value)
            {
                _currentAbility = ability;
                _animatorHandler.AbilityIndex = ability.AbilityIndex;
            }
            else
            {
                _currentAbility = null;
                _animatorHandler.AbilityIndex = 0;
            }

            ability.DoEnable(value);
            UpdateMovementSpeed();
        }

        /// <summary> Update <see cref="_moveSpeed"/> variable </summary>
        private void UpdateMovementSpeed()
        {
            if (_currentAbility != null && _currentAbility.TargetSpeed > AnimatorHandler.MinMoveSpeed)
            {
                _moveSpeed = _currentAbility.TargetSpeed;
            }
            else
            {
                _moveSpeed = moveSpeed;
            }
        }

        /// <summary> Checks if the ability is loaded to be used </summary>
        /// <typeparam name="T"> Type of ability to be checked </typeparam>
        /// <returns> true if the ability is loaded </returns>
        public bool HasAbility<T>() where T : Ability => GetAbility<T>() != null;

        /// <summary> Checks if the ability is loaded to be used </summary>
        /// <param name="t"> Type of ability to be checked </param>
        /// <returns> true if the ability is loaded </returns>
        public bool HasAbility(Type t) => GetAbility(t) != null;

        /// <summary> Checks if the ability is loaded to be used </summary>
        /// <param name="ability">Ability to be checked</param>
        /// <returns> true if the ability is loaded </returns>
        public bool HasAbility(Ability ability) => GetAbility(ability.GetType()) == ability;

        /// <summary> Add a force to player GameObject </summary>
        /// <param name="value"> Amount of force to be added </param>
        /// <param name="forceMode"> Nature of force to be added </param>
        public void AddForce(Vector3 value, ForceMode forceMode = ForceMode.Force)
        {
            if (value.sqrMagnitude <= AnimatorHandler.SqreMinMoveSpeed) return;
            switch (forceMode)
            {
                case ForceMode.Force:
                    _acceleration += value / Mass;
                    break;
                case ForceMode.Acceleration:
                    _acceleration += value;
                    break;
                case ForceMode.Impulse:
                    _acceleration += value / (Mass * Time.deltaTime); // divide by Time.deltaTime here so as to make this force FrameRate Independent (Remember will multiply it by Time.deltaTime to calculate Velocity)
                    break;
                case ForceMode.VelocityChange:
                    _velocity += value;
                    break;
            }
        }

        /// <summary> Add a torque to player GameObject </summary>
        /// <param name="value"> Amount of torque to be added </param>
        /// <param name="forceMode"> Nature of torque to be added </param>
        public void AddTorque(Vector3 value, ForceMode forceMode = ForceMode.Force)
        {
            switch (forceMode)
            {
                case ForceMode.Force:
                    _frameRotDelta += value / Mass;
                    break;
                case ForceMode.Acceleration:
                    _frameRotDelta += value;
                    break;
                case ForceMode.Impulse:
                    _acceleration += value / (Mass * Time.deltaTime); // divide by Time.deltaTime here so as to make this force FrameRate Independent (Remember will multiply it by Time.deltaTime to calculate Velocity)
                    break;
                case ForceMode.VelocityChange:
                    _frameRotDelta += value;
                    break;
            }
        }

        /// <summary> Try to enable an ability. </summary>
        /// <param name="ability"> Ability to be enabled </param>
        /// <param name="force"> Should this ability be enabled even if the currently enabled ability blocks this ability enable. </param>
        /// <returns> true if the ability was enabled </returns>
        public bool TryEnableAbility(Ability ability, bool force = false)
        {
            if (!this.HasAbility(ability)) return false;

            bool currAbiNul = _currentAbility != null;
            if (force || (currAbiNul && !_currentAbility.ShouldBlockAbilityStart(ability)))
            {
                if (currAbiNul) SetAbilityActiveInner(_currentAbility, false);
                SetAbilityActiveInner(ability, true);
                return true;
            }

            return false;
        }

        /// <summary> Try to disable an ability. </summary>
        /// <param name="ability">Ability to be disable</param>
        /// <param name="force"> Should this ability be disabled even if the currently enabled ability blocks this ability disable. </param>
        /// <returns> true if the ability was disabled </returns>
        public bool TryDisableAbility(Ability ability, bool force = false)
        {
            if (!HasAbility(ability) || !ability.IsEnabled) return false;

            if (force || (_currentAbility != null && !_currentAbility.ShouldBlockAbilityStop(ability)))
            {
                SetAbilityActiveInner(ability, false);
                return true;
            }

            return false;
        }

        /// <summary> Get an ability of the given type </summary>
        /// <typeparam name="T"> type of ability </typeparam>
        /// <returns> ability instance if that ability is loaded, null otherwise </returns>
        public T GetAbility<T>() where T : Ability
        {
            foreach (Ability ability in abilities)
            {
                if (ability is T abiT) return abiT;
            }

            return null;
        }

        /// <summary> Get an ability of the given type </summary>
        /// <param name="abilityType"> type of ability </param>
        /// <returns> Ability instance if that ability is loaded, null if the ability is not loaded of given type if not subclass of <see cref="Ability"/> </returns>
        public Ability GetAbility(Type abilityType)
        {
            if (!abilityType.IsSubclassOf(typeof(Ability))) return null;
            return abilities.FirstOrDefault(ability => ability.GetType() == abilityType);
        }

        /// <summary> Try Get an ability of the given type </summary>
        /// <param name="ability"> output parameter, this will be set to ability instance </param>
        /// <typeparam name="T"> type of ability </typeparam>
        /// <returns> true if ability is found, null otherwise </returns>
        public bool TryGetAbility<T>(out T ability) where T : Ability
        {
            ability = GetAbility<T>();
            return ability == null;
        }

        /// <summary> Try Get an ability of the given type </summary>
        /// <param name="abilityType"> type of ability </param>
        /// <param name="ability"> output parameter, this will be set to ability instance </param>
        /// <returns> true if ability is found, null otherwise </returns>
        public bool TryGetAbility(Type abilityType, out Ability ability)
        {
            ability = GetAbility(abilityType);
            return ability == null;
        }
        #endregion

        #region Editor
        #if UNITY_EDITOR
        /// <summary> EDITOR ONLY METHOD. Load an ability of the given type </summary>
        /// <param name="abilityType"> type of ability </param>
        /// <returns> Tru if the ability was added </returns>
        public bool __EditorModeAddAbility__(Type abilityType)
        {
            if (HasAbility(abilityType)) return false;

            Ability ability = (Ability)Activator.CreateInstance(abilityType);
            ability.__EditorModeCreated__();
            ArrayUtility.Add(ref abilities, ability);
            if (Application.isPlaying) ability.Init(this);
            return true;
        }

        /// <summary> EDITOR ONLY METHOD. Check if an empty ability is loaded and removes if found. </summary>
        /// <returns> true if the empty ability was found </returns>
        public bool __EditorModeRemoveEmptyAbility__()
        {
            int i = 0;
            foreach (Ability ability in abilities)
            {
                if (ability == null)
                {
                    ArrayUtility.RemoveAt(ref abilities, i);
                    return true;
                }

                i++;
            }

            return false;
        }
        #endif
        #endregion
    }
}