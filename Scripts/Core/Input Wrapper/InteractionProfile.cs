using System;
using CCN.Core;
using UnityEngine;

namespace CCN.InputSystemWrapper
{
    public abstract class InteractionProfileBase : ScriptableObject
    {
        protected enum PcInteractionMode
        {
            DontInteract = 0,
            PressToToggle = 1,
            ActiveWhilePressed = 2
        }

        protected enum PcInputMode
        {
            GetKey = 0,
            GetButton = 1,
            GetAxis = 2
        }

        protected enum UiInteractionMode
        {
            PressToToggle = 0,
            ActiveWhilePressed = 1,
            HorizontalDrag = 2,
            VerticalDrag = 3
        }

        [SerializeField] protected bool tryStartInAwake;
        [SerializeField] protected PcInteractionMode pcMode;

        [SerializeField] protected PcInputMode pcInputType;
        [SerializeField] protected string pcTriggerName;
        [SerializeField] protected float pcAxisThreshold;
        [SerializeField] protected KeyCode pcKeycode;

        [SerializeField] protected UiInteractionMode uiMode;
        [SerializeField] protected float uiAxisIntensity;
        [SerializeField] protected float uiAxisThreshold;


        [NonSerialized] private bool _hasInitialized;
        [NonSerialized] private bool _hasButton;
        [NonSerialized] protected InteractionTrigger button;
        [NonSerialized] protected Func<bool> shouldStopCheck;
        [NonSerialized] protected Func<bool> shouldStartCheck;

        // protected bool fetchAxisValueFromTrigger;


        protected abstract void OnUiAxis(float value);
        protected abstract void UninteractTarget();
        protected abstract void InteractTarget();
        protected abstract void ToggleInteractAllTargets();


        public Vector2 GetUiAxisIntensity()
        {
            return uiMode switch
            {
                UiInteractionMode.HorizontalDrag => new Vector2(uiAxisIntensity, 0f),
                UiInteractionMode.VerticalDrag => new Vector2(0f, uiAxisIntensity),
                _ => Vector2.zero
            };
        }

        public float GetAxisValue()
        {
            #if UNITY_STANDALONE
            if (_hasButton) return (button.AxisValue + Input.GetAxisRaw(pcTriggerName)) * 0.5f;
            return Input.GetAxisRaw(pcTriggerName);
            #endif
            
            return _hasButton ? button.AxisValue : 0f;
        }

        public void SetButton(InteractionTrigger newButton)
        {
            if (newButton == null) return;

            _hasButton = true;
            button = newButton;
            switch (uiMode)
            {
                case UiInteractionMode.PressToToggle:
                    button.pointerDownCallback = ToggleInteractAllTargets;
                    break;
                case UiInteractionMode.ActiveWhilePressed:
                    button.pointerDownCallback = InteractTarget;
                    button.pointerUpCallback = UninteractTarget;
                    break;
                case UiInteractionMode.HorizontalDrag:
                    button.dragCallback = OnUiAxis;
                    button.pointerUpCallback = UninteractTarget;
                    break;
                case UiInteractionMode.VerticalDrag:
                    button.dragCallback = OnUiAxis;
                    button.pointerUpCallback = UninteractTarget;
                    break;
            }
        }

        public virtual void DoTarget(IInteractionProfileTarget target, Agent agent)
        {
            if (_hasInitialized == false)
            {
                Init();
                _hasInitialized = true;
            }

            if (tryStartInAwake) target.StartInteraction(this);
        }

        protected virtual void Init()
        {
            switch (pcMode, pcInputType)
            {
                case (PcInteractionMode.PressToToggle, PcInputMode.GetKey):
                    shouldStartCheck = () => Input.GetKeyUp(pcKeycode);
                    shouldStopCheck = () => Input.GetKeyUp(pcKeycode);
                    break;
                case (PcInteractionMode.PressToToggle, PcInputMode.GetButton):
                    shouldStartCheck = () => Input.GetButtonUp(pcTriggerName);
                    shouldStopCheck = () => Input.GetButtonUp(pcTriggerName);
                    break;
                case (PcInteractionMode.PressToToggle, PcInputMode.GetAxis):
                    shouldStartCheck = () => Mathf.Abs(Input.GetAxis(pcTriggerName)) > pcAxisThreshold;
                    shouldStopCheck = () => Mathf.Abs(Input.GetAxis(pcTriggerName)) > pcAxisThreshold;
                    break;
                case (PcInteractionMode.ActiveWhilePressed, PcInputMode.GetKey):
                    shouldStartCheck = () => Input.GetKeyDown(pcKeycode);
                    shouldStopCheck = () => Input.GetKey(pcKeycode) == false;
                    break;
                case (PcInteractionMode.ActiveWhilePressed, PcInputMode.GetButton):
                    shouldStartCheck = () => Input.GetButtonDown(pcTriggerName);
                    shouldStopCheck = () => Input.GetButton(pcTriggerName) == false;
                    break;
                case (PcInteractionMode.ActiveWhilePressed, PcInputMode.GetAxis):
                    shouldStartCheck = () => Mathf.Abs(Input.GetAxis(pcTriggerName)) > pcAxisThreshold;
                    shouldStopCheck = () => Mathf.Abs(Input.GetAxis(pcTriggerName)) < pcAxisThreshold;
                    break;
                case (PcInteractionMode.DontInteract, _):
                {
                    shouldStartCheck = () => false;
                    shouldStopCheck = () => false;
                    break;
                }
                    ;
            }
        }

        protected void CheckInteractionFor(IInteractionProfileTarget target)
        {
            if (target.IsInteracting(this))
            {
                if (shouldStopCheck())
                {
                    target.EndInteraction(this);
                }
            }
            else
            {
                if (shouldStartCheck())
                {
                    target.StartInteraction(this);
                }
            }
        }
    }
}