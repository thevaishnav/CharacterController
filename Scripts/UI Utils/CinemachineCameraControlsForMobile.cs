using System;
using UnityEngine;
using Cinemachine;
using Omnix.CCN.InputSystemWrapper;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Omnix.CCN.Utils
{
    [RequireComponent(typeof(Image))]
    public class CinemachineCameraControlsForMobile : UiInputBase
    {
        [SerializeField, Tooltip("If true then the object will move with input to act like joystick head")]
        private bool actLikeJoystick;

        [Header("Assign any one of the following")] 
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CinemachineFreeLook freeLookCamera;

        #if UNITY_ANDROID || UNITY_IPHONE
        private Action<float> _horizontalAxisSetter;
        private Action<float> _verticalAxisSetter;

        private void Start()
        {
            if (virtualCamera != null)
            {
                foreach (CinemachineComponentBase componentBase in virtualCamera.GetComponentPipeline())
                {
                    if (componentBase is CinemachinePOV pov)
                    {
                        pov.m_VerticalAxis.m_InputAxisName = string.Empty;
                        pov.m_HorizontalAxis.m_InputAxisName = string.Empty;
                        _horizontalAxisSetter = (value => pov.m_HorizontalAxis.Value += value);
                        _verticalAxisSetter = (value => pov.m_VerticalAxis.Value += value);
                        break;
                    }
                }
            }
            else if (freeLookCamera != null)
            {
                freeLookCamera.m_XAxis.m_InputAxisName = string.Empty;
                freeLookCamera.m_YAxis.m_InputAxisName = string.Empty;
                _horizontalAxisSetter = (value => freeLookCamera.m_XAxis.Value += value);
                _verticalAxisSetter = (value => freeLookCamera.m_YAxis.Value += value);
            }
        }
        #endif

        protected override void OnInputReceived(Vector2 anchorPosition, Vector2 input)
        {
            #if UNITY_ANDROID || UNITY_IPHONE
            _verticalAxisSetter?.Invoke(input.y);
            _horizontalAxisSetter?.Invoke(input.x);
            if (actLikeJoystick) reactTransform.anchoredPosition = anchorPosition;
            #endif
        }

        protected override void OnInputStopped(Vector2 anchorPosition, PointerEventData eventData)
        {
            if (actLikeJoystick) reactTransform.anchoredPosition = anchorPosition;
        }
    }
}