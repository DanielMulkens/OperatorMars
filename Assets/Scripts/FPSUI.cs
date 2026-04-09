using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FPSUIInteraction : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputActions;   // Drag your PlayerInput.inputactions here
    public string actionMapName = "Player"; // Name of the Action Map
    public string actionName = "InteractMain";  // Name of your action

    [Header("Camera & UI")]
    public Camera playerCamera;
    public float interactDistance = 5f;

    private InputAction interactAction;
    private EventSystem eventSystem;

    void Awake()
    {
        if (inputActions == null)
        {
            Debug.LogError("InputActions asset not assigned!");
            return;
        }

        interactAction = inputActions.FindAction($"{actionMapName}/{actionName}");
        if (interactAction == null)
        {
            Debug.LogError($"Action '{actionMapName}/{actionName}' not found in InputActions asset!");
        }

        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("No EventSystem found in the scene!");
        }
    }

    void OnEnable()
    {
        if (interactAction != null)
            interactAction.performed += OnInteract;
    }

    void OnDisable()
    {
        if (interactAction != null)
            interactAction.performed -= OnInteract;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        TryUIInteract();
    }

    /// <summary>
    /// Tries to interact with a UI button at the center of the screen
    /// </summary>
    void TryUIInteract()
    {
        if (eventSystem == null || playerCamera == null) return;

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = new Vector2(Screen.width / 2f, Screen.height / 2f)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster[] raycasters = FindObjectsOfType<GraphicRaycaster>();

        foreach (var gr in raycasters)
        {
            gr.Raycast(pointerData, results);
            foreach (var r in results)
            {
                Button btn = r.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.Invoke();
                    Debug.Log($"Button clicked: {btn.name}");
                    return; // Only trigger the first detected button
                }
            }
        }

        Debug.Log("No button detected under center of screen.");
    }
}