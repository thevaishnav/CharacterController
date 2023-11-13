using UnityEngine;
using UnityEngine.UI;

namespace Omnix.CCN.Utils
{
    /// <summary> A Unity component for creating a procedural cross. </summary>
    public class ProceduralCross : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Image _centerImage;

        [SerializeField] private Image _leftImage;
        [SerializeField] private Image _rightImage;
        [SerializeField] private Image _upImage;
        [SerializeField] private Image _downImage;


        [Header("Settings")] [SerializeField] private float _centerSize;
        [SerializeField] private float _armLength;
        [SerializeField] private float _armThickness;
        [SerializeField] private float _armOffset;
        [SerializeField] private bool _showArms;
        [SerializeField] private Color _centerColor;
        [SerializeField] private Color _armColor;

        private RectTransform _centerRect;
        private RectTransform _leftRect;
        private RectTransform _rightRect;
        private RectTransform _upRect;
        private RectTransform _downRect;

        /// <summary> Size of the center part of the cross. </summary>
        public float CenterSize
        {
            get => _centerSize;
            set
            {
                if (_centerSize < 0) return;
                _centerSize = value;
                _centerRect.sizeDelta = Vector2.one * _centerSize;
                SetArmPosition(_armOffset);
            }
        }

        /// <summary> Whether the arms of the cross are visible. </summary>
        public bool ShowArm
        {
            get => _showArms;
            set => SetArmActive(value);
        }

        /// <summary> Length of the cross arms. </summary>
        public float ArmLength
        {
            get => _armLength;
            set => SetArmSize(value, _armThickness);
        }

        /// <summary> Thickness of the cross arms. </summary>
        public float ArmThickness
        {
            get => _armThickness;
            set => SetArmSize(_armLength, value);
        }

        /// <summary> Offset of the cross arms from the center. </summary>
        public float ArmOffset
        {
            get => _armOffset;
            set => SetArmPosition(value);
        }

        /// <summary> Color of the cross arms. </summary>
        public Color ArmColor
        {
            get => _armColor;
            set => SetArmColors(value);
        }

        /// <summary> Vector2 with x-component representing Length & y-component representing ArmThickness. Its faster to use this if you want to set both. </summary>
        public Vector2 ArmSize
        {
            get => new Vector2(_armLength, _armThickness);
            set => SetArmSize(value.x, value.y);
        }

        
        /// <summary> Color of the center part of the cross. </summary>
        public Color CenterColor
        {
            get => _centerColor;
            set
            {
                _centerColor = value;
                _centerImage.color = value;
            }
        }
        

        #if UNITY_EDITOR
        /// <summary> Called when the component is added in the Unity Editor. Initializes default values and settings. </summary>
        private void Reset()
        {
            _centerImage = new GameObject("center").AddComponent<Image>();
            _leftImage = new GameObject("left").AddComponent<Image>();
            _rightImage = new GameObject("right").AddComponent<Image>();
            _upImage = new GameObject("up").AddComponent<Image>();
            _downImage = new GameObject("down").AddComponent<Image>();

            _centerImage.transform.SetParent(transform);
            _leftImage.transform.SetParent(transform);
            _rightImage.transform.SetParent(transform);
            _upImage.transform.SetParent(transform);
            _downImage.transform.SetParent(transform);

            _centerSize = 7f;
            _armLength = 15f;
            _armThickness = 7f;
            _armOffset = 5f;
            _showArms = true;
            _centerColor = Color.white;
            _armColor = Color.white;


            ValidateRectTransforms();
            TotalRefresh();
        }

        /// <summary> Called when any value in the editor is changed. Updates the cross's appearance in the editor. </summary>
        private void OnValidate()
        {
            ValidateRectTransforms();
            TotalRefresh();
        }
        #endif

        /// <summary>  </summary>
        private void Awake()
        {
            ValidateRectTransforms();
            TotalRefresh();
        }

        /// <summary> Makes sure that all rectTransforms are set properly. </summary>
        private void ValidateRectTransforms()
        {
            _centerRect = _centerImage.GetComponent<RectTransform>();
            _leftRect = _leftImage.GetComponent<RectTransform>();
            _rightRect = _rightImage.GetComponent<RectTransform>();
            _upRect = _upImage.GetComponent<RectTransform>();
            _downRect = _downImage.GetComponent<RectTransform>();
            
            var half = new Vector2(0.5f, 0.5f);
            _centerRect.anchorMin = half;
            _centerRect.anchorMax = half;
            _centerRect.anchoredPosition = Vector2.zero;
            _centerRect.localScale = Vector3.one;

            _leftRect.anchorMin = half;
            _leftRect.anchorMax = half;
            _leftRect.localScale = Vector3.one;

            _rightRect.anchorMin = half;
            _rightRect.anchorMax = half;
            _rightRect.localScale = Vector3.one;

            _upRect.anchorMin = half;
            _upRect.anchorMax = half;
            _upRect.localScale = Vector3.one;

            _downRect.anchorMin = half;
            _downRect.anchorMax = half;
            _downRect.localScale = Vector3.one;
        }

        /// <summary> Sets the color of the cross arms. </summary>
        private void SetArmColors(Color color)
        {
            _armColor = color;
            _leftImage.color = _armColor;
            _rightImage.color = _armColor;
            _upImage.color = _armColor;
            _downImage.color = _armColor;
        }

        /// <summary> Sets the position of the cross arms based on the given offset. </summary>
        private void SetArmPosition(float offset)
        {
            if (offset < 0) return;
            
            _armOffset = offset;
            float posOffset = (_centerSize + _armLength) * 0.5f + _armOffset;
            _leftRect.anchoredPosition = new Vector2(-posOffset, 0f);
            _rightRect.anchoredPosition = new Vector2(posOffset, 0f);
            _upRect.anchoredPosition = new Vector2(0f, posOffset);
            _downRect.anchoredPosition = new Vector2(0f, -posOffset);
        }

        /// <summary> Sets whether the arms of the cross are visible. </summary>
        private void SetArmActive(bool value)
        {
            _showArms = value;
            _leftImage.enabled = _showArms;
            _rightImage.enabled = _showArms;
            _upImage.enabled = _showArms;
            _downImage.enabled = _showArms;
        }

        /// <summary> Sets length & thickness of the arms  </summary>
        public void SetArmSize(float length, float thickness)
        {
            if (length <= 0 || thickness <= 0) return;
            
            _armLength = length;
            _armThickness = thickness;
            _leftRect.sizeDelta = new Vector2(_armLength, _armThickness);
            _rightRect.sizeDelta = new Vector2(_armLength, _armThickness);
            _upRect.sizeDelta = new Vector2(_armThickness, _armLength);
            _downRect.sizeDelta = new Vector2(_armThickness, _armLength);
            SetArmPosition(_armOffset);
        }

        /// <summary> Refreshes all the attributes of the cursor </summary>
        public void TotalRefresh()
        {
            _centerImage.color = _centerColor;
            _centerRect.sizeDelta = Vector2.one * _centerSize;
            SetArmActive(_showArms);

            if (_showArms)
            {
                SetArmColors(_armColor);
                SetArmSize(_armLength, _armThickness); // Will call SetArmPosition
            }
        }

        /// <summary> Refreshes all the attributes of the cursor </summary>
        public void TotalRefresh(bool showArms, float centerSize = -1f, float armLength = -1f, float armThickness = -1f, float armOffset = -1f, Color centerColor = default, Color armColor = default)
        {
            if (centerSize > 0) _centerRect.sizeDelta = Vector2.one * _centerSize;
            if (centerColor != default) _centerImage.color = _centerColor;
            SetArmActive(showArms);

            if (showArms)
            {
                if (armColor != default) SetArmColors(armColor);
                if (armOffset >= 0) _armOffset = armOffset;
                SetArmSize(armLength, armThickness); // Will call SetArmPosition
            }
        }
    }
}