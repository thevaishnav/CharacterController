using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsGrounded {get; private set;}
    public bool IsMoving {get; private set;}
    public float Speed {get; private set;}
    public Vector3 Velocity {get; private set;}
    
    [Header("References")]
    public Camera firstPersonCam;
    public Camera scopedInCam;
    public Camera thirdPersonCam;
    public CharacterController characterController;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    [SerializeField] float groundDistance = 0.4f;
    
    [Header("Common Tunables")]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float jumpHight = 2f;
    [SerializeField] int maxJumpsInAir = 1;
    [SerializeField] float gravityModifier = 2f;
    [SerializeField] int perspective = 3;

    [Header("First Person Tunables")]
    [SerializeField] float maxVerticalAngle = 80f;

    [Header("Third Person Tunables")]
    [SerializeField] float turnSmoothTime = 0.1f;
    
    [HideInInspector] public int FirstPersonPerspective = 1;
    [HideInInspector] public int ScopedInPerspective = 2;
    [HideInInspector] public int ThirdPersonPerspective = 3;

    float xRotation = 0f;
    Vector3 velocity;
    bool isGrounded = false;
    int jumpedInAir = 0;
    float turnSmoothVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gravityModifier *= Physics.gravity.y;
        changePerspecive(perspective);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        { 
            if (perspective == FirstPersonPerspective) { changePerspecive(ThirdPersonPerspective); }
            else if (perspective == ThirdPersonPerspective) { changePerspecive(FirstPersonPerspective); }
        }
        if (perspective == FirstPersonPerspective || perspective == ScopedInPerspective) { firstPersonMovement(); }
        else if (perspective == ThirdPersonPerspective) { thirdPersonMovement(); }
        handleJump();
    }

    private void firstPersonMovement()
    {
        // movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            characterController.Move(move * runSpeed * Time.deltaTime);
        } 
        else
        {
            characterController.Move(move * walkSpeed * Time.deltaTime);
        }

        // rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        firstPersonCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        scopedInCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void thirdPersonMovement()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horiz, 0, vert).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + thirdPersonCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            float moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { moveSpeed = runSpeed; }
            else { moveSpeed = walkSpeed; }
            
            direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(direction.normalized * moveSpeed * Time.deltaTime);
        }
    }

    private void handleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        if (isGrounded)
        {
            if (velocity.y < 0) { velocity.y = -2f; }
            jumpedInAir = 0;
            
            if (maxJumpsInAir == 0 && Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHight * -2 * gravityModifier);
                jumpedInAir++;
            }
        }
        
        if (maxJumpsInAir > 0 && Input.GetButtonDown("Jump") && jumpedInAir < maxJumpsInAir)
        {
            velocity.y = Mathf.Sqrt(jumpHight * -2 * gravityModifier);
            jumpedInAir++;
        }

        velocity.y += gravityModifier * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void changePerspecive(int newPerspective)
    {
        perspective = newPerspective;
        if (newPerspective == FirstPersonPerspective)
        {
            firstPersonCam.gameObject.SetActive(true);
            scopedInCam.gameObject.SetActive(false);
            thirdPersonCam.gameObject.SetActive(false);
        } else if (newPerspective == ScopedInPerspective)
        {
            firstPersonCam.gameObject.SetActive(false);
            scopedInCam.gameObject.SetActive(true);
            thirdPersonCam.gameObject.SetActive(false);
        } else
        {
            firstPersonCam.gameObject.SetActive(false);
            scopedInCam.gameObject.SetActive(false);
            thirdPersonCam.gameObject.SetActive(true);
        }
    }

    public int GetPerspective() { return perspective; }

}
