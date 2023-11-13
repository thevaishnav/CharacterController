using Omnix.CCN.InputSystemWrapper;
using Omnix.CCN.Health;
using UnityEngine;

namespace Omnix.CCN.Items
{
    [CreateAssetMenu(menuName = "CCN/Gun Specs", fileName = "Gun Specs")]
    public class GunSpecs : ScriptableObject
    {
        private const string TT_RECOIL_FORCE = "Backward force experienced by player when the gun is fired";
        private const string TT_FIRE_DURATION = "How much time it takes (in sec) to fire 1 shot.";
        private const string TT_RELOAD_DURATION = "How much time it takes (in sec) to reload once";
        private const string TT_MAGAZINE_SIZE = "<= 0, means no reloading";
        private const string TT_AUTO_RELOAD = "Should the gun automatically reload when the mag runs out..";
        private const string TT_RELOAD_TYPE = "Choose how the reloading should be handled";
        private const string TT_DISTANCE_TO_DAMAGE_CURVE = "Animation curve to convert distance of hit-point from gun (time axis of curve) to damage (value axis of curve). \nPro Tip: You can right click the key.";
        private const string TT_DISTANCE_TO_SPREAD_CURVE = "Animation curve to convert distance of hit-point from gun (time axis of curve) to shot in-accuracy (value axis of curve). \nPro Tip: You can right click the key.";
        private const string TT_SPEED_INFLUENCE_ON_SPREAD = "How much further (on top of spread curve value) to spread the shot based on character move speed. <=0 values means don't account for move speed.";

        // @formatter:off
        [ Tooltip(TT_RECOIL_FORCE)              ] public float          recoilForce;
        [ Tooltip(TT_FIRE_DURATION)             ] public float          fireDuration;
        [ Tooltip(TT_RELOAD_DURATION)           ] public float          reloadDuration;
        [ Tooltip(TT_MAGAZINE_SIZE)             ] public int            magazineSize;
        [ Tooltip(TT_AUTO_RELOAD)               ] public bool           autoReload;
        [ Tooltip(TT_RELOAD_TYPE)               ] public GunReloadType  reloadType;
        [ Tooltip(TT_DISTANCE_TO_DAMAGE_CURVE)  ] public AnimationCurve distanceToDamageCurve;
        [ Tooltip(TT_DISTANCE_TO_SPREAD_CURVE)  ] public AnimationCurve distanceToSpreadCurve;
        [ Tooltip(TT_SPEED_INFLUENCE_ON_SPREAD) ] public float          speedInfluenceOnSpread;
        // @formatter:on


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