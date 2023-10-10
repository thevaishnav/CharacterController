using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Ability : MonoBehaviour
{
    [SerializeField, Tooltip("Unique identifier for this ability. When this ability is enabled, \"Ability Index\" in animator will be set to this parameter value. Positive for custom abilities, negative for built-in abilities, 0 for nullAbility")]
    internal int abilityIndex;

    [SerializeField, Tooltip("How you want to start this ability. Automatic means it will start when the controller is activated, manual means you will start it from code, or you can choose to start it on keypress.")]
    internal AbilityStartType startType;

    [SerializeField, DrawIfEnumEqual("startType", 2)]
    internal KeyCode startKeyCode;

    [SerializeField, Tooltip("How you want to end this ability. Automatic means it will end when another ability is started, manual means you will end it from code, or you can choose to start it on keypress.")]
    internal AbilityEndType endType;

    [SerializeField, DrawIfEnumEqual("endType", 2)]
    internal KeyCode endKeyCode;

    [SerializeField, Tooltip("What should be the movement speed of the character when this ability is enabled. Keep it <=0 if you wish to use default speed (set in PlayerMovement component)")]
    internal float movementSpeed = -1f;

    protected bool IsGrounded => Controller.IsGrounded;
    protected bool IsMoving => Controller.IsMoving;
    protected float Speed => Controller.Speed;
    protected Vector3 Velocity => Controller.m_velocity;
    public bool IsEnabled => enabled;
    public int AbilityIndex => abilityIndex;
    public PlayerMovement Controller { get; private set; }

    private void Reset()
    {
        Type type = this.GetType();
        PlayerMovement pm = GetComponentInParent<PlayerMovement>();
        if (pm == null)
        {
            Debug.LogError($"Ability can only be added as a child of a GameObject with \"PlayerMovement\" script");
            DestroyImmediate(this);
            return;
        }

        Component[] comps = pm.GetComponentsInChildren(type);
        if (comps.Length > 1 && comps[0] != null)
        {
            #if UNITY_EDITOR
            EditorGUIUtility.PingObject(comps[0].GetComponent(type));
            #endif
            Debug.LogError($"There already exists an ability of type {type}.");
            DestroyImmediate(this);
            return;
        }

        if (type.GetMethod("Awake", BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.CreateInstance | BindingFlags.DeclaredOnly) != null)
        {
            Debug.LogError($"Awake wont work in ability ({type}). You can use other MonoBehaviour callbacks like OnEnable or Start.");
        }

        DefaultAbilityIndex abilityIndex = type.GetCustomAttribute<DefaultAbilityIndex>();
        if (abilityIndex != null)
        {
            this.abilityIndex = abilityIndex.index;
        }

        DefaultAbilityStartType abilityStartType = type.GetCustomAttribute<DefaultAbilityStartType>();
        if (abilityStartType != null)
        {
            this.startType = abilityStartType.startType;
            this.startKeyCode = abilityStartType.startKeyCode;
        }

        DefaultAbilityEndType abilityEndType = type.GetCustomAttribute<DefaultAbilityEndType>();
        if (abilityEndType != null)
        {
            this.endType = abilityEndType.endType;
            this.endKeyCode = abilityEndType.endKeyCode;
        }

        DefaultMoveSpeed moveSpeed = type.GetCustomAttribute<DefaultMoveSpeed>();
        if (moveSpeed != null)
        {
            this.movementSpeed = moveSpeed.speed;
        }
    }
    
    internal void SelfInit(PlayerMovement abilitiesHandler)
    {
        Controller = abilitiesHandler;
    }

    public virtual bool ShouldBlockAbilityStart(Ability abilityAboutToStart)
    {
        return false;
    }

    public virtual bool ShouldBlockAbilityEnd(Ability ability)
    {
        return false;
    }

    public bool TryEnable(bool force) => this.Controller.TryEnableAbility(this, force);
    public bool TryDisable(bool force) => this.Controller.TryDisableAbility(this, force);
}