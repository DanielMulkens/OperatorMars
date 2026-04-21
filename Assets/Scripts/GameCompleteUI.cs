using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteUI : MonoBehaviour
{
    [Header("Assign the INNER PANEL (GameCompletePanel)")]
    public GameObject panel;

    [Header("References")]
    public CanvasGroup levelCompleteUI;
    public SimpleFPSController fpsController;

    private void Awake()
    {
        if (panel == null)
        {
            Debug.LogError("GameCompleteUI: Panel NOT assigned in Inspector!");
            return;
        }

        panel.SetActive(false);
    }

    public void Show()
    {
        Debug.Log("GameCompleteUI.Show() called");

        if (panel == null)
        {
            Debug.LogError("Panel is NULL inside GameCompleteUI!");
            return;
        }

        // Ensure every parent in the hierarchy is active
        Transform t = panel.transform.parent;
        while (t != null)
        {
            t.gameObject.SetActive(true);
            t = t.parent;
        }

        // Hide level complete UI if assigned
        if (levelCompleteUI != null)
        {
            levelCompleteUI.alpha = 0f;
            levelCompleteUI.interactable = false;
            levelCompleteUI.blocksRaycasts = false;
        }

        // Disable FPS controller
        if (fpsController != null)
            fpsController.SetPaused(true);

        panel.SetActive(true);
        panel.transform.SetAsLastSibling();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void GoToMainMenu()
    {
        if (fpsController != null)
            fpsController.SetPaused(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void RestartGame()
    {
        if (fpsController != null)
            fpsController.SetPaused(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}