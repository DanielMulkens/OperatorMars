using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsGrab : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputActions;
    private InputAction interactAction;
    private InputAction throwAction;

    [Header("Grab Settings")]
    public Transform holdPoint;
    public float grabDistance = 3f;
    public float moveSpeed = 15f;

    [Header("Throw Settings")]
    public float throwForce = 15f;
    public float throwUpwardBoost = 0.2f;

    [Header("Layers")]
    public LayerMask ignoreLayers;

    private Rigidbody heldRb;
    private GameObject heldObject;

    void Awake()
    {
        interactAction = inputActions.FindAction("InteractMain");
        throwAction = inputActions.FindAction("Throw");
    }

    void OnEnable()
    {
        interactAction.Enable();
        throwAction.Enable();

        interactAction.performed += OnInteract;
        throwAction.performed += OnThrow;
    }

    void OnDisable()
    {
        interactAction.performed -= OnInteract;
        throwAction.performed -= OnThrow;

        interactAction.Disable();
        throwAction.Disable();
    }

    void Update()
    {
        if (heldObject != null)
        {
            MoveObject();
        }
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (heldObject == null)
            TryGrab();
        else
            Release();
    }

    void OnThrow(InputAction.CallbackContext context)
    {
        if (heldObject != null)
        {
            ThrowObject();
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        int layerMask = ~ignoreLayers;

        if (Physics.Raycast(ray, out hit, grabDistance, layerMask))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                heldObject = hit.collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();

                if (heldRb != null)
                {
                    heldRb.useGravity = false;
                    heldRb.linearDamping = 10f;
                    heldRb.angularDamping = 10f;
                }
            }
        }
    }

    void MoveObject()
    {
        Vector3 targetPosition = holdPoint.position;
        Vector3 direction = targetPosition - heldObject.transform.position;

        heldRb.linearVelocity = direction * moveSpeed;
    }

    void ThrowObject()
    {
        heldRb.useGravity = true;
        heldRb.linearDamping = 0;
        heldRb.angularDamping = 0;

        // Reset velocity before throwing
        heldRb.linearVelocity = Vector3.zero;

        // Natural arc throw direction
        Vector3 throwDirection = Camera.main.transform.forward + Vector3.up * throwUpwardBoost;

        heldRb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);

        heldObject = null;
        heldRb = null;
    }

    void Release()
    {
        heldRb.useGravity = true;
        heldRb.linearDamping = 0;
        heldRb.angularDamping = 0;

        heldObject = null;
        heldRb = null;
    }
}