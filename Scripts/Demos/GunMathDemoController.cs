using Omnix.CCN.Health;
using Omnix.CCN.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omnix.CCN.Demos
{
    public class GunMathDemoController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _ipMagSize;
        [SerializeField] private TMP_InputField _ipCurrentAmmo;
        [SerializeField] private TMP_InputField _ipCurrentMag;
        [SerializeField] private Button _bnSemiReload;
        [SerializeField] private Button _bnFullReload;
        [SerializeField] private TextMeshProUGUI _txTotalAmmo;
        [SerializeField] private TextMeshProUGUI _txStatus;
        
        [Header("Shoot")]
        [SerializeField] private Button _bnShoot;
        [SerializeField] private Toggle _tgAllowReloadBeforeShoot;
        [SerializeField] private Toggle _tgAllowReloadAfterShoot;
        

        private void Start()
        {
            _ipMagSize.onValueChanged.AddListener(UpdateTotalAmmo);
            _ipCurrentAmmo.onValueChanged.AddListener(UpdateTotalAmmo);
            _ipCurrentMag.onValueChanged.AddListener(UpdateTotalAmmo);

            _bnShoot.onClick.AddListener(OnShoot);
            _bnSemiReload.onClick.AddListener(OnSemiReload);
            _bnFullReload.onClick.AddListener(OnFullReload);
            
            UpdateTotalAmmo(null);
        }

        private bool GetValues(out int magSize, out int currentAmmo, out float currentMag)
        {
            bool b1 = int.TryParse(_ipMagSize.text, out magSize);
            bool b2 = int.TryParse(_ipCurrentAmmo.text, out currentAmmo);
            bool b3 = float.TryParse(_ipCurrentMag.text, out currentMag);
            if (b1 == false) Debug.LogError($"Unable to parse MagSize: ({_ipMagSize.text})");
            if (b2 == false) Debug.LogError($"Unable to parse CurrentAmmo: ({_ipCurrentAmmo.text})");
            if (b3 == false) Debug.LogError($"Unable to parse CurrentAmmo: ({_ipCurrentMag.text})");
            return b1 && b2 && b3;
        }

        public void UpdateTotalAmmo()
        {
            if (GetValues(out int magSize, out int currentAmmo, out float currentMag))
                _txTotalAmmo.text = GunMath.TotalAvailableAmmo(currentAmmo, currentMag, magSize).ToString();
        }
        
        private void UpdateTotalAmmo(string _)
        {
            if (GetValues(out int magSize, out int currentAmmo, out float currentMag))
                _txTotalAmmo.text = GunMath.TotalAvailableAmmo(currentAmmo, currentMag, magSize).ToString();
        }

        private void OnShoot()
        {
            if (GetValues(out int magSize, out int currentAmmo, out float currentMag) == false) return;

            ShootStatus status = GunMath.Shoot(ref currentAmmo, ref currentMag, magSize, _tgAllowReloadBeforeShoot.isOn, _tgAllowReloadAfterShoot.isOn);
            _ipCurrentAmmo.text = currentAmmo.ToString();
            _ipCurrentMag.text = currentMag.ToString();
            _txStatus.text = status.ToString();
            _txTotalAmmo.text = GunMath.TotalAvailableAmmo(currentAmmo, currentMag, magSize).ToString();
        }

        private void OnSemiReload()
        {
            if (GetValues(out int magSize, out int currentAmmo, out float currentMag) == false) return;

            bool status = GunMath.Reload(ref currentAmmo, ref currentMag, magSize, GunReloadType.SemiReload, false);
            _ipCurrentAmmo.text = currentAmmo.ToString();
            _ipCurrentMag.text = currentMag.ToString();
            _txStatus.text = status ? "Reload Success" : "Reload Failed";
            _txTotalAmmo.text = GunMath.TotalAvailableAmmo(currentAmmo, currentMag, magSize).ToString();
        }

        private void OnFullReload()
        {
            if (GetValues(out int magSize, out int currentAmmo, out float currentMag) == false) return;

            bool status = GunMath.Reload(ref currentAmmo, ref currentMag, magSize, GunReloadType.FullReload, false);
            _ipCurrentAmmo.text = currentAmmo.ToString();
            _ipCurrentMag.text = currentMag.ToString();
            _txStatus.text = status ? "Reload Success" : "Reload Failed";
            _txTotalAmmo.text = GunMath.TotalAvailableAmmo(currentAmmo, currentMag, magSize).ToString();
        }
    }
}