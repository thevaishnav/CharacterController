using Omnix.CCN.Items.ForceSources;
using UnityEngine;

namespace Omnix.CCN.Weapon
{
    public class GrapplingGun : ForceSourceSingleTarget
    {
        // @formatter:off
        private const string TT_ROPE_RENDERER = "LineRenderer to render rope. Must have exactly 2 positions";

        [Header("GrapplingGun")]
        [SerializeField, Tooltip(TT_ROPE_RENDERER)] private LineRenderer _ropeRenderer;
        
        protected override void Equip() { }
        protected override void Unequip() { }
        protected override void OnStartUse() => _ropeRenderer.enabled = true;
        protected override void OnStopUse() => _ropeRenderer.enabled = false;
        // @formatter:on

        protected override void Awake()
        {
            base.Awake();
            _ropeRenderer.enabled = false;
            _ropeRenderer.positionCount = 2;
        }

        private void LateUpdate()
        {
            // Draw Rope
            if (IsUsing && CurrentTarget != null)
            {
                _ropeRenderer.SetPosition(0, Nozzle.position);
                _ropeRenderer.SetPosition(1, CurrentTarget.transform.position);
            }
        }
    }
}