using System;
using System.Collections.Generic;
using CCN.Core;
using UnityEngine;

namespace CCN.InputSystemWrapper
{
    [CreateAssetMenu(menuName = "CCN/Interaction Profile (Multiple Targets)", fileName = "New Interaction Profile")]
    public class InteractionProfileMultipleTargets : InteractionProfileBase
    {
        [NonSerialized] private List<IInteractionProfileTarget> _targets;

        protected override void Init()
        {
            base.Init();
            _targets = new List<IInteractionProfileTarget>();
        }
        
        public override void DoTarget(IInteractionProfileTarget target, Agent agent)
        {
            base.DoTarget(target, agent);

            _targets.Add(target);
            
            #if UNITY_STANDALONE_WIN
            agent.EvUpdate += () => CheckInteractionFor(target);
            #endif
        }

        protected override void ToggleInteractAllTargets()
        {
            if (_targets == null || _targets.Count == 0) return;

            foreach (IInteractionProfileTarget target in _targets)
            {
                if (target.IsInteracting(this)) target.EndInteraction(this);
                else target.StartInteraction(this);
            }
        }

        protected override void InteractTarget()
        {
            if (_targets == null || _targets.Count == 0) return;

            foreach (IInteractionProfileTarget target in _targets)
            {
                target.StartInteraction(this);
            }
        }

        protected override void UninteractTarget()
        {
            if (_targets == null || _targets.Count == 0) return;

            foreach (IInteractionProfileTarget target in _targets)
            {
                target.EndInteraction(this);
            }
        }

        protected override void OnUiAxis(float value)
        {
            if (_targets == null || _targets.Count == 0) return;

            bool aboveThreshold = value > uiAxisThreshold;

            foreach (IInteractionProfileTarget target in _targets)
            {
                if (aboveThreshold)
                {
                    if (target.IsInteracting(this) == false)
                    {
                        target.StartInteraction(this);
                    }
                }
                else
                {
                    if (target.IsInteracting(this) == true)
                    {
                        target.EndInteraction(this);
                    }
                }
            }
        }
    }
}