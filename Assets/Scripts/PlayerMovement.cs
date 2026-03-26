using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    public InputActionAsset controls;
    public Transform cameraTransform;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Look")]
    [Tooltip("Scale this to adjust mouse speed")]
    public float mouseSensitivity = 1f;

    [Header("Gravity")]
    public float gravity = -9.81f;

    [Header("Interaction")]
    public float interactDistance = 5f;

    CharacterController controller;

    InputAction moveAction;
    InputAction lookAction;
    InputAction interactSecondAction; // E key

    Vector3 velocity;
    float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        var map = controls.FindActionMap("Player");

        moveAction = map.FindAction("Move");
        lookAction = map.FindAction("Look");
        interactSecondAction = map.FindAction("InteractSecond"); // E key for world buttons
    }

    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        interactSecondAction.Enable();

        interactSecondAction.performed += OnInteractSecond;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        interactSecondAction.Disable();

        interactSecondAction.performed -= OnInteractSecond;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void OnInteractSecond(InputAction.CallbackContext context)
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            InteractableButton button = hit.collider.GetComponent<InteractableButton>();
            if (button != null)
            {
                button.Interact();
            }
        }
    }
}