using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [Header("Level Setup")]
    public Transform levelParent;
    public GameObject[] levelPrefabs;
    public InteractableButton sequenceButton;
    public RobotController robotController;

    [Header("UI Reference (drag GameCompleteUI ROOT here)")]
    public GameCompleteUI gameCompleteUI;

    private GameObject currentLevelInstance;

    public LevelData CurrentLevelData { get; private set; }
    public int CurrentLevelIndex { get; private set; } = -1;

    private void Start()
    {
        if (gameCompleteUI == null)
        {
            Debug.LogError("GameCompleteUI not assigned in LevelLoader!");
        }

        LoadLevel(0);
    }

    public void LoadLevel(int index)
    {
        if (levelPrefabs == null || levelPrefabs.Length == 0)
        {
            Debug.LogError("No level prefabs assigned!");
            return;
        }

        // =========================
        // GAME COMPLETE
        // =========================
        if (index >= levelPrefabs.Length)
        {
            Debug.Log("ALL LEVELS COMPLETED — showing Game Complete UI");

            if (gameCompleteUI == null)
            {
                Debug.LogError("GameCompleteUI reference missing in LevelLoader!");
                return;
            }

            // Destroy last level before showing UI
            if (currentLevelInstance != null)
            {
                Destroy(currentLevelInstance);
                currentLevelInstance = null;
            }

            gameCompleteUI.Show();
            return;
        }

        // Destroy old level
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        CurrentLevelIndex = index;

        // Spawn level
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

        // Move robot
        if (robotController != null && CurrentLevelData.robotStart != null)
        {
            robotController.transform.position = CurrentLevelData.robotStart.position;
            robotController.transform.rotation = CurrentLevelData.robotStart.rotation;
        }

        // Setup button
        if (sequenceButton != null)
        {
            sequenceButton.currentLevel = CurrentLevelData;
            sequenceButton.ResetSendCount();
        }

        Time.timeScale = 1f;
    }

    public void LoadNextLevel()
    {
        LoadLevel(CurrentLevelIndex + 1);
    }
}