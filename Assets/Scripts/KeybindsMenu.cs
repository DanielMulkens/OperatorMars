using UnityEngine;
using UnityEngine.SceneManagement;

public class KeybindingsMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string mainMenuScene = "StartScreen";

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}