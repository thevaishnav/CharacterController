using System;
using KS.CharaCon.Attributes;
using UnityEngine;

namespace KS.CharaCon.Abilities
{
    /// <summary> Run or Walk slowly ability for the player </summary>
    [Serializable]
    [DefaultAbilityIndex(-3)]
    [DefaultAbilityStartType(KeyCode.LeftShift)]
    [DefaultAbilityEndType(KeyCode.LeftShift)]
    [DefaultMoveSpeed(10)]
    public class SpeedChange : KS.CharaCon.Ability
    {
    }
}