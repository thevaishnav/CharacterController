using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CCN.InputSystemWrapper
{
    [RequireComponent(typeof(RectTransform))]
    public class InteractionTrigger : UiInputBase
    {
        [SerializeField] private InteractionProfileBase target;

        public Vector2 AxisValue => _axisValue;
        private Vector2 _maxValue;
        private Vector2 _axisValue;
        
        
        public Action PointerDownCallback; 
        public Action PointerUpCallback;
        public Action<Vector2> DragCallback;
        

        private void Start()
        {
            if (target == null)
            {
                gameObject.SetActive(false);
                return;
            }

            target.SetButton(this);
            _maxValue = target.GetUiAxisIntensity();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _axisValue = Vector2.zero;
            PointerDownCallback?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _axisValue = Vector2.zero;
            PointerUpCallback?.Invoke();
        }

        protected override void OnInputReceived(Vector2 anchorPosition, Vector2 input)
        {
            reactTransform.anchoredPosition = anchorPosition;
            _axisValue += (input * _maxValue) / maxMovementDistance;
            DragCallback?.Invoke(_axisValue);
        }

        protected override void OnInputStopped(Vector2 anchorPosition, PointerEventData eventData)
        {
            OnInputReceived(anchorPosition, Vector2.zero);
        }
    }
}