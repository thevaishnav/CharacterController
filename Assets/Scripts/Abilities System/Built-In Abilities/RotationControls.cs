using UnityEngine;


public class RotationControls : Ability
{
    [SerializeField] private float sensitivity = 2.5f;
    
    void Update()
    {
        if (Controller.IsMoving)
        {
            float horz = Input.GetAxis("Mouse X");
            if (horz != 0.1f)
            {
                Controller.InstantRotateByY(-1f * horz * sensitivity);
            }   
        }
    }
}
