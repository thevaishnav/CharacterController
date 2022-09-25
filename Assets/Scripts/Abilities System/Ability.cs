using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] internal int abilityIndex;
    [SerializeField] internal AbilityStartType startType;
    [SerializeField, DrawIfEnumEqual("startType", 2)] internal KeyCode startKeyCode;
    [SerializeField] internal AbilityEndType endType;
    [SerializeField, DrawIfEnumEqual("endType", 1)] internal KeyCode endKeyCode;
    [SerializeField] internal SpeedType speedType = SpeedType.Ignored;
    [SerializeField, DrawIfEnumEqual("speedType", 0, DrawIfEnumEqualAttribute.ComparisonType.NotEqual)] internal float movementSpeed;

    internal new bool enabled
    {
        get => base.enabled;
        set => base.enabled = value;
    }

    protected bool IsGrounded => Controller.IsGrounded;
    protected bool IsMoving => Controller.IsMoving;
    protected float Speed => Controller.Speed;
    protected Vector3 Velocity => Controller.Velocity;
    public bool IsEnabled => enabled;
    public int AbilityIndex => abilityIndex;
    public PlayerMovement Controller { get; private set; }
    

    private void Awake()
    {
        
    }
    
    internal void SelfInit(PlayerMovement abilitiesHandler)
    {
        Controller = abilitiesHandler;
        this.enabled = (startType == AbilityStartType.Automatic);
    }

    public virtual bool ShouldBlockAbilityStart(Ability ability)
    {
        return false;
    }

    public virtual bool ShouldBlockAbilityEnd(Ability ability)
    {
        return false;
    }

    public bool TryEnable(bool force) => this.Controller.TryEnableAbility(this, force);
    public bool TryDisable(bool force) => this.Controller.TryDisableAbility(this, force);
}