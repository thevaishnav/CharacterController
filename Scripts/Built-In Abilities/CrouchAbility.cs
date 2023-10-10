using System;
using KS.CharaCon.Attributes;
using UnityEngine;

namespace KS.CharaCon.Abilities
{
    /// <summary> Crouch ability for the player </summary>
    [Serializable]
    [DefaultAbilityIndex(-1)]
    [DefaultAbilityStartType(KeyCode.C)]
    [DefaultAbilityEndType(KeyCode.C)]
    public class CrouchAbility : Ability
    {
    }
}