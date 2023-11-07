using System;
using System.Collections;
using CCN.Core;
using CCN.InputSystemWrapper;
using CCN.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCN.Health
{
    public enum GunReloadType
    {
        /// <summary>
        /// Instantly empty the currently loaded magazine, and load the next magazine completely.
        /// </summary>
        FullReload,

        /// <summary>
        /// Reduce enough ammo from new mag to fill the current mag (Accounting for ammo present in current mag) 
        /// </summary>
        SemiReload
    }


    // TODO: Incomplete implementation
    public class BasicGun : AgentItem, IDamageDealer
    {
        [Header("References")] [SerializeField, Tooltip("Spawn location and move direction for the bullet")]
        protected Transform nozzle;

        [SerializeField, Tooltip("Specs for this gun")]
        protected GunSpecs specs;

        [SerializeField, Tooltip("What object layers the gun can attack")]
        protected LayerMask hitMask;

        [SerializeField, Tooltip("Ammo currently present in the magazine.")]
        protected int currentAmmo;

        [SerializeField, Tooltip("Magazines currently available to the player. Float because its possible to have half loaded magazine.")]
        protected float currentMagCount;


        [SerializeField, Tooltip("Input profile for reloading")]
        protected InteractionProfileBase reloadProfile;

        [SerializeField, Tooltip("Value of \"ManagedInt1\" animator parameter while reloading this weapon.")]
        protected int reloadAnimationManagedInt1Value;

        protected bool DidHit;
        protected RaycastHit Hit;

        public float Range { get; private set; }
        public bool IsReloading { get; private set; }

        protected virtual void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (nozzle == null) nozzle = transform;
            if (Selection.activeTransform != nozzle && Selection.activeTransform != transform) return;


            Vector3 pos = nozzle.position;
            Vector3 direction = nozzle.forward;

            GizmosExtension.DrawArrow(pos, direction);
            #endif
        }

        public override void Init(Agent agent)
        {
            base.Init(agent);
            if (reloadProfile) reloadProfile.DoTarget(this, agent);
        }
        
        /// <summary> Reload this gun </summary>
        public virtual void Reload()
        {
            GunMath.Reload(ref currentAmmo, ref currentMagCount, specs.magazineSize, specs.reloadType, false);
            StartCoroutine(Utilitiees.TempSetManagedIntOne(Agent, reloadAnimationManagedInt1Value, specs.reloadDuration));
        }

        /// <summary>  Will be called only if the Gun is Equipped <see cref="AgentItem"/> </summary>
        protected virtual void Update()
        {
            if (IsUsing) return;
            DidHit = Physics.Raycast(nozzle.position, nozzle.forward, out Hit, Range, hitMask);
        }

        protected override void Equip()
        {
            Keyframe lastKey = specs.distanceToDamageCurve[specs.distanceToDamageCurve.length - 1];
            Range = lastKey.time;
        }

        protected override void Unequip()
        {
        }

        protected override void StartUse()
        {
            if (currentAmmo <= 0 && specs.autoReload == false) return;
            
        }

        protected override void StopUse()
        {
        }

        protected override bool IsInteractingWithProfile(InteractionProfileBase profile)
        {
            if (base.IsInteractingWithProfile(profile)) return true;
            if (profile == reloadProfile) return IsReloading;
            return false;
        }

        protected override bool StartInteractionWithProfile(InteractionProfileBase profile)
        {
            if (base.StartInteractionWithProfile(profile)) return true;
            if (profile == reloadProfile)
            {
                Reload();
                return true;
            }

            return false;
        }
    }
}