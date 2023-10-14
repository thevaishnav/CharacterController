using System;
using CCN.Core;
using UnityEngine;

namespace CCN.Behaviours
{
    /// <summary> Run or Walk slowly behaviour for the agent </summary>
    [Serializable]
    [DefaultId(-3)]
    [DefaultMoveSpeedMultiplier(2f)]
    [StartStopProfileInfo(StartStopProfile.Behaviour.ActiveWhilePressed, StartStopProfile.TriggerType.GetKey, "", KeyCode.LeftShift)]
    public class SetMoveSpeed : AgentBehaviour
    {
    }
}