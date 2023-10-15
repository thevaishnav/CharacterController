using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CCN.InputSystemWrapper
{
    [RequireComponent(typeof(RectTransform))]
    public class InteractionTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        
        [SerializeField] private InteractionProfileBase profile;
        [SerializeField] private float maxMovementDistance;
        [SerializeField] private GameObject deactivateInWindows;

        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform reactTransform;
        
        public float AxisValue { get; private set; }
        public Action pointerDownCallback; 
        public Action pointerUpCallback;
        public Action<float> dragCallback;

        private Vector2 _startPosition;
        private Vector2 _startPointerPos;
        private float _axisMaxValue;
        
        /// <summary> 0 is horizontal, 1 is vertical </summary>
        private int _axis;

        private void Awake()
        {
            if (profile == null)
            {
                gameObject.SetActive(false);
                return;
            }

            #if UNITY_STANDALONE_WIN
            if (deactivateInWindows)
            {
                deactivateInWindows.SetActive(false);
                return;
            }
            #endif

            profile.SetButton(this);
            Vector2 move = profile.GetUiAxisIntensity();
            if (move.x > 0)
            {
                _axis = 0;
                _axisMaxValue = move.x;
            }
            else
            {
                _axis = 1;
                _axisMaxValue = move.y;
            }
            
        }

        private void Reset()
        {
            canvas = GetComponentInParent<Canvas>();
            reactTransform = GetComponent<RectTransform>();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _startPointerPos = eventData.position;
            _startPosition = reactTransform.anchoredPosition;
            AxisValue = 0f;
            pointerDownCallback?.Invoke();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            reactTransform.anchoredPosition = _startPosition;
            AxisValue = 0f;
            pointerUpCallback?.Invoke();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            AxisValue = 0f;
            Vector2 deltaFromStart = (eventData.position - _startPointerPos) / canvas.scaleFactor;
            if (deltaFromStart.magnitude > maxMovementDistance) deltaFromStart = maxMovementDistance * deltaFromStart.normalized;
            Vector2 newPos = deltaFromStart + _startPosition;

            if (_axis == 0)
            {
                newPos.y = reactTransform.anchoredPosition.y;
                AxisValue = deltaFromStart.x * _axisMaxValue / maxMovementDistance;
            }
            else
            {
                newPos.x = reactTransform.anchoredPosition.x;
                AxisValue = deltaFromStart.y * _axisMaxValue / maxMovementDistance;
            }
            
            reactTransform.anchoredPosition = newPos;
            dragCallback?.Invoke(AxisValue);
        }
    }
}