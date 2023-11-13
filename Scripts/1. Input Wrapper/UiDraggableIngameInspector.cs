using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omnix.CCN.InputSystemWrapper
{
    public class UiDraggableIngameInspector : MonoBehaviour
    {
        [SerializeField] private UiInputBase target;
        [SerializeField] private TMP_InputField xSensitivityInput;
        [SerializeField] private TMP_InputField ySensitivityInput;
        [SerializeField] private TMP_InputField pullForceInput;
        [SerializeField] private TMP_Dropdown overflowInput;
        [SerializeField] private Button saveButton;

        private void OnEnable()
        {
            xSensitivityInput.text = target.sensitivityMultiplier.x.ToString(CultureInfo.InvariantCulture);
            ySensitivityInput.text = target.sensitivityMultiplier.y.ToString(CultureInfo.InvariantCulture);
            pullForceInput.text = target.pullForce.ToString(CultureInfo.InvariantCulture);
            overflowInput.value = (int)target.Overflow;
            saveButton.onClick.AddListener(Save);
        }

        private void OnDisable()
        {
            saveButton.onClick.RemoveListener(Save);
        }

        private void Save()
        {
            float xSense, ySense, pull;
            string xSenseText = xSensitivityInput.text;
            if (!float.TryParse(xSenseText, out xSense)) xSense = target.sensitivityMultiplier.x;
            if (!float.TryParse(ySensitivityInput.text, out ySense)) ySense = target.sensitivityMultiplier.y;

            target.sensitivityMultiplier = new Vector2(xSense, ySense);

            if (float.TryParse(pullForceInput.text, out pull)) target.pullForce = pull;

            int overflow = overflowInput.value;
            if (0 <= overflow && overflow < 2) target.Overflow = (UiInputBase.OverflowBehaviour)overflow;
        }
    }
}