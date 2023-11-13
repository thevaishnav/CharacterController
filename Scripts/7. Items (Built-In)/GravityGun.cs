using MenuManagement.Base;
using Omnix.CCN.Core;
using Omnix.CCN.InputSystemWrapper;
using Omnix.CCN.Items.ForceSources;
using UnityEngine;

namespace Omnix.CCN.Items
{
    [GroupProperties(_.INPUT_PROFILES, "useReverseProfile")]
    public class GravityGun : ForceSourceMultiTargets
    {
        [SerializeField] private InteractionProfileBase useReverseProfile;
        [SerializeField] private float reverseForceMultiplier;


        private float _persistantForce;
        private bool _usingReverse;

        protected override void SetupInteractions(Agent agent)
        {
            _persistantForce = PersistantForce;

            if (useProfile == null || useReverseProfile == null)
            {
                base.SetupInteractions(agent);
                return;
            }

            if (equipmentProfile)
            {
                var iptEquipment = new IPT_FuncBool(() => IsEquipped, TryEquip, TryUnequip);
                equipmentProfile.DoTarget(iptEquipment, agent);
            }

            var iptUse = new IPT_FuncBool(() => IsUsing && !_usingReverse, TryStartUse, TryStopUse);
            useProfile.DoTarget(iptUse, agent);

            var iptRev = new IPT_FuncBool(() => IsUsing && _usingReverse, UseReverse, TryStopUse);
            useReverseProfile.DoTarget(iptRev, agent);
        }

        private bool UseReverse()
        {
            _usingReverse = true;
            return TryStartUse();
        }

        protected override void OnStartUse()
        {
            if (_usingReverse) PersistantForce = -reverseForceMultiplier * _persistantForce;
            else PersistantForce = _persistantForce;
        }

        protected override void OnStopUse()
        {
            _usingReverse = false;
        }
    }
}