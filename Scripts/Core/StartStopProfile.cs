using System;
using UnityEngine;
using UnityEngine.UI;

namespace CCN.Core
{
    public class StartStopProfile : MonoBehaviour
    {
        public enum Behaviour
        {
            PressToToggle,
            ActiveWhilePressed
        }

        public enum TriggerType
        {
            GetButton = 0,
            GetKey = 2
        }

        [SerializeField, Tooltip("Behaviour for PC")]
        private Behaviour pcBehaviour;

        [SerializeField, Tooltip("Method from Input class, that will trigger start/stop")]
        private TriggerType pcTriggerType;

        [SerializeField, Tooltip("Button/Axis name")]
        private string pcButton;

        [SerializeField, Tooltip("Key Code")] 
        private KeyCode pcKeycode;

        [SerializeField, Tooltip("UI Button that will trigger start/stop for android")]
        private Button button;

        private IStartStopProfileTarget _target;

        #if UNITY_STANDALONE
        private Func<bool> _shouldStartCheck;
        private Func<bool> _shouldStopCheck;
        #endif


        private void Awake()
        {
            #if UNITY_IPHONE || UNITY_ANDROID
            if (button != null)
            {
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => {
                    if (_target == null) return;
                    if (_target.IsEnabled) _target.TryDisable();
                    else _target.TryEnable();
                });
            }
            #elif UNITY_STANDALONE
            enabled = _target != null;
            if (button) button.gameObject.SetActive(false);
            
            switch (pcBehaviour, pcTriggerType)
            {
                case (Behaviour.PressToToggle, TriggerType.GetKey):
                    _shouldStartCheck = () => Input.GetKeyUp(pcKeycode);
                    _shouldStopCheck = () => Input.GetKeyUp(pcKeycode);
                    break;
                case (Behaviour.PressToToggle, TriggerType.GetButton):
                    _shouldStartCheck = () => Input.GetButtonUp(pcButton);
                    _shouldStopCheck = () => Input.GetButtonUp(pcButton);
                    break;
                case (Behaviour.ActiveWhilePressed, TriggerType.GetKey):
                    _shouldStartCheck = () => Input.GetKeyDown(pcKeycode);
                    _shouldStopCheck = () => Input.GetKeyUp(pcKeycode);
                    break;
                case (Behaviour.ActiveWhilePressed, TriggerType.GetButton):
                    _shouldStartCheck = () => Input.GetButtonDown(pcButton);
                    _shouldStopCheck = () => Input.GetButtonUp(pcButton);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
            #endif
        }

        #if UNITY_STANDALONE
        public void Update()
        {
            if (_target.IsEnabled)
            {
                if (_shouldStopCheck())
                {
                    Debug.Log($"Disable: {_target.GetType().Name}");
                    _target.TryDisable();
                }
            }
            else
            {
                if (_shouldStartCheck())
                {
                    _target.TryEnable();
                }
            }
        }
        #endif

        public void SetTarget(IStartStopProfileTarget newTarget)
        {
            _target = newTarget;
            enabled = _target != null;
        }

        public void SetInfo(StartStopProfileInfo info)
        {
            pcBehaviour = info.pcBehaviour;
            pcTriggerType = info.pcTriggerType;
            pcButton = info.pcButton;
            pcKeycode = info.pcKeycode;
        }
    }
}