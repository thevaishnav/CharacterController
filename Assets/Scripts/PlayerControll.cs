using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public CharacterController characterController;
    private float camRotation = 0;
    public GameObject camera;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float speed = 15f;
    [SerializeField] bool isGrounded;
    [SerializeField] private Vector3 velocity;
    [SerializeField] float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundmask;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void HandleRotation()
    {
        float mousex = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mousey = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mousex);
        float camrotafterframe = camRotation - mousey;
        camRotation = camrotafterframe;
        camRotation = Mathf.Clamp(camRotation, -90, 90);
        camera.transform.localRotation = Quaternion.Euler(camRotation, 0, 0);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        characterController.Move(horizontal * transform.right);
        characterController.Move(vertical * transform.forward);
    }

    void HandleGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);
        if (isGrounded)
        {
            if(velocity.y < 0)
            {
                velocity.y = -2;
            }
        }
        velocity.y += gravity * Time.deltaTime  ;
        characterController.Move(velocity * Time.deltaTime);
       }    

    void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleGravity();
    }
}
