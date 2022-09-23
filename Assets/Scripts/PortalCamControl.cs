using UnityEngine;

public class PortalCamControl : MonoBehaviour
{
    public Vector3 floorOffset;
    public Transform playerCam;


    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = floorOffset + playerCam.position;
        transform.rotation = playerCam.rotation;
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
}
