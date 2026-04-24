using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public InputActionAsset inputActions;
    public SimpleFPSController fpsController;
    public GameObject[] uiElementsToHide;

    [Header("Scene Names")]
    public string mainMenuScene = "StartScreen";

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
        Debug.Log("RESUME called");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        fpsController.SetPaused(false);
        SetHiddenUI(true);
    }

    public void GoToMainMenu()
    {
        Debug.Log("GO TO MAIN MENU called");
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Quit()
    {
        Debug.Log("QUIT called");
        Time.timeScale = 1f;
        Application.Quit();
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