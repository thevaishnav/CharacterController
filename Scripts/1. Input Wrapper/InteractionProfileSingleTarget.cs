using System;
using CCN.Core;
using UnityEngine;

namespace CCN.InputSystemWrapper
{
    [CreateAssetMenu(menuName = "CCN/Interaction Profile  (Single Target)", fileName = "New Interaction Profile")]
    public class InteractionProfileSingleTarget : InteractionProfileBase
    {
        [NonSerialized] private IInteractionProfileTarget _target;
        [NonSerialized] private Agent _targetAgent;

        private void CheckInteract()
        {
            CheckInteractionFor(_target);
        }

        public override void DoTarget(IInteractionProfileTarget target, Agent agent)
        {
            base.DoTarget(target, agent);
            
            if (_targetAgent != null)
            {
                #if UNITY_EDITOR || UNITY_STANDALONE
                if (pcMode != PcInteractionMode.DontInteract)
                    _targetAgent.EvUpdate -= CheckInteract;
                #endif
            }
            
            _target = target;
            _targetAgent = agent;
            #if UNITY_EDITOR || UNITY_STANDALONE
            if (pcMode != PcInteractionMode.DontInteract) 
                agent.EvUpdate += CheckInteract;
            #endif
        }

        protected override void ToggleInteractAllTargets()
        {
            if (_target == null) return;
            if (_target.IsInteracting(this)) _target.EndInteraction(this);
            else _target.StartInteraction(this);
        }

        protected override void InteractTarget()
        {
            if (_target == null) return;
            _target.StartInteraction(this);
        }

        protected override void UninteractTarget()
        {
            if (_target == null) return;
            _target.EndInteraction(this);
        }

        protected override void OnUiAxis(Vector2 value)
        {
            if (_target == null) return;
            bool aboveThreshold = value.sqrMagnitude > uiAxisThreshold;
            if (aboveThreshold)
            {
                if (_target.IsInteracting(this) == false)
                {
                    _target.StartInteraction(this);
                }
            }
            else
            {
                if (_target.IsInteracting(this) == true)
                {
                    _target.EndInteraction(this);
                }
            }
        }
    }
}