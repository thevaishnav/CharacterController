using CCN.InputSystemWrapper;
using UnityEngine;

namespace CCN.Health
{
    [CreateAssetMenu(menuName = "CCN/Gun Specs", fileName = "Gun Specs")]
    public class GunSpecs : ScriptableObject
    {
        #region Used by Editor
        #if UNITY_EDITOR
        private float damageDistance;
        private float damageAmount => distanceToDamageCurve.Evaluate(damageDistance);

        private float spredDistance;
        private float spredAmount => distanceToSpreadCurve.Evaluate(spredDistance);
        #endif
        #endregion

        [Tooltip("Backward force experienced by player when the gun is fired")]
        public float recoilForce;
        
        [Tooltip("How much time it takes (in sec) to fire 1 shot.")]
        public float fireDuration;

        [Tooltip("How much time it takes (in sec) to reload once")]
        public float reloadDuration;

        [Tooltip("<= 0, means no reloading")] 
        public int magazineSize;
        
        [Tooltip("Should the gun automatically reload when the mag runs out..")]
        public bool autoReload;

        [Tooltip("Choose how the reloading should be handled")]
        public GunReloadType reloadType;
        
        [Tooltip("Animation curve to convert distance of hit-point from gun (time axis of curve) to damage (value axis of curve). \nPro Tip: You can right click the key.")]
        public AnimationCurve distanceToDamageCurve;

        [Tooltip("Animation curve to convert distance of hit-point from gun (time axis of curve) to shot in-accuracy (value axis of curve). \nPro Tip: You can right click the key.")]
        public AnimationCurve distanceToSpreadCurve;

        [Tooltip("How much further (on top of spread curve value) to spread the shot based on character move speed. <=0 values means don't account for move speed.")]
        public float speedInfluenceOnSpread;


        protected virtual void Reset()
        {
            distanceToDamageCurve = new AnimationCurve()
            {
                keys = new Keyframe[]
                {
                    new Keyframe(time: 0f, value: 4f, inTangent: Mathf.Infinity, outTangent: Mathf.Infinity, inWeight: 0f, outWeight: 0f),
                    new Keyframe(time: 15f, value: 3f, inTangent: Mathf.Infinity, outTangent: Mathf.Infinity, inWeight: 0f, outWeight: 0.333333f),
                    new Keyframe(time: 30f, value: 2f, inTangent: Mathf.Infinity, outTangent: Mathf.Infinity, inWeight: 0f, outWeight: 0.333333f),
                    new Keyframe(time: 45f, value: 1f, inTangent: Mathf.Infinity, outTangent: Mathf.Infinity, inWeight: 0f, outWeight: 0.333333f),
                    new Keyframe(time: 60f, value: 0f, inTangent: Mathf.Infinity, outTangent: Mathf.Infinity, inWeight: 0f, outWeight: 0f),
                }
            };


            distanceToSpreadCurve = new AnimationCurve()
            {
                keys = new Keyframe[]
                {
                    new Keyframe(time: 0f, value: 0f, inTangent: 0.02f, outTangent: 0.02f, inWeight: 0f, outWeight: 0.333333f),
                    new Keyframe(time: 60f, value: 1f, inTangent: 0.02f, outTangent: 0.02f, inWeight: 0.33333f, outWeight: 0.333333f),
                }
            };
        }
    }
}