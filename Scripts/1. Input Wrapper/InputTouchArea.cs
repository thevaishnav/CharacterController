using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omnix.CCN.InputSystemWrapper
{
    [RequireComponent(typeof(Image))]
    public class InputTouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField, Tooltip("Game object to activate when user presses the button")] private RectTransform root;
        [SerializeField] private UiInputBase target;

        private void Start()
        {
            root.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            root.gameObject.SetActive(true);
            root.position = eventData.position;
            target.OnPointerDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            root.gameObject.SetActive(false);
            target.OnPointerUp(eventData);
        }
    }
}