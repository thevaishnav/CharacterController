using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class AbilitiesHandler : MonoBehaviour
{
    private Dictionary<string, HashSet<Ability>> eventListeners;
    [SerializeField, HideInInspector] private List<Ability> allAbilities;
    private List<Ability> activeAbilities;
    public PlayerMovement Controller { get; private set; }
    

    void Awake()
    {
        Controller = GetComponentInParent<PlayerMovement>();       
        allAbilities = GetComponentsInChildren<Ability>().ToList();
        foreach (Ability ability in allAbilities)
        {
            ability.SelfInit(this);
        }
    }

    void SetAbilityActiveInner(Ability ability, bool value)
    {
        ability.enabled = value;
        if (value) this.activeAbilities.Add(ability);
        else this.activeAbilities.Remove(ability);
    }
    
    /*internal void BindAbilityEvents(Ability ability)
    {
        foreach (string eventName in ability.eventsList)
        {
            if (eventListeners.ContainsKey(eventName)) eventListeners[eventName].Add(ability);
            else eventListeners.Add(eventName, new HashSet<Ability>() {ability});
        }
    }
    
    internal void UnbindAbilityEvents(Ability ability)
    {
        foreach (string eventName in ability.eventsList)
        {
            eventListeners[eventName].Remove(ability);
        }
    }

    internal bool HasEvent(string eventName)
    {
        return this.eventListeners.ContainsKey(eventName);
    }

    internal void BroadcastEvent(string eventName)
    {
        foreach (Ability ability in eventListeners[eventName])
        {
            ability.QuickInvoke(eventName);
        }
    }*/

    public bool ActivateAbility(Ability ability, bool force)
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

    public bool DeactivateAbility(Ability ability, bool force)
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
}
