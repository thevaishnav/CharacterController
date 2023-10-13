using System;
using UnityEngine;

namespace CCN.Core
{
    /// <summary> <see cref="Ability.AbilityId"/> of this ability when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultAbilityId : Attribute
    {
        public readonly int value;

        /// <summary> Set <see cref="Ability.AbilityId"/> value of this ability when it is created in the inspector </summary>
        /// <param name="value"> Value of ability value </param>
        public DefaultAbilityId(int value)
        {
            this.value = value;
        }
    }

    /// <summary> <see cref="Ability.TargetSpeed"/> of this ability when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultMoveSpeed : Attribute
    {
        public float speed;

        /// <summary> <see cref="Ability.TargetSpeed"/> of this ability when it is created in the inspector  </summary>
        /// <param name="value"> move speed </param>
        public DefaultMoveSpeed(float value)
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