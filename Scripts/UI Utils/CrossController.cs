using System;
using UnityEngine;
using UnityEngine.UI;

namespace CCN.Utils
{
    public class CrossController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Image centerImage;

        [SerializeField] private Image leftImage;
        [SerializeField] private Image rightImage;
        [SerializeField] private Image upImage;
        [SerializeField] private Image downImage;


        [Header("Settings")] [SerializeField] private float centerSize;
        [SerializeField] private float sidesLength;
        [SerializeField] private float sidesWidth;
        [SerializeField] private float sidesOffset;
        [SerializeField] private bool showSides;
        [SerializeField] private Color centerColor;
        [SerializeField] private Color sidesColor;

        private RectTransform _centerRect;
        private RectTransform _leftRect;
        private RectTransform _rightRect;
        private RectTransform _upRect;
        private RectTransform _downRect;

        public float CenterSize
        {
            get => centerSize;
            set
            {
                centerSize = value;
                RefreshCursor();
            }
        }

        public float SidesLength
        {
            get => sidesLength;
            set
            {
                sidesLength = value;
                RefreshCursor();
            }
        }

        public float SidesWidth
        {
            get => sidesWidth;
            set
            {
                sidesWidth = value;
                RefreshCursor();
            }
        }

        public float SidesOffset
        {
            get => sidesOffset;
            set
            {
                sidesOffset = value;
                RefreshCursor();
            }
        }

        public bool ShowSides
        {
            get => showSides;
            set
            {
                showSides = value;
                RefreshCursor();
            }
        }

        public Color CenterColor
        {
            get => centerColor;
            set
            {
                centerColor = value;
                RefreshCursor();
            }
        }

        public Color SidesColor
        {
            get => sidesColor;
            set
            {
                sidesColor = value;
                RefreshCursor();
            }
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            centerImage = new GameObject("center").AddComponent<Image>();
            leftImage = new GameObject("left").AddComponent<Image>();
            rightImage = new GameObject("right").AddComponent<Image>();
            upImage = new GameObject("up").AddComponent<Image>();
            downImage = new GameObject("down").AddComponent<Image>();

            centerImage.transform.SetParent(transform);
            leftImage.transform.SetParent(transform);
            rightImage.transform.SetParent(transform);
            upImage.transform.SetParent(transform);
            downImage.transform.SetParent(transform);

            centerSize = 7f;
            sidesLength = 15f;
            sidesWidth = 7f;
            sidesOffset = 5f;
            showSides = true;
            centerColor = Color.white;
            sidesColor = Color.white;


            GetRectTransforms();
            RefreshCursor();
        }

        private void OnValidate()
        {
            GetRectTransforms();
            RefreshCursor();
        }
        #endif

        private void Awake()
        {
            GetRectTransforms();
        }

        private void GetRectTransforms()
        {
            _centerRect = centerImage.GetComponent<RectTransform>();
            _leftRect = leftImage.GetComponent<RectTransform>();
            _rightRect = rightImage.GetComponent<RectTransform>();
            _upRect = upImage.GetComponent<RectTransform>();
            _downRect = downImage.GetComponent<RectTransform>();
        }

        public void RefreshCursor()
        {
            var half = new Vector2(0.5f, 0.5f);

            _centerRect.anchorMin = half;
            _centerRect.anchorMax = half;
            _centerRect.anchoredPosition = Vector2.zero;
            _centerRect.sizeDelta = Vector2.one * centerSize;
            centerImage.color = centerColor;

            leftImage.enabled = showSides;
            rightImage.enabled = showSides;
            upImage.enabled = showSides;
            downImage.enabled = showSides;

            if (showSides == false) return;


            float posOffset = centerSize + sidesLength + sidesOffset;

            _leftRect.anchorMin = half;
            _leftRect.anchorMax = half;
            _leftRect.sizeDelta = new Vector2(sidesLength, sidesWidth);
            _leftRect.anchoredPosition = new Vector2(-posOffset, 0f);
            leftImage.color = sidesColor;

            _rightRect.anchorMin = half;
            _rightRect.anchorMax = half;
            _rightRect.sizeDelta = new Vector2(sidesLength, sidesWidth);
            _rightRect.anchoredPosition = new Vector2(posOffset, 0f);
            rightImage.color = sidesColor;
            
            _upRect.anchorMin = half;
            _upRect.anchorMax = half;
            _upRect.sizeDelta = new Vector2(sidesWidth, sidesLength);
            _upRect.anchoredPosition = new Vector2(0f, posOffset);
            upImage.color = sidesColor;
            
            _downRect.anchorMin = half;
            _downRect.anchorMax = half;
            _downRect.sizeDelta = new Vector2(sidesWidth, sidesLength);
            _downRect.anchoredPosition = new Vector2(0f, -posOffset);
            downImage.color = sidesColor;
        }
    }
}