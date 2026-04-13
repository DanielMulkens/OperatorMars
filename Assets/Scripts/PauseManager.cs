using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public InputActionAsset inputActions;
    public SimpleFPSController fpsController;
    public GameObject[] uiElementsToHide;

    private bool isPaused = false;
    private InputAction pauseAction;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        pauseAction = inputActions.FindActionMap("Player").FindAction("Pause");
        pauseAction.Enable();
        pauseAction.performed += OnPause;
    }

    void OnDestroy()
    {
        pauseAction.performed -= OnPause;
        pauseAction.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        fpsController.SetPaused(false);
        SetHiddenUI(true);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Quit");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        fpsController.SetPaused(true);
        SetHiddenUI(false);
    }

    void SetHiddenUI(bool visible)
    {
        foreach (GameObject ui in uiElementsToHide)
        {
            if (ui != null)
                ui.SetActive(visible);
        }
    }
}