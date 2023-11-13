using System;
using System.Collections.Generic;
using Omnix.CCN.Core;
using UnityEngine;

namespace Omnix.CCN.InputSystemWrapper
{
    [CreateAssetMenu(menuName = "CCN/Interaction Profile (Multiple Targets)", fileName = "New Interaction Profile")]
    public class InteractionProfileMultipleTargets : InteractionProfileBase
    {
        [NonSerialized] private List<IInteraction> _targets;

        protected override void Init()
        {
            base.Init();
            _targets = new List<IInteraction>();
        }
        
        public override void DoTarget(IInteraction target, Agent agent)
        {
            base.DoTarget(target, agent);

            _targets.Add(target);
            
            #if UNITY_EDITOR || UNITY_STANDALONE
            agent.EvUpdate += () => CheckInteractionFor(target);
            #endif
        }

        protected override void ToggleInteractAllTargets()
        {
            if (_targets == null || _targets.Count == 0) return;

            foreach (IInteraction target in _targets)
            {
                if (target.IsInteracting()) target.EndInteraction();
                else target.StartInteraction();
            }
        }

        protected override void InteractAllTargets()
        {
            if (_targets == null || _targets.Count == 0) return;

            foreach (IInteraction target in _targets)
            {
                target.StartInteraction();
            }
        }

        protected override void UninteractAllTargets()
        {
            if (_targets == null || _targets.Count == 0) return;

            foreach (IInteraction target in _targets)
            {
                target.EndInteraction();
            }
        }

        protected override void OnUiAxis(Vector2 value)
        {
            if (_targets == null || _targets.Count == 0) return;

            bool aboveThreshold = value.sqrMagnitude > uiAxisThreshold;

            foreach (IInteraction target in _targets)
            {
                if (aboveThreshold)
                {
                    if (target.IsInteracting() == false)
                    {
                        target.StartInteraction();
                    }
                }
                else
                {
                    if (target.IsInteracting() == true)
                    {
                        target.EndInteraction();
                    }
                }
            }
        }
    }
}