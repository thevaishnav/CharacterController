using System;
using CCN.Core;
using UnityEngine;

namespace CCN.Abilities
{
    /// <summary> Crouch ability for the player </summary>
    [Serializable]
    [DefaultAbilityId(-1)]
    [StartStopProfileInfo(StartStopProfile.Behaviour.PressToToggle, StartStopProfile.TriggerType.GetKey, "", KeyCode.C)]
    public class CrouchAbility : Ability
    {
    }
}
