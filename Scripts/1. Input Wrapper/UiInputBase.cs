using System;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Omnix.CCN.InputSystemWrapper
{
    public abstract class UiInputBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum OverflowBehaviour
        {
            /// <summary> Act like we never overflown. Meaning count player's mouse movement as input </summary>
            [Tooltip("Act like we never overflown. Meaning count player's mouse movement as input")]
            Ignore,


            /// <summary> Zero the input in the direction of overflow, dont affect perpendicular direction </summary>
            [Tooltip("Zero the input in the direction of overflow, dont affect perpendicular direction")]
            StopDragging,


            /// <summary> Count the overflow amount as the input, and multiply it by force factor to control it. </summary>
            [Tooltip("Count the overflow amount as the input, and multiply it by force factor to control it.")]
            Pull
        }

        [SerializeField, Tooltip("Sensitivity for mobile (Will not affect axis-sensitivity set in Cinemachine component)")]
        public Vector2 sensitivityMultiplier = Vector2.one;

        [SerializeField, Tooltip("Choose how the input should behave when the user drags this object out of maxMovementDistance.")]
        private OverflowBehaviour overflowBehaviour;

        [SerializeField] protected float maxMovementDistance;
        [SerializeField] public float pullForce = 0.1f;
        [SerializeField] private Canvas canvas;
        [SerializeField] protected RectTransform reactTransform;

        public OverflowBehaviour Overflow
        {
            get => overflowBehaviour;
            set
            {
                overflowBehaviour = value;
                UpdateOverflowBehaviour();
            }
        }

        private bool _isDragging = false;
        private int _pointerId;
        private float _squareMoveDistance;
        private Vector2 _startPointerPos;
        private Vector2 _startPosition;
        private Vector2 _temp;
        private Vector2 _newAnchorPosition;
        private Vector2 _lastFrameAnchorRaw;
        private Vector2 _inputValue;
        private Func<Vector2, Vector2> _inputGetter;
        private Vector2 _inputAxisSmoothDamped;
        private Vector2 _inputAxisSmoothDampVel;
        
        /// <param name="anchorPosition"> New anchorPosition of the this object. reactTransform.anchorPosition to this value to move the object according to input. </param>
        /// <param name="input"> How much input is received from user. React to this value. </param>
        protected abstract void OnInputReceived(Vector2 anchorPosition, Vector2 input);

        protected abstract void OnInputStopped(Vector2 anchorPosition, PointerEventData eventData);

        protected virtual void OnEnable()
        {
            _squareMoveDistance = maxMovementDistance * maxMovementDistance;

            // Cache the input calculation method
            UpdateOverflowBehaviour();
        }

        protected virtual void Reset()
        {
            canvas = GetComponentInParent<Canvas>();
            reactTransform = GetComponent<RectTransform>();
        }

        protected virtual void Update()
        {
            if (_isDragging == false) return;

            if (_pointerId >= 0 && _pointerId < Input.touches.Length) _temp = Input.touches[_pointerId].position;
            else _temp = Input.mousePosition;

            _inputValue = _newAnchorPosition;
            _newAnchorPosition = (_temp - _startPointerPos) / canvas.scaleFactor;
            if (_newAnchorPosition.sqrMagnitude > _squareMoveDistance)
            {
                Vector2 correctedAnchor = maxMovementDistance * _newAnchorPosition.normalized;
                _temp = _startPosition + correctedAnchor;
                _inputValue = _inputGetter(correctedAnchor);
                /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
                 * Global variables have been reused to save space, but the logic is as follows                      *
                 * OverflowBehaviour.Ignore:                                                                         *
                 *     Act like we never overflown. Meaning count player's mouse movement as input                   *
                 *     = (currentMousePosition - lastFrameMousePosition)                                             *
                 *     = (currentMousePosition - startMousePosition) - (lastFrameMousePosition - startMousePosition) *
                 *     = _newAnchorPosition - _lastFrameAnchorRaw                                                    *
                 * OverflowBehaviour.StopDragging:                                                                   *
                 *     Zero the input in the direction of overflow, dont affect perpendicular direction              *
                 *     As the anchor position of the object is clamped within radius, we can use it.                 *
                 *     = currentAnchorPosition - lastFrameAnchorPosition                                             *
                 *     = _temp - _inputValue                                                                         *
                 * OverflowBehaviour.Pull:                                                                           *
                 *     Count the overflow amount as the input, and multiply it by a factor to control it.            *
                 *     = (currentMousePosition - currentAnchorPosition) * force                                      *
                 *     = (_newAnchorPosition - correctedAnchor) * force,                                             *
                \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
            }
            else
            {
                _temp = _newAnchorPosition + _startPosition;
                _inputValue = _newAnchorPosition - _inputValue;
            }

            _lastFrameAnchorRaw = _newAnchorPosition;
            _newAnchorPosition = _temp;
            _inputAxisSmoothDamped = Vector2.SmoothDamp(_inputAxisSmoothDamped, _inputValue, ref _inputAxisSmoothDampVel, 0.15f);
            OnInputReceived(_temp, _inputAxisSmoothDamped * sensitivityMultiplier);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = true;
            _startPointerPos = eventData.position;
            _startPosition = reactTransform.anchoredPosition;
            _pointerId = eventData.pointerId;
            _inputValue = Vector2.zero;
            _newAnchorPosition = _startPosition;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
            OnInputStopped(_startPosition, eventData);
        }

        public void UpdateOverflowBehaviour()
        {
            _inputGetter = overflowBehaviour switch
            {
                OverflowBehaviour.Ignore => (_ => _newAnchorPosition - _lastFrameAnchorRaw),
                OverflowBehaviour.StopDragging => (_ => _temp - _inputValue),
                OverflowBehaviour.Pull => (correctedAnchor => (_newAnchorPosition - correctedAnchor) * pullForce),
                _ => (_ => Vector2.zero)
            };
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UiInputBase), true)]
    public class UiDraggableBaseEditor : UnityEditor.Editor
    {
        private SerializedProperty _sensitivityMultiplier;
        private SerializedProperty _overflowBehaviour;
        private SerializedProperty _pullForce;
        private SerializedProperty _maxMovementDistance;
        private SerializedProperty _canvas;
        private SerializedProperty _reactTransform;
        private SerializedProperty _cached;

        private bool isExpanded;

        private void OnEnable()
        {
            _sensitivityMultiplier = serializedObject.FindProperty("sensitivityMultiplier");
            _maxMovementDistance = serializedObject.FindProperty("maxMovementDistance");
            _overflowBehaviour = serializedObject.FindProperty("overflowBehaviour");
            _pullForce = serializedObject.FindProperty("pullForce");
            _canvas = serializedObject.FindProperty("canvas");
            _reactTransform = serializedObject.FindProperty("reactTransform");
        }


        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_sensitivityMultiplier);
            EditorGUILayout.PropertyField(_maxMovementDistance);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_overflowBehaviour);
            bool updateBehaviour = EditorGUI.EndChangeCheck() && Application.isPlaying;
            switch (_overflowBehaviour.intValue)
            {
                case 0:
                    EditorGUILayout.HelpBox("Overflow behaviour: \n Act like we never overflown. Meaning count player's mouse movement as input", MessageType.Info);
                    break;
                case 1:
                    EditorGUILayout.HelpBox("Overflow behaviour: \n Zero the input in the direction of overflow, dont affect perpendicular direction", MessageType.Info);
                    break;
                case 2:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_pullForce);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.HelpBox("Overflow behaviour: \n Count the overflow amount as the input, and multiply it by force factor to control it.", MessageType.Info);
                    break;
            }

            isExpanded = EditorGUILayout.Foldout(isExpanded, "Automatically set");
            if (isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_canvas);
                EditorGUILayout.PropertyField(_reactTransform);
                EditorGUI.indentLevel--;
            }

            if (_canvas.objectReferenceValue == null) _canvas.objectReferenceValue = ((UiInputBase)target).transform.GetComponentInParent<Canvas>();
            RectTransform targetRt = ((UiInputBase)target).GetComponent<RectTransform>();
            if (_reactTransform.objectReferenceValue != targetRt) _reactTransform.objectReferenceValue = targetRt;

            EditorGUILayout.Space(10f);
            _cached = _reactTransform.Copy();
            while (_cached.Next(false))
            {
                EditorGUILayout.PropertyField(_cached, true);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            if (updateBehaviour) ((UiInputBase)target).UpdateOverflowBehaviour();
        }
    }
    #endif
}