using CCN.Core;
using UnityEngine;

namespace CCN.Weapon
{
    public class GrapplingGun : AgentItem
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform gunTip;
        [SerializeField] private LayerMask whatIsGrappleable;
        [SerializeField] private Joint joint;
        [SerializeField] private float range = 100f;


        private RaycastHit _hit;

        private void Reset()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        protected override void Awake()
        {
            base.Awake();
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 2;
        }
        
        private void Update()
        {
            DrawRope();
        }

        private void DrawRope()
        {
            if (IsUsing == false)
            {
               Debug.DrawRay(gunTip.position, gunTip.forward, Color.red);
                return;
            }
            if (joint.connectedBody == null) return;
            
            lineRenderer.SetPosition(0, gunTip.position);
            lineRenderer.SetPosition(1, joint.connectedBody.position);
        }


        protected override void Equip()
        {
            
        }

        protected override void Unequip()
        {
        }

        protected override void StartUse()
        {
            if (!Physics.Raycast(gunTip.position, gunTip.forward, out _hit, range, whatIsGrappleable)) return;
            
            joint.connectedBody = _hit.rigidbody;
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = _hit.point;

            lineRenderer.enabled = true;
        }

        protected override void StopUse()
        {
            lineRenderer.enabled = false;
            joint.connectedBody = null;
        }
    }
}