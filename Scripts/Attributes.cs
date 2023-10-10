using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultAbilityIndex : Attribute
{
    public int index;

    public DefaultAbilityIndex(int index)
    {
        this.index = index;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultAbilityStartType : Attribute
{
    public AbilityStartType startType;
    public KeyCode startKeyCode;

    /// <param name="isAutomatic">Set it to True if you want default start type to be automatic, false if you want start type to be manual</param>
    public DefaultAbilityStartType(bool isAutomatic)
    {
        if (isAutomatic)
        {
            startType = AbilityStartType.Automatic;
        }
        else
        {
            startType = AbilityStartType.Manual;
        }
    }

    /// <param name="keyCode">Pass the keycode you want the ability to start with</param>
    public DefaultAbilityStartType(KeyCode keyCode)
    {
        startType = AbilityStartType.KeyDown;
        startKeyCode = keyCode;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultAbilityEndType : Attribute
{
    public AbilityEndType endType;
    public KeyCode endKeyCode;

    /// <param name="isAutomatic">Set it to True if you want default start type to be automatic, false if you want start type to be manual</param>
    public DefaultAbilityEndType(bool isAutomatic)
    {
        if (isAutomatic)
        {
            endType = AbilityEndType.Automatic;
        }
        else
        {
            endType = AbilityEndType.Manual;
        }
    }

    /// <param name="keyCode">Pass the keycode you want the ability to start with</param>
    public DefaultAbilityEndType(KeyCode keyCode)
    {
        endType = AbilityEndType.KeyUp;
        endKeyCode = keyCode;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultMoveSpeed : Attribute
{
    public float speed;

    public DefaultMoveSpeed(float speed)
    {
        this.speed = speed;
    }
}