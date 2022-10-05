using System;
using System.Collections.Generic;
using System.Linq;
using DebugToScreen;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[DefaultExecutionOrder(-100)]
public class PlayerMovement : MonoBehaviour
{
    [Header("Controls")] 
    [SerializeField] float defaultMoveSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float mass = 100f;
    [SerializeField] bool hideCursor;
    [SerializeField] bool rotatePlayerWithCamera = true;

    [Header("Ground Check")] 
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance = 0.4f;

    [Header("References")] 
    [SerializeField] Animator animator;
    [SerializeField] Transform camera;

    private CharacterController buildInController;
    private AnimatorHandler animatorHandler;
    private Vector3 frameRotDelta;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    
    
    private Dictionary<string, HashSet<Ability>> eventListeners;
    private Ability[] allAbilities;
    private Ability[] keyDownStartAbilities;
    private Ability[] keyUpEndAbilities;
    private Ability currentAbility;
    private float moveSpeed;
    private float turnSmoothVelocity;

    internal Vector3 m_velocity;

    [NonSerialized] public Vector3 Gravity = new Vector3(0f, -9.8f, 0f);

    public Vector3 Acceleration { get; private set; }
    public Vector3 Velocity => m_velocity;
    public float Mass => mass;
    public float Speed => animatorHandler.Speed;
    public bool IsMoving => animatorHandler.IsMoving;
    public bool IsGrounded => animatorHandler.IsGrounded;

    void Awake()
    {
        Acceleration = Vector3.zero;
        m_velocity = Vector3.zero;

        allAbilities = this.GetComponentsInChildren<Ability>();
        animatorHandler = new AnimatorHandler(animator);

        moveSpeed = float.MinValue;
        int startCount = 0;
        int endCount = 0;

        foreach (Ability ability in allAbilities)
        {
            ability.SelfInit(this);
            if (ability.startType == AbilityStartType.Automatic)
            {
                ability.enabled = TryEnableAbility(ability, currentAbility == null);
            }
            else
            {
                ability.enabled = false;
            }

            if (ability.startType == AbilityStartType.KeyDown) startCount++;
            if (ability.endType == AbilityEndType.KeyUp) endCount++;
        }

        keyDownStartAbilities = new Ability[startCount];
        keyUpEndAbilities = new Ability[endCount];

        startCount = 0;
        endCount = 0;
        foreach (Ability ability in allAbilities)
        {
            if (ability.startType == AbilityStartType.KeyDown)
            {
                keyDownStartAbilities[startCount] = ability;
                startCount++;
            }

            if (ability.endType == AbilityEndType.KeyUp)
            {
                keyUpEndAbilities[endCount] = ability;
                endCount++;
            }
        }

        UpdateMovementSpeed();
    }

    void Start()
    {
        buildInController = GetComponent<CharacterController>();
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        GameDebug.StartMyInfo(this, "", "Controller");
    }

    void FixedUpdate()
    {
        animatorHandler.IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    void Update()
    {
        MoveByInputs();

        foreach (Ability ability in keyDownStartAbilities)
        {
            if (!ability.enabled && Input.GetKeyDown(ability.startKeyCode)) SetAbilityActiveInner(ability, true);
        }

        foreach (Ability ability in keyUpEndAbilities)
        {
            if (ability.enabled && Input.GetKeyUp(ability.endKeyCode)) SetAbilityActiveInner(ability, false);
        }

        animatorHandler.UpdateSpeed();
    }

    void LateUpdate()
    {
        if (IsGrounded && m_velocity.y < 0) m_velocity.y = -2f;
        m_velocity += Acceleration * Time.deltaTime;
        if (Input.GetKey(KeyCode.Z)) Debug.Log($"Acc: {Acceleration}, Vel: {m_velocity}");
        Acceleration = Gravity;

        if (m_velocity.sqrMagnitude > AnimatorHandler.SqreMinMoveSpeed)
        {
            buildInController.Move(m_velocity * Time.deltaTime);
            animatorHandler.Speed = m_velocity.magnitude;
        }

        if (frameRotDelta.sqrMagnitude > 0.01f)
        {
            transform.Rotate(frameRotDelta);
            frameRotDelta = Vector3.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (this.groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheck.position, groundDistance);
        }
    }

    void MoveByInputs()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
        if (direction.sqrMagnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            if (rotatePlayerWithCamera) targetAngle += camera.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * moveSpeed;
            m_velocity.x = move.x;
            m_velocity.z = move.z;
        }
        else
        {
            m_velocity.x = 0;
            m_velocity.z = 0;
            animatorHandler.Speed = 0f;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    void SetAbilityActiveInner(Ability ability, bool value)
    {
        if (value)
        {
            this.currentAbility = ability;
            this.animatorHandler.AbilityIndex = ability.abilityIndex;
        }
        else
        {
            this.currentAbility = null;
            this.animatorHandler.AbilityIndex = 0;
        }

        ability.enabled = value;
        UpdateMovementSpeed();
    }

    void UpdateMovementSpeed()
    {
        if (currentAbility != null && currentAbility.movementSpeed > AnimatorHandler.MinMoveSpeed)
        {
            moveSpeed = currentAbility.movementSpeed;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }
    }

    public void AddForce(Vector3 force)
    {
        if (force.sqrMagnitude <= AnimatorHandler.SqreMinMoveSpeed)
        {
            return;
        }
        Debug.Log($"Add Force: {force}");
        Acceleration += force / mass;
    }

    public void AddForce(Vector3 value, ForceMode forceMode)
    {
        if (value.sqrMagnitude <= AnimatorHandler.SqreMinMoveSpeed) return;
        Debug.Log($"Add Force: {value}, {forceMode}");

        switch (forceMode)
        {
            case ForceMode.Force:
                Acceleration += value / mass;
                break;
            case ForceMode.Acceleration:
                Acceleration += value;
                break;
            case ForceMode.Impulse:
                Acceleration += value / (mass * Time.deltaTime);    // divide by Time.deltaTime here so as to make this force FrameRate Independent (Remember will multiply it by Time.deltaTime to calculate Velocity)
                break;
            case ForceMode.VelocityChange:
                m_velocity += value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(forceMode), forceMode, null);
        }
        
        
    }

    public void RotateBy(Vector3 angle)
    {
        frameRotDelta += angle;
    }

    public bool TryEnableAbility(Ability ability, bool force = false)
    {
        if (!this.allAbilities.Contains(ability)) return false;

        if (force || (currentAbility != null && !currentAbility.ShouldBlockAbilityStart(ability)))
        {
            SetAbilityActiveInner(currentAbility, false);
            SetAbilityActiveInner(ability, true);
            return true;
        }

        return false;
    }

    public bool TryDisableAbility(Ability ability, bool force = false)
    {
        if (!this.allAbilities.Contains(ability) || !ability.enabled) return false;

        if (force || (currentAbility != null && !currentAbility.ShouldBlockAbilityEnd(ability)))
        {
            SetAbilityActiveInner(ability, false);
            return true;
        }

        return false;
    }

    public T GetAbility<T>() where T : Ability
    {
        foreach (Ability ability in allAbilities)
        {
            if (ability is T abiT) return abiT;
        }

        return null;
    }

    public bool TryGetAbility<T>(out T ability) where T : Ability
    {
        ability = GetAbility<T>();
        return ability == null;
    }


    public int AbilityIntData
    {
        get => animatorHandler.AbilityIntData;
        set => animatorHandler.AbilityIntData = value;
    }
    
    public float AbilityFloatData
    {
        get => animatorHandler.AbilityFloatData;
        set => animatorHandler.AbilityFloatData = value;
    }
    
    
    public int ManagedInt1
    {
        get => animatorHandler.ManagedInt1;
        set => animatorHandler.ManagedInt1 = value;
    }
    
    
    public int ManagedInt2
    {
        get => animatorHandler.ManagedInt2;
        set => animatorHandler.ManagedInt2 = value;
    }
    
    
    public float ManagedFloat1
    {
        get => animatorHandler.ManagedFloat1;
        set => animatorHandler.ManagedFloat1 = value;
    }
    
    public float ManagedFloat2
    {
        get => animatorHandler.ManagedFloat2;
        set => animatorHandler.ManagedFloat2 = value;
    }
}