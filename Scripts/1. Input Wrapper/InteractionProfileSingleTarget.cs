using System;
using Omnix.CCN.Core;
using UnityEngine;

namespace Omnix.CCN.InputSystemWrapper
{
    [CreateAssetMenu(menuName = "CCN/Interaction Profile  (Single Target)", fileName = "New Interaction Profile")]
    public class InteractionProfileSingleTarget : InteractionProfileBase
    {
        [NonSerialized] private IInteraction _target;
        [NonSerialized] private Agent _targetAgent;

        private void CheckInteract()
        {
            CheckInteractionFor(_target);
        }

        public override void DoTarget(IInteraction target, Agent agent)
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
            if (_target.IsInteracting()) _target.EndInteraction();
            else _target.StartInteraction();
        }

        protected override void InteractAllTargets()
        {
            if (_target == null) return;
            _target.StartInteraction();
        }

        protected override void UninteractAllTargets()
        {
            if (_target == null) return;
            _target.EndInteraction();
        }

        protected override void OnUiAxis(Vector2 value)
        {
            if (_target == null) return;
            bool aboveThreshold = value.sqrMagnitude > uiAxisThreshold;
            if (aboveThreshold)
            {
                if (_target.IsInteracting() == false)
                {
                    _target.StartInteraction();
                }
            }
            else
            {
                if (_target.IsInteracting() == true)
                {
                    _target.EndInteraction();
                }
            }
        }
    }
}