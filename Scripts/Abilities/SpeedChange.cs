using System;
using CCN.Core;
using UnityEngine;

namespace CCN.Abilities
{
    /// <summary> Run or Walk slowly ability for the player </summary>
    [Serializable]
    [DefaultAbilityId(-3)]
    [DefaultMoveSpeed(10)]
    [StartStopProfileInfo(StartStopProfile.Behaviour.ActiveWhilePressed, StartStopProfile.TriggerType.GetKey, "", KeyCode.LeftShift)]
    public class SpeedChange : Ability
    {
    }
}