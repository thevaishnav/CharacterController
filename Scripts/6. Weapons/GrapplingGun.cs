using CCN.Core;
using UnityEngine;

namespace CCN.Weapon
{
    public class GrapplingGun : AgentItem
    {
        [SerializeField] private LineRenderer ropeRenderer;
        [SerializeField] private Transform nozzle;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private LayerMask whatIsGrappleable;
        [SerializeField] private float range = 100f;
        [SerializeField] private float initialForce = 10f;
        [SerializeField] private float persistantForce = -10f;

        private RaycastHit _hit;
        private Rigidbody _currentTarget;
        private Vector3 _lastNozzlePos;
        private Vector3 _currentNozzlePos;

        protected override void Awake()
        {
            base.Awake();
            ropeRenderer.enabled = false;
            ropeRenderer.positionCount = 2;
        }

        private void FixedUpdate()
        {
            AddForceOnTarget(persistantForce);
        }

        private void LateUpdate()
        {
            // Draw Rope
            if (IsUsing && _currentTarget != null)
            {
                ropeRenderer.SetPosition(0, nozzle.position);
                ropeRenderer.SetPosition(1, _currentTarget.transform.position);
            }
        }

        protected override void Equip()
        {
        }

        protected override void Unequip()
        {
        }
        
        protected override void StartUse()
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out _hit, range, whatIsGrappleable))
            {
                _currentTarget = _hit.rigidbody;
                ropeRenderer.enabled = true;
                AddForceOnTarget(initialForce);
            }
            else
            {
                ropeRenderer.enabled = true;
                TryStopUse();
            }
        }

        private void AddForceOnTarget(float multiplier)
        {
            if (!IsUsing || _currentTarget == null) return;

            Vector3 force = (transform.position - _currentTarget.position) * multiplier;
            _currentNozzlePos = nozzle.position;
            if (_currentNozzlePos != _lastNozzlePos) force = Quaternion.Euler((_currentNozzlePos - _lastNozzlePos).normalized) * force;
            _currentTarget.AddForce(force);
            _lastNozzlePos = _currentNozzlePos;
        }

        protected override void StopUse()
        {
            _currentTarget = null;
            ropeRenderer.enabled = false;
        }
    }
}