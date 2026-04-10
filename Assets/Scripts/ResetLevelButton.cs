using UnityEngine;

public class ResetLevelButton : MonoBehaviour
{
    [Header("References")]
    public LevelLoader levelLoader;
    public SequenceUI sequenceUI;

    public void ResetLevel()
    {
        if (levelLoader == null)
        {
            Debug.LogError("LevelLoader not assigned!");
            return;
        }

        // Reload current level
        levelLoader.LoadLevel(levelLoader.CurrentLevelIndex);

        // Clear sequence UI (extra safety)
        if (sequenceUI != null)
        {
            sequenceUI.ClearSequence();
        }

        Debug.Log("Level Reset!");
    }
}