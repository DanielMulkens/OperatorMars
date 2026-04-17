using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [Header("Level Setup")]
    public Transform levelParent;
    public GameObject[] levelPrefabs;
    public InteractableButton sequenceButton;
    public RobotController robotController;

    private GameObject currentLevelInstance;
    public LevelData CurrentLevelData { get; private set; }
    public int CurrentLevelIndex { get; private set; } = -1;

    public void LoadLevel(int index)
    {
        if (levelPrefabs == null || levelPrefabs.Length == 0)
        {
            Debug.LogError("No level prefabs assigned!");
            return;
        }

        if (index >= levelPrefabs.Length)
        {
            Debug.Log("All levels completed!");
            return;
        }

        // Destroy previous level fully
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        CurrentLevelIndex = index;

        // Instantiate new level
        currentLevelInstance = Instantiate(
            levelPrefabs[index],
            Vector3.zero,
            Quaternion.identity,
            levelParent
        );

        CurrentLevelData = currentLevelInstance.GetComponent<LevelData>();
        if (CurrentLevelData == null)
        {
            Debug.LogError("Level prefab missing LevelData!");
            return;
        }

        // Place robot at start
        if (robotController != null && CurrentLevelData.robotStart != null)
        {
            robotController.transform.position = CurrentLevelData.robotStart.position;
            robotController.transform.rotation = CurrentLevelData.robotStart.rotation;
        }

        // Assign LevelData to sequence button and reset sends
        if (sequenceButton != null)
        {
            sequenceButton.currentLevel = CurrentLevelData;
            sequenceButton.ResetSendCount();
        }
    }

    public void LoadNextLevel()
    {
        LoadLevel(CurrentLevelIndex + 1);
    }
}