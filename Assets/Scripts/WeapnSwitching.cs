using UnityEngine;

public class WeapnSwitching : MonoBehaviour
{
    public Transform firstPersonCam;
    public PlayerMovement pc;
    [SerializeField] private int currentWeapon = 0;
    private Vector3 angleOffset;

    void Start()
    {
        selectWeapon();
        angleOffset = transform.rotation.eulerAngles - firstPersonCam.rotation.eulerAngles;
    }

    void Update()
    {
        if (pc.GetPerspective() == pc.ScopedInPerspective) { return; }
        
        float mouseInput = Input.GetAxis("Mouse ScrollWheel");
        int previousSelectedWeapon = currentWeapon;
        if (mouseInput > 0f) 
        {
            if (currentWeapon >= transform.childCount - 1) { currentWeapon = 0;} 
            else {currentWeapon++;}
        }
        else if (mouseInput < 0f) 
        {
            if (currentWeapon <= 0) { currentWeapon = transform.childCount - 1; }
            else { currentWeapon--;}
        }
        checkKeyInput();
        if (currentWeapon != previousSelectedWeapon) { selectWeapon(); }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(firstPersonCam.rotation.eulerAngles + angleOffset);
    }

    void checkKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentWeapon = 0; }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) { currentWeapon = 1; }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) { currentWeapon = 2; }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4) { currentWeapon = 3; }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5) { currentWeapon = 4; }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 6) { currentWeapon = 5; }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && transform.childCount >= 7) { currentWeapon = 6; }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && transform.childCount >= 8) { currentWeapon = 7; }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && transform.childCount >= 9) { currentWeapon = 8; }
    }

    void selectWeapon()
    {
        int ind = 0;
        foreach (Transform weapon in transform)
        {
            if (ind == currentWeapon) { weapon.gameObject.SetActive(true); }
            else { weapon.gameObject.SetActive(false); }
            ind++;
        }
    }
}
