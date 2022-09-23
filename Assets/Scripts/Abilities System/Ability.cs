using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] private AbilityStartType startType;
    [SerializeField] private int abilityIndex;
    /*internal List<string> eventsList;*/

    internal new bool enabled
    {
        get => base.enabled;
        set => base.enabled = value;
    }
    protected bool IsGrounded => handler.Controller.IsGrounded;
    protected bool IsMoving => handler.Controller.IsMoving;
    protected float Speed => handler.Controller.Speed;
    protected Vector3 Velocity => handler.Controller.Velocity;
    public bool IsActive => enabled;
    public int AbilityIndex => abilityIndex;
    public AbilitiesHandler handler { get; private set; }
    public PlayerMovement Controller => handler.Controller;
    
    
    internal void SelfInit(AbilitiesHandler abilitiesHandler)
    {
        handler = abilitiesHandler;
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

    public bool Activate(bool force) => this.handler.ActivateAbility(this, force);
    public bool Deactivate(bool force) => this.handler.DeactivateAbility(this, force);
    
    
    /*internal void SelfUpdateEventInfo()
{
    if (eventsList == null)
    {
        eventsList = new List<string>();
    }
    else
    {
        this.handler.UnbindAbilityEvents(this);
        this.eventsList.Clear();
    }

    foreach (MethodInfo methodinf in this.GetAllMethods())
    {
        if (this.handler.HasEvent(methodinf.Name) && methodinf.GetParameters().Length == 0)
        {
            eventsList.Add(methodinf.Name);
        }
    }
    this.handler.BindAbilityEvents(this);
}*/

}