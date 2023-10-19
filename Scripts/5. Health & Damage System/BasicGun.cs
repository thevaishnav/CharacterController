using CCN.Core;
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


    public class BasicGun : AgentItem, IDamageDealer
    {
        [Header("References")] 
        [SerializeField, Tooltip("Spawn location and move direction for the bullet")]
        protected Transform nozzle;

        [SerializeField] protected GunSpecs gunSpecs;
        
        [SerializeField, Tooltip("What object layers the gun can attack")]
        protected LayerMask hitMask;

        [SerializeField, Tooltip("Ammo currently present in the magazine.")]
        public int currentAmmo;

        [SerializeField, Tooltip("Magazines currently available to the player. Float because its possible to have half loaded magazine.")]
        public float magazinesCount;

        [SerializeField, Tooltip("Value of \"ManagedInt1\" animator parameter while reloading this weapon.")]
        public int reloadAnimationManagedInt1Value;


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


        public override void Use()
        {
        }
    }
}