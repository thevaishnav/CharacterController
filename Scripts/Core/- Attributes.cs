using System;
using UnityEngine;

namespace CCN.Core
{
    /// <summary> <see cref="AgentBehaviour.ID"/> of this behaviour when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultId : Attribute
    {
        public readonly int value;

        /// <summary> Set <see cref="AgentBehaviour.ID"/> value of this behaviour when it is created in the inspector </summary>
        /// <param name="value"> Value of behaviour value </param>
        public DefaultId(int value)
        {
            this.value = value;
        }
    }

    /// <summary> <see cref="AgentBehaviour.MoveSpeedMultiplier"/> of this behaviour when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultMoveSpeedMultiplier : Attribute
    {
        public float speed;

        /// <summary> <see cref="AgentBehaviour.MoveSpeedMultiplier"/> of this behaviour when it is created in the inspector  </summary>
        /// <param name="value"> move speed </param>
        public DefaultMoveSpeedMultiplier(float value)
        {
            speed = value;
        }
    }
    
    /// <summary> Start stop profile info </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StartStopProfileInfo : Attribute
    {
        public StartStopProfile.Behaviour pcBehaviour;
        public StartStopProfile.TriggerType pcTriggerType;
        public string pcButton;
        public KeyCode pcKeycode;

        public StartStopProfileInfo(StartStopProfile.Behaviour pcBehaviour, StartStopProfile.TriggerType pcTriggerType, string pcButton, KeyCode pcKeycode)
        {
            this.pcBehaviour = pcBehaviour;
            this.pcTriggerType = pcTriggerType;
            this.pcButton = pcButton;
            this.pcKeycode = pcKeycode;
        }
    }
}