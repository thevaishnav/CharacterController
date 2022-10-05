using UnityEngine;
using UnityEngine.UI;

namespace KSRecs.Callbacks
{
    [RequireComponent(typeof(Dropdown))]
    public abstract class DropdownCallback : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            Dropdown toggle = GetComponent<Dropdown>();
            if (toggle)
                toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnDisable()
        {
            Dropdown toggle = GetComponent<Dropdown>();
            if (toggle)
                toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        protected abstract void OnValueChanged(int newValue);
    }
}