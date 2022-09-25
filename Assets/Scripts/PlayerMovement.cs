using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;


[RequireComponent(typeof(CharacterController))]
[DefaultExecutionOrder(-100)]
public class PlayerMovement : MonoBehaviour
{
    #region Serialized
    [Header("Controls")] [SerializeField] float defaultMoveSpeed = 5f;
    [SerializeField] bool hideCursor;

    [Header("Ground Check")] [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance = 0.4f;
    #endregion

    #region Private
    private CharacterController buildInController;
    private Vector3 framePosDelta;
    private Vector3 frameRotDelta;
    private bool isRotating;
    #endregion

    #region Abilities Parameters
    private Dictionary<string, HashSet<Ability>> eventListeners;
    private Ability[] allAbilities;
    private List<Ability> activeAbilities;
    private float MoveSpeed;
    #endregion

    #region Exposed Variables
    public bool IsMoving { get; private set; }
    public float Speed { get; private set; }
    public Vector3 Velocity { get; private set; }
    public bool IsGrounded { get; private set; }
    #endregion

    #region Unity Callbacks
    void Awake()
    {
        allAbilities = GetComponentsInChildren<Ability>();
        activeAbilities = new List<Ability>();
        MoveSpeed = float.MinValue;

        foreach (Ability ability in allAbilities)
        {
            ability.SelfInit(this);
            if (ability.enabled) activeAbilities.Add(ability);
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
    }
    
    void FixedUpdate()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    void Update()
    {
        foreach (Ability ability in allAbilities)
        {
            if (ability.enabled)
            {
                if (ability.endType == AbilityEndType.KeyUp && Input.GetKeyUp(ability.endKeyCode)) SetAbilityActiveInner(ability, false);
            }
            else
            {
                if (ability.startType == AbilityStartType.KeyDown && Input.GetKeyDown(ability.startKeyCode)) SetAbilityActiveInner(ability, true);
            }
        }
    }
    
    void LateUpdate()
    {
        if (IsMoving)
        {
            buildInController.Move(framePosDelta);
            Velocity = framePosDelta / Time.deltaTime;
            Speed = Velocity.magnitude;
        }

        if (isRotating)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + frameRotDelta);
            isRotating = false;
            frameRotDelta = Vector3.zero;
        }
        
        // Move with inputs
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        framePosDelta = move * MoveSpeed * Time.deltaTime;
        IsMoving = move.sqrMagnitude > 0.02f;
    }
    #endregion
    
    #region Methods
    void SetAbilityActiveInner(Ability ability, bool value)
    {
        if (value) this.activeAbilities.Add(ability);
        else this.activeAbilities.Remove(ability);
        ability.enabled = value;
        UpdateMovementSpeed();
    }

    void UpdateMovementSpeed()
    {
        MoveSpeed = float.MinValue;
        foreach (Ability ability in activeAbilities)
        {
            if (ability.speedType == SpeedType.AtLeast) MoveSpeed = Mathf.Max(MoveSpeed, ability.movementSpeed);
            else MoveSpeed = Mathf.Min(MoveSpeed, ability.movementSpeed);
        }
        if (MoveSpeed <= 0) MoveSpeed = defaultMoveSpeed;
    }

    public void InstantMoveBy(Vector3 distance)
    {
        framePosDelta += distance;
        IsMoving = true;
    }

    public void InstantRotateBy(Vector3 angle)
    {
        frameRotDelta += angle;
        isRotating = true;
    }

    public void InstantMoveByX(float distance)
    {
        framePosDelta.x += distance;
        IsMoving = true;
    }
    
    public void InstantMoveByY(float distance)
    {
        framePosDelta.y += distance;
        IsMoving = true;
    }
    
    public void InstantMoveByZ(float distance)
    {
        framePosDelta.z += distance;
        IsMoving = true;
    }

    public void InstantRotateByX(float angle)
    {
        frameRotDelta.x += angle;
        isRotating = true;
    }

    public void InstantRotateByY(float angle)
    {
        frameRotDelta.y += angle;
        isRotating = true;
    }
    
    public void InstantRotateByZ(float angle)
    {
        frameRotDelta.z += angle;
        isRotating = true;
    }
    
    public bool TryEnableAbility(Ability ability, bool force)
    {
        if (!this.allAbilities.Contains(ability)) return false;

        if (force)
        {
            SetAbilityActiveInner(ability, true);
            return true;
        }

        foreach (Ability activeAbility in activeAbilities)
        {
            if (activeAbility.ShouldBlockAbilityStart(ability)) return false;
        }

        SetAbilityActiveInner(ability, true);
        return true;
    }

    public bool TryDisableAbility(Ability ability, bool force)
    {
        if (!this.allAbilities.Contains(ability) || !this.activeAbilities.Contains(ability)) return false;

        if (force)
        {
            SetAbilityActiveInner(ability, false);
            return true;
        }

        foreach (Ability activeAbility in activeAbilities)
        {
            if (activeAbility.ShouldBlockAbilityEnd(ability)) return false;
        }

        SetAbilityActiveInner(ability, false);
        return true;
    }
    #endregion
}