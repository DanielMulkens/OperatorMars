using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Scene names (make sure they match EXACTLY in Build Settings)
    [Header("Scene Names")]
    public string mainGameScene = "OperatorMars";
    public string levelSelectorScene = "LevelSelector";
    public string creditsScene = "Credits";

    // Start Game button
    public void StartGame()
    {
        SceneManager.LoadScene(mainGameScene);
    }

    // Level Selector button
    public void OpenLevelSelector()
    {
        SceneManager.LoadScene(levelSelectorScene);
    }

    // Credits button
    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }

    // Quit button
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}