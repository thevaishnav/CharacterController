using System;
using UnityEngine;

namespace KS.CharaCon.Attributes
{
    /// <summary> <see cref="Ability.AbilityIndex"/> of this ability when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultAbilityIndex : Attribute
    {
        public readonly int Index;
        /// <summary> Set <see cref="Ability.AbilityIndex"/> index of this ability when it is created in the inspector </summary>
        /// <param name="value"> Value of ability index </param>
        public DefaultAbilityIndex(int value)
        {
            this.Index = value;
        }
    }

    /// <summary> <see cref="Ability.startType"/> of this ability when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultAbilityStartType : Attribute
    {
        public AbilityStartType StartType;
        public KeyCode StartKeyCode;

        /// <summary>
        /// Ability <see cref="Ability.startType"/> of this ability when it is created in the inspector.
        /// Use this overload only for <see cref="AbilityStartType.Automatic"/> or <see cref="AbilityStartType.Manual"/> start type.
        /// </summary>
        /// <param name="isAutomatic"> true if you want default start type to be <see cref="AbilityStartType.Automatic"/>, false if you want start type to be <see cref="AbilityStartType.Manual"/></param>
        public DefaultAbilityStartType(bool isAutomatic)
        {
            if (isAutomatic) StartType = AbilityStartType.Automatic;
            else StartType = AbilityStartType.Manual;
        }


        /// <summary>
        /// Ability <see cref="Ability.startType"/> of this ability when it is created in the inspector.
        /// Use this overload for start type <see cref="AbilityStartType.KeyDown"/>.
        /// </summary>
        /// <param name="keyCode"> Keycode of the key </param>
        public DefaultAbilityStartType(KeyCode keyCode)
        {
            StartType = AbilityStartType.KeyDown;
            StartKeyCode = keyCode;
        }
    }

    /// <summary> <see cref="Ability.endType"/> of this ability when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultAbilityEndType : Attribute
    {
        public AbilityEndType EndType;
        public KeyCode EndKeyCode;
        public float Duration;

        /// <summary>
        /// Ability <see cref="Ability.endType"/> of this ability when it is created in the inspector.
        /// Use this overload only for <see cref="AbilityEndType.Automatic"/> or <see cref="AbilityEndType.Manual"/> end type.
        /// </summary>
        /// <param name="isAutomatic"> true if you want default start type to be <see cref="AbilityEndType.Automatic"/>, false if you want start type to be <see cref="AbilityEndType.Manual"/></param>
        public DefaultAbilityEndType(bool isAutomatic)
        {
            if (isAutomatic) EndType = AbilityEndType.Automatic;
            else EndType = AbilityEndType.Manual;
        }

        /// <summary>
        /// Ability <see cref="Ability.endType"/> of this ability when it is created in the inspector.
        /// Use this overload end type <see cref="AbilityEndType.KeyUp"/>.
        /// </summary>
        /// <param name="keyCode"> Keycode of the key </param>
        public DefaultAbilityEndType(KeyCode keyCode)
        {
            EndType = AbilityEndType.KeyUp;
            EndKeyCode = keyCode;
        }
    }

    /// <summary> <see cref="Ability.TargetSpeed"/> of this ability when it is created in the inspector </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultMoveSpeed : Attribute
    {
        public float Speed;

        /// <summary> <see cref="Ability.TargetSpeed"/> of this ability when it is created in the inspector  </summary>
        /// <param name="value"> move speed </param>
        public DefaultMoveSpeed(float value)
        {
            this.Speed = value;
        }
    }
}