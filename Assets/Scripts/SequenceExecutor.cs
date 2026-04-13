using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceExecutor : MonoBehaviour
{
    public SequenceUI sequenceUI;
    public RobotController robotController;
    public float caveTeleportThreshold = 0.75f;

    private CaveEntrance[] caveEntrances;
    private CaveEntrance lastUsedEntrance;

    void Start()
    {
        RefreshCaveEntrances();
    }

    public void RefreshCaveEntrances()
    {
        caveEntrances = FindObjectsByType<CaveEntrance>(FindObjectsSortMode.None);
        Debug.Log($"Found {caveEntrances.Length} cave entrances");
    }

    public void PlaySequence()
    {
        StartCoroutine(PlaySequenceCoroutine());
    }

    public IEnumerator PlaySequenceCoroutine()
    {
        List<CommandType> commands = sequenceUI.GetSequence();

        foreach (CommandType command in commands)
        {
            switch (command)
            {
                case CommandType.MoveForward:
                    yield return robotController.MoveForward();
                    break;
                case CommandType.TurnLeft:
                    yield return robotController.TurnLeft();
                    break;
                case CommandType.TurnRight:
                    yield return robotController.TurnRight();
                    break;
                case CommandType.UseDrill:
                    yield return robotController.UseDrill();
                    break;
            }
        }

        Debug.Log($"Sequence done. Robot position: {robotController.transform.position}");
        CheckCaveEntrance();
    }

    private void CheckCaveEntrance()
    {
        if (caveEntrances == null || caveEntrances.Length == 0)
        {
            Debug.Log("No cave entrances found");
            return;
        }

        foreach (CaveEntrance cave in caveEntrances)
        {
            if (cave == null) continue;

            // Skip the exit point the robot just arrived at
            if (cave == lastUsedEntrance?.linkedEntrance) continue;

            float distance = Vector3.Distance(
                new Vector3(robotController.transform.position.x, 0, robotController.transform.position.z),
                new Vector3(cave.transform.position.x, 0, cave.transform.position.z)
            );

            Debug.Log($"Distance to {cave.gameObject.name}: {distance}");

            if (distance <= caveTeleportThreshold)
            {
                lastUsedEntrance = cave;
                cave.Teleport(robotController);
                return;
            }
        }

        // Reset last used entrance if robot moved away
        lastUsedEntrance = null;
    }
}