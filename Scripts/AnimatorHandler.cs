using UnityEngine;

public class AnimatorHandler
{
    public static readonly float MinMoveSpeed = 0.005f;
    public static readonly float SqreMinMoveSpeed = MinMoveSpeed * MinMoveSpeed;

    public bool IsMoving => animator.GetBool(ID_IsMoving);

    public bool IsGrounded
    {
        get => animator.GetBool(ID_IsGrounded);
        set => animator.SetBool(ID_IsGrounded, value);
    }

    public float Speed
    {
        get => this.currentSpeed;
        set
        {
            if (value > MinMoveSpeed)
            {
                targetSpeed = value;
                animator.SetBool(ID_IsMoving, true);
            }
            else
            {
                targetSpeed = 0f;
                animator.SetBool(ID_IsMoving, false);
            }
        }
    }

    public int AbilityIndex
    {
        get => animator.GetInteger(ID_AbilityIndex);
        set => animator.SetInteger(ID_AbilityIndex, value);
    }

    public int AbilityIntData
    {
        get => animator.GetInteger(ID_AbilityIntData);
        set => animator.SetInteger(ID_AbilityIntData, value);
    }

    public float AbilityFloatData
    {
        get => animator.GetFloat(ID_AbilityFloatData);
        set => animator.SetFloat(ID_AbilityFloatData, value);
    }

    public int ManagedInt1
    {
        get => animator.GetInteger(ID_ManagedInt1);
        set => animator.SetInteger(ID_ManagedInt1, value);
    }

    public int ManagedInt2
    {
        get => animator.GetInteger(ID_ManagedInt2);
        set => animator.SetInteger(ID_ManagedInt2, value);
    }

    public float ManagedFloat1
    {
        get => animator.GetFloat(ID_ManagedFloat1);
        set => animator.SetFloat(ID_ManagedFloat1, value);
    }

    public float ManagedFloat2
    {
        get => animator.GetFloat(ID_ManagedFloat2);
        set => animator.SetFloat(ID_ManagedFloat2, value);
    }


    Animator animator;
    int ID_IsMoving;
    int ID_IsGrounded;
    int ID_Speed;
    int ID_AbilityIndex;
    int ID_AbilityIntData;
    int ID_AbilityFloatData;
    int ID_ManagedInt1;
    int ID_ManagedInt2;
    int ID_ManagedFloat1;
    int ID_ManagedFloat2;

    private float targetSpeed;
    private float currentSpeed;

    public AnimatorHandler(Animator animator)
    {
        this.animator = animator;
        ID_AbilityIndex = Animator.StringToHash("Ability Index");
        ID_Speed = Animator.StringToHash("Speed");
        ID_IsMoving = Animator.StringToHash("Moving");
        ID_IsGrounded = Animator.StringToHash("Grounded");
        ID_AbilityIntData = Animator.StringToHash("AbilityIntData");
        ID_AbilityFloatData = Animator.StringToHash("AbilityFloatData");
        ID_ManagedInt1 = Animator.StringToHash("ManagedInt1");
        ID_ManagedInt2 = Animator.StringToHash("ManagedInt2");
        ID_ManagedFloat1 = Animator.StringToHash("ManagedFloat1");
        ID_ManagedFloat2 = Animator.StringToHash("ManagedFloat2");

        currentSpeed = 0f;
        animator.SetFloat(ID_Speed, 0f);
        animator.SetBool(ID_IsMoving, false);
    }

    public void UpdateSpeed()
    {
        if (Mathf.Abs(currentSpeed - targetSpeed) < MinMoveSpeed)
        {
            if (currentSpeed < MinMoveSpeed)
            {
                currentSpeed = 0f;
                animator.SetFloat(ID_Speed, 0f);
            }

            return;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 10f);
        animator.SetFloat(ID_Speed, currentSpeed);
    }
}