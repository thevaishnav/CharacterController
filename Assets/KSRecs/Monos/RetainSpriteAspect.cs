using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]

    public class RetainSpriteAspect : UIBehaviour, ILayoutSelfController
    {
        public enum AspectMode
        {
            None,
            ConstantWidth,
            ConstantHeight,
            FitInParent,
            EnvelopeParent
        }

        [SerializeField] private AspectMode m_AspectMode = AspectMode.None;
        public AspectMode aspectMode
        {
            get { return m_AspectMode; }
            set
            {
                m_AspectMode = value;
                UpdateRect();
            }
        }
        private Image m_imgeComp;

        [System.NonSerialized]
        private RectTransform m_Rect;
        private bool m_DelayedSetDirty = false;
        private bool m_DoesParentExist = false;

        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }
        private Image image
        {
            get
            {
                if (m_imgeComp == null)
                    m_imgeComp = GetComponent<Image>();
                return m_imgeComp;
            }
        }
        private float AspectRatio
        {
            get
            {
                if (image.sprite == null) return 1f;
                return image.sprite.rect.width / image.sprite.rect.height;
            }
        }

#pragma warning disable 649
        private DrivenRectTransformTracker m_Tracker;
#pragma warning restore 649



        protected override void OnEnable()
        {
            base.OnEnable();
            m_DoesParentExist = rectTransform.parent ? true : false;
            SetDirty();
        }

        protected override void Start()
        {
            base.Start();
            //Disable the component if the aspect mode is not valid or the object state/setup is not supported with AspectRatio setup.
            if (!IsComponentValidOnObject() || !IsAspectModeValid())
                this.enabled = false;
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();

            m_DoesParentExist = rectTransform.parent ? true : false;
            SetDirty();
        }

        /// <summary>
        /// Update the rect based on the delayed dirty.
        /// Got around issue of calling onValidate from OnEnable function.
        /// </summary>
        protected virtual void Update()
        {
            if (m_DelayedSetDirty)
            {
                m_DelayedSetDirty = false;
                SetDirty();
            }
        }

        /// <summary>
        /// Function called when this RectTransform or parent RectTransform has changed dimensions.
        /// </summary>
        protected override void OnRectTransformDimensionsChange()
        {
            UpdateRect();
        }

        private void UpdateRect()
        {
            if (!IsActive() || !IsComponentValidOnObject())
                return;

            m_Tracker.Clear();
            switch (m_AspectMode)
            {
#if UNITY_EDITOR
                case AspectMode.None: break;
#endif
                case AspectMode.ConstantHeight:
                    {
                        m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
                        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * AspectRatio);
                        break;
                    }
                case AspectMode.ConstantWidth:
                    {
                        m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
                        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.width / AspectRatio);
                        break;
                    }
                case AspectMode.FitInParent:
                case AspectMode.EnvelopeParent:
                    {
                        if (!DoesParentExists())
                            break;

                        m_Tracker.Add(this, rectTransform,
                            DrivenTransformProperties.Anchors |
                            DrivenTransformProperties.AnchoredPosition |
                            DrivenTransformProperties.SizeDeltaX |
                            DrivenTransformProperties.SizeDeltaY);

                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.one;
                        rectTransform.anchoredPosition = Vector2.zero;

                        Vector2 sizeDelta = Vector2.zero;
                        Vector2 parentSize = GetParentSize();
                        if ((parentSize.y * AspectRatio < parentSize.x) ^ (m_AspectMode == AspectMode.FitInParent))
                        {
                            sizeDelta.y = GetSizeDeltaToProduceSize(parentSize.x / AspectRatio, 1);
                        }
                        else
                        {
                            sizeDelta.x = GetSizeDeltaToProduceSize(parentSize.y * AspectRatio, 0);
                        }
                        rectTransform.sizeDelta = sizeDelta;

                        break;
                    }
            }
        }

        private float GetSizeDeltaToProduceSize(float size, int axis)
        {
            return size - GetParentSize()[axis] * (rectTransform.anchorMax[axis] - rectTransform.anchorMin[axis]);
        }

        private Vector2 GetParentSize()
        {
            RectTransform parent = rectTransform.parent as RectTransform;
            return !parent ? Vector2.zero : parent.rect.size;
        }

        /// <summary>
        /// Method called by the layout system. Has no effect
        /// </summary>
        public virtual void SetLayoutHorizontal() { }

        /// <summary>
        /// Method called by the layout system. Has no effect
        /// </summary>
        public virtual void SetLayoutVertical() { }

        /// <summary>
        /// Mark the AspectRatioFitter as dirty.
        /// </summary>
        protected void SetDirty()
        {
            UpdateRect();
        }

        public bool IsComponentValidOnObject()
        {
            Canvas canvas = gameObject.GetComponent<Canvas>();
            if (canvas && canvas.isRootCanvas && canvas.renderMode != RenderMode.WorldSpace)
            {
                return false;
            }
            return true;
        }

        public bool IsAspectModeValid()
        {
            if (!DoesParentExists() && (aspectMode == AspectMode.EnvelopeParent || aspectMode == AspectMode.FitInParent))
                return false;

            return true;
        }

        private bool DoesParentExists()
        {
            return m_DoesParentExist;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            m_DelayedSetDirty = true;
        }

#endif
    }
}
