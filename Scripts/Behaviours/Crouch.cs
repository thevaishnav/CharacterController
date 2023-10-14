using System;
using CCN.Core;
using UnityEngine;

namespace CCN.Behaviours
{
    /// <summary> Crouch behaviour for the Agent </summary>
    [Serializable]
    [DefaultId(-1)]
    [StartStopProfileInfo(StartStopProfile.Behaviour.PressToToggle, StartStopProfile.TriggerType.GetKey, "", KeyCode.C)]
    public class Crouch : AgentBehaviour
    {
    }
}
