using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsGrab : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputActions;
    private InputAction interactAction;

    [Header("Grab Settings")]
    public Transform holdPoint;
    public float grabDistance = 3f;
    public float moveSpeed = 15f;

    [Header("Layers")]
    public LayerMask ignoreLayers;

    private Rigidbody heldRb;
    private GameObject heldObject;

    void Awake()
    {
        interactAction = inputActions.FindAction("InteractMain");
    }

    void OnEnable()
    {
        interactAction.Enable();
        interactAction.performed += OnInteract;
    }

    void OnDisable()
    {
        interactAction.performed -= OnInteract;
        interactAction.Disable();
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
                    heldRb.linearDamping = 10;
                    heldRb.angularDamping = 10;
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

    void Release()
    {
        heldRb.useGravity = true;
        heldRb.linearDamping = 0;
        heldRb.angularDamping = 0;

        heldObject = null;
        heldRb = null;
    }
}