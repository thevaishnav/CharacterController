using CCN.Core;
using UnityEngine;

namespace CCN.Weapon
{
    public class GrapplingGun : AgentItem
    {
        private const string TT_ROPE_RENDERER = "LineRenderer to render rope. Must have exactly 2 positions";
        private const string TT_NOZZLE = "Point at which the rope is attached to the gun";
        private const string TT_CAMERA_TRANSFORM = "Camera transform, used for rayCast";
        private const string TT_LAYER_MASK = "What is grapple-able";
        private const string TT_RANGE = "Maximum distance to which the gun can target anything";
        private const string TT_ATTACHMENT_FORCE = "Force applied when the grappling starts";
        private const string TT_SPRING_FORCE = "Force applied every frame";
        private const string TT_MOVEMENT_INFLUENCE = "How much should the gun movement influence spring force. 0 means ignore movement, 1 means apply force in direction of movement.";

        // @formatter:off
        [Header("GrapplingGun")]
        [SerializeField, Tooltip(TT_ROPE_RENDERER)]                      private LineRenderer _ropeRenderer;
        [SerializeField, Tooltip(TT_NOZZLE)]                             private Transform _nozzle;
        [SerializeField, Tooltip(TT_CAMERA_TRANSFORM)]                   private Transform _cameraTransform;
        [SerializeField, Tooltip(TT_LAYER_MASK)]                         private LayerMask _layerMask;
        [SerializeField, Tooltip(TT_RANGE)]                              private float _range = 100f;
        [SerializeField, Tooltip(TT_ATTACHMENT_FORCE)]                   private float _attachmentForce = 10f;
        [SerializeField, Tooltip(TT_SPRING_FORCE)]                       private float _springForce = -3f;
        [SerializeField, Tooltip(TT_SPRING_FORCE)]                       private float _maxSpringForceMagnitude = -20f;
        [SerializeField, Range(-1f, 1f), Tooltip(TT_MOVEMENT_INFLUENCE)] private float _movementInfluence;
        // @formatter:on

        private RaycastHit _hit;
        private Rigidbody _currentTarget;
        private Vector3 _lastNozzlePos;

        protected override void Awake()
        {
            base.Awake();
            _ropeRenderer.enabled = false;
            _ropeRenderer.positionCount = 2;
        }

        private void FixedUpdate()
        {
            AddForceOnTarget(_springForce);
        }

        private void LateUpdate()
        {
            // Draw Rope
            if (IsUsing && _currentTarget != null)
            {
                _ropeRenderer.SetPosition(0, _nozzle.position);
                _ropeRenderer.SetPosition(1, _currentTarget.transform.position);
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
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out _hit, _range, _layerMask))
            {
                _currentTarget = _hit.rigidbody;
                _ropeRenderer.enabled = true;
                _lastNozzlePos = _nozzle.position;
                AddForceOnTarget(_attachmentForce);
            }
            else
            {
                _ropeRenderer.enabled = true;
                TryStopUse();
            }
        }

        private void AddForceOnTarget(float multiplier)
        {
            if (!IsUsing || _currentTarget == null) return;

            Vector3 springForce = (_currentTarget.position - transform.position) * multiplier;
            float totalForceMagnitude = springForce.magnitude;
            Vector3 springForceDirection = springForce.normalized;
            
            if (_nozzle.position != _lastNozzlePos)
            {
                Vector3 movementDirection = (_nozzle.position - _lastNozzlePos).normalized;
                springForce = Vector3.SlerpUnclamped(springForceDirection, movementDirection, _movementInfluence);
                springForce *= Mathf.Min(totalForceMagnitude, _maxSpringForceMagnitude);
            }
            else
            {
                springForce = springForceDirection * Mathf.Min(totalForceMagnitude, _maxSpringForceMagnitude);
            }

            _currentTarget.AddForce(springForce);
            _lastNozzlePos = _nozzle.position;
        }

        protected override void StopUse()
        {
            _currentTarget = null;
            _ropeRenderer.enabled = false;
        }
    }
}