using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelLoader levelLoader;
    public CanvasGroup levelCompleteUI;
    public TypewriterEffectTMP typewriterTMP;

    [Header("UI")]
    public TextMeshPro levelIndicatorText;

    [Header("Settings")]
    public float levelCompleteDelay = 2f; // Delay before next level

    private bool levelCompleted = false;

    void Start()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.alpha = 0f;
            levelCompleteUI.interactable = false;
            levelCompleteUI.blocksRaycasts = false;
        }

        LoadLevel(0); // Start first level
    }

    void Update()
    {
        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        if (levelCompleted || levelLoader == null || levelLoader.CurrentLevelData == null)
            return;

        var robot = levelLoader.robotController;
        var target = levelLoader.CurrentLevelData.targetArea;

        if (robot != null && target != null && target.bounds.Contains(robot.transform.position))
        {
            LevelCompleted();
        }
    }

    private void LevelCompleted()
    {
        levelCompleted = true;
        Debug.Log("Level Completed!");

        if (levelCompleteUI != null)
        {
            levelCompleteUI.alpha = 1f;
            levelCompleteUI.interactable = true;
            levelCompleteUI.blocksRaycasts = true;
        }

        if (typewriterTMP != null)
        {
            typewriterTMP.ShowMessage($"LEVEL {levelLoader.CurrentLevelIndex + 1} COMPLETED");
        }

        // Load next level after delay
        Invoke(nameof(LoadNextLevel), levelCompleteDelay);
    }

    public void LoadLevel(int index)
    {
        levelCompleted = false;

        if (levelCompleteUI != null)
        {
            levelCompleteUI.alpha = 0f;
            levelCompleteUI.interactable = false;
            levelCompleteUI.blocksRaycasts = false;
        }

        levelLoader.LoadLevel(index);
        UpdateLevelIndicator();
    }

    private void LoadNextLevel()
    {
        int nextIndex = levelLoader.CurrentLevelIndex + 1;
        if (nextIndex < levelLoader.levelPrefabs.Length)
        {
            LoadLevel(nextIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    private void UpdateLevelIndicator()
    {
        if (levelIndicatorText != null && levelLoader.CurrentLevelData != null)
        {
            levelIndicatorText.text = $"Level {levelLoader.CurrentLevelIndex + 1}: {levelLoader.CurrentLevelData.levelName}";
        }
    }
}