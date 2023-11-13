using System;
using MenuManagement.Base;
using Omnix.CCN.Core;
using Omnix.Utils;
using UnityEngine;

namespace Omnix.CCN.Items.ForceSources
{
    [Serializable]
    public class ForceSourceInfo
    {
        private const string TT_DISTANCE_INFLUENCE = "How does the force behave with distance. On time axis: 0 to 1 corresponding distance 0 to Range.";
        private const string TT_RANGE = "Maximum distance to which the gun can target anything";
        private const string TT_ATTACHMENT_FORCE = "Force applied when the grappling starts";
        private const string TT_PERSISTANT_FORCE = "Force applied every frame";
        private const string TT_LINQ_INFLUENCE = "Stiffness of the connecting link (How much should the gun movement affect the force applied to object). 0 is like rubber (gun movement has no influence on force), 1 means like a metal rod.";
        private const string TT_MAX_FORCE_MAGNITUDE = "Magnitude (ignoring sign) of the maximum force that can be applied on the attached object";

        // @formatter:off
        [Tooltip(TT_DISTANCE_INFLUENCE)                   ]       public AnimationCurve DistanceInfluence;
        [Tooltip(TT_RANGE)                                ]       public float          Range = 20f;
        [Tooltip(TT_ATTACHMENT_FORCE)                     ]       public float          AttachmentForce = 10f;
        [Tooltip(TT_PERSISTANT_FORCE)                     ]       public float          PersistantForce = -3f;
        [Tooltip(TT_MAX_FORCE_MAGNITUDE)                  ]       public float          MaxForceMagnitude = 20f;
        [Tooltip(TT_LINQ_INFLUENCE), Range(-0f, 1f)       ]       public float          LinqInfluence = 0f;
        // @formatter:on

        public float ForceMagnitudeAtPoint(float distanceFromOrigin) => DistanceInfluence.Evaluate(distanceFromOrigin / Range) * MaxForceMagnitude;
        public Vector3 PersistantForceAtPoint(Vector3 pointDistanceFromOrigin, Vector3 nozzleOffset) => GetForceAtPoint(pointDistanceFromOrigin, nozzleOffset, PersistantForce);
        public Vector3 AttachmentForceAtPoint(Vector3 pointDistanceFromOrigin, Vector3 nozzleOffset) => GetForceAtPoint(pointDistanceFromOrigin, nozzleOffset, AttachmentForce);

        private Vector3 GetForceAtPoint(Vector3 point, Vector3 nozzleOffset, float multiplier)
        {
            Vector3 springForce = point;
            Vector3 springForceDirection = springForce.normalized;
            float totalForceMagnitude = DistanceInfluence.Evaluate(springForce.magnitude / Range) * MaxForceMagnitude * multiplier;

            if (LinqInfluence > 0.001f && nozzleOffset.sqrMagnitude > 0.001f)
            {
                Vector3 movementDirection = nozzleOffset.normalized;
                springForce = Vector3.SlerpUnclamped(springForceDirection, movementDirection, LinqInfluence);
                springForce *= Mathf.Min(totalForceMagnitude, MaxForceMagnitude);
                return springForce;
            }

            return springForceDirection * Mathf.Min(totalForceMagnitude, MaxForceMagnitude);
        }
    }


    /// <summary>
    /// Represent item that affects multiple object from distance (By adding force to them).
    /// Example: Magnet, Wind Turbine, or maybe Psychic Ability.
    /// </summary>
    [GroupProperties(_.FORCE_SOURCE, "Nozzle", "CameraTransform", "LayerMask", "FieldOfView", "MaxSimultaneousTargets", "sourceInfo")]
    public class ForceSourceMultiTargets : AgentItem
    {
        // @formatter:off
        #region ToolTips
        private const string TT_NOZZLE = "Point at which the rope is attached to the gun";
        private const string TT_CAMERA_TRANSFORM = "Camera transform, used for rayCast";
        private const string TT_LAYER_MASK = "What is grapple-able";
        private const string TT_FORCE_SOURCE_INFO = "Maximum number of objects this item can affect at once";
        private const string TT_FIELD_OF_VIEW = "Half angle (in radiens) of the field of view";
        private const string TT_MAX_SIMULTANEOUS_TARGETS = "Maximum number of objects this item can affect at once";
        #endregion

        #region Fields
        [ SerializeField,  Tooltip(TT_NOZZLE)                               ]       protected Transform       Nozzle;
        [ SerializeField,  Tooltip(TT_CAMERA_TRANSFORM)                     ]       protected Transform       CameraTransform;
        [ SerializeField,  Tooltip(TT_LAYER_MASK)                           ]       protected LayerMask       LayerMask;
        [ SerializeField,  Tooltip(TT_FORCE_SOURCE_INFO)                    ]       protected ForceSourceInfo sourceInfo;
        [ SerializeField,  Tooltip(TT_FIELD_OF_VIEW), Range(0f, 1.560f)     ]       protected float           FieldOfView = Mathf.PI;
        [ SerializeField,  Tooltip(TT_MAX_SIMULTANEOUS_TARGETS)             ]       protected int             MaxSimultaneousTargets = 3;
        

        private Collider[] _hitResults;       // Result of SphereCastNonAlloc
        private Rigidbody[] _currentTargets;  // Objects that currently being controlled by this item
        private int _hitCount;                // Number of objects SphereCastNonAlloc hit in this frame
        private int _currentTargetsCount = 0; // Number of objects currently being controlled by this item
        private Vector3 _lastNozzlePos;
        // @formatter:on


        /// <summary> How does the force behave with distance. On time axis: 0 to 1 corresponding distance 0 to Range. </summary>
        public AnimationCurve DistanceInfluence
        {
            get => sourceInfo.DistanceInfluence;
            set => sourceInfo.DistanceInfluence = value;
        }

        /// <summary> Maximum distance to which the gun can target anything </summary>
        public float Range
        {
            get => sourceInfo.Range;
            set => sourceInfo.Range = value;
        }

        /// <summary> Force applied when the grappling starts </summary>
        public float AttachmentForce
        {
            get => sourceInfo.AttachmentForce;
            set => sourceInfo.AttachmentForce = value;
        }

        /// <summary> Force applied every frame </summary>
        public float PersistantForce
        {
            get => sourceInfo.PersistantForce;
            set => sourceInfo.PersistantForce = value;
        }

        /// <summary> Stiffness of the connecting link (How much should the gun movement affect the force applied to object). 0 is like rubber (gun movement has no influence on force), 1 means like a metal rod. </summary>
        public float MaxForceMagnitude
        {
            get => sourceInfo.MaxForceMagnitude;
            set => sourceInfo.MaxForceMagnitude = value;
        }

        /// <summary> Magnitude (ignoring sign) of the maximum force that can be applied on the attached object </summary>
        public float LinqInfluence
        {
            get => sourceInfo.LinqInfluence;
            set => sourceInfo.LinqInfluence = value;
        }
        #endregion

        #region Empty Members
        // @formatter:off
        protected virtual void OnStartUse(){}
        protected virtual void OnStopUse(){}
        protected override void Equip(){}
        protected override void Unequip(){}
        #endregion
        // @formatter:on

        #region Functionalities
        private void DoRayCast()
        {
            if (_currentTargetsCount >= MaxSimultaneousTargets)
            {
                return;
            }

            _hitCount = PhysicsUtils.OverlapConeNonAlloc(CameraTransform.position, CameraTransform.forward, sourceInfo.Range, FieldOfView, _hitResults, LayerMask);


            for (int i = 0; i < _hitCount; i++)
            {
                if (_currentTargetsCount >= MaxSimultaneousTargets)
                {
                    break;
                }

                Collider hit = _hitResults[i];
                if (hit.TryGetComponent(out Rigidbody target) == false)
                {
                    continue;
                }

                if (IsTargeting(target))
                {
                    continue;
                }

                _currentTargets[_currentTargetsCount] = target;
                _currentTargetsCount++;
                target.AddForce(sourceInfo.AttachmentForceAtPoint(target.transform.position - Nozzle.position, Vector3.zero));
            }
        }

        private void AddPersistantForceOnAllTargets()
        {
            if (IsUsing == false) return;
            if (_currentTargetsCount == 0) return;

            Vector3 nozzleOffset = Nozzle.position - _lastNozzlePos;
            for (int i = 0; i < _currentTargetsCount; i++)
            {
                Rigidbody target = _currentTargets[i];
                target.AddForce(sourceInfo.PersistantForceAtPoint(target.transform.position - Nozzle.position, nozzleOffset));
            }

            _lastNozzlePos = Nozzle.position;
        }

        protected override void Awake()
        {
            base.Awake();
            _hitResults = new Collider[MaxSimultaneousTargets * 2];
            _currentTargets = new Rigidbody[MaxSimultaneousTargets];
        }

        protected virtual void FixedUpdate()
        {
            if (IsUsing == false)
            {
                return;
            }

            DoRayCast();
            AddPersistantForceOnAllTargets(); // Changed to plural
        }

        protected override void StartUse()
        {
            _lastNozzlePos = Nozzle.position;
            _currentTargetsCount = 0;
            if (_currentTargets == null) _currentTargets = new Rigidbody[MaxSimultaneousTargets];
            else
            {
                for (int i = 0; i < MaxSimultaneousTargets; i++)
                {
                    _currentTargets[i] = null;
                }
            }

            DoRayCast();
            OnStartUse();
        }

        protected override void StopUse()
        {
            _currentTargetsCount = 0;
            OnStopUse();
        }

        /// <returns> Is rigidbody being controlled by this item </returns>
        public bool IsTargeting(Rigidbody toCheck)
        {
            if (toCheck == null) return false;

            for (int i = 0; i < _currentTargetsCount; i++)
            {
                if (_currentTargets[i] == toCheck)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary> Try to get a target by index </summary>
        /// <returns> true if provided index was valid, and the found target at that index is not null, false otherwise. </returns>
        public bool TryGetTargetAtIndex(int index, out Rigidbody target)
        {
            // The check will automatically fail if _currentTargetsCount is 0 (i.e. nothing is being targeted)
            if (index < 0 || index >= _currentTargetsCount)
            {
                target = null;
                return false;
            }

            target = _currentTargets[index];
            return target != null;
        }

        private void OnDrawGizmosSelected()
        {
            if (CameraTransform != null)
                GizmosUtils.DrawCone(CameraTransform.position, CameraTransform.forward, sourceInfo.Range, FieldOfView, 10, Color.yellow);
        }
        #endregion
    }


    /// <summary>
    /// Represent item that affects a single object from distance (By adding force to it).
    /// Example: Grappling Gun, Jet Spray, or maybe Psychic Ability.
    /// </summary>
    public class ForceSourceSingleTarget : AgentItem
    {
        // @formatter:off
        #region ToolTips
        private const string TT_NOZZLE = "Point at which the rope is attached to the gun";
        private const string TT_CAMERA_TRANSFORM = "Camera transform, used for rayCast";
        private const string TT_LAYER_MASK = "What is grapple-able";
        private const string TT_RANGE = "Maximum distance to which the gun can target anything";
        private const string TT_ATTACHMENT_FORCE = "Force applied when the grappling starts";
        private const string TT_SPRING_FORCE = "Force applied every frame";
        private const string TT_SPRING_STIFFNESS = "How much should the gun movement influence spring force. 0 means ignore movement, 1 means apply force in direction of movement.";
        #endregion

        #region Fields
        [Header("ForceSource")]
        [ SerializeField, Tooltip(TT_NOZZLE)                          ]    protected Transform Nozzle;
        [ SerializeField, Tooltip(TT_CAMERA_TRANSFORM)                ]    protected Transform CameraTransform;
        [ SerializeField, Tooltip(TT_LAYER_MASK)                      ]    protected LayerMask LayerMask;
        [ SerializeField, Tooltip(TT_LAYER_MASK)                      ]    protected ForceSourceInfo sourceInfo;
       
        protected Rigidbody CurrentTarget;
        private RaycastHit _hit;
        private Vector3 _lastNozzlePos;
        // @formatter:on


        /// <summary> How does the force behave with distance. On time axis: 0 to 1 corresponding distance 0 to Range. </summary>
        public AnimationCurve DistanceInfluence
        {
            get => sourceInfo.DistanceInfluence;
            set => sourceInfo.DistanceInfluence = value;
        }

        /// <summary> Maximum distance to which the gun can target anything </summary>
        public float Range
        {
            get => sourceInfo.Range;
            set => sourceInfo.Range = value;
        }

        /// <summary> Force applied when the grappling starts </summary>
        public float AttachmentForce
        {
            get => sourceInfo.AttachmentForce;
            set => sourceInfo.AttachmentForce = value;
        }

        /// <summary> Force applied every frame </summary>
        public float PersistantForce
        {
            get => sourceInfo.PersistantForce;
            set => sourceInfo.PersistantForce = value;
        }

        /// <summary> Stiffness of the connecting link (How much should the gun movement affect the force applied to object). 0 is like rubber (gun movement has no influence on force), 1 means like a metal rod. </summary>
        public float MaxForceMagnitude
        {
            get => sourceInfo.MaxForceMagnitude;
            set => sourceInfo.MaxForceMagnitude = value;
        }

        /// <summary> Magnitude (ignoring sign) of the maximum force that can be applied on the attached object </summary>
        public float LinqInfluence
        {
            get => sourceInfo.LinqInfluence;
            set => sourceInfo.LinqInfluence = value;
        }
        #endregion

        #region Empty Members
        // @formatter:off
        protected virtual void OnStartUse() {}
        protected virtual void OnStopUse() {}
        protected override void Equip() {}
        protected override void Unequip() {}
        #endregion
        // @formatter:on

        #region Functionalities
        protected virtual void FixedUpdate()
        {
            if (!IsUsing || CurrentTarget == null) return;

            CurrentTarget.AddForce(sourceInfo.PersistantForceAtPoint(CurrentTarget.transform.position - Nozzle.position, Nozzle.position - _lastNozzlePos));
            _lastNozzlePos = Nozzle.position;
        }

        protected override void StartUse()
        {
            if (Physics.Raycast(CameraTransform.position, CameraTransform.forward, out _hit, sourceInfo.Range, LayerMask))
            {
                CurrentTarget = _hit.rigidbody;
                _lastNozzlePos = Nozzle.position;
                CurrentTarget.AddForce(sourceInfo.AttachmentForceAtPoint(CurrentTarget.transform.position - _lastNozzlePos, Vector3.zero));
                OnStartUse();
            }
            else
            {
                TryStopUse();
            }
        }

        protected override void StopUse()
        {
            CurrentTarget = null;
            OnStopUse();
        }
        #endregion
    }
}