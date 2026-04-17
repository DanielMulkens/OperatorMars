using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceExecutor : MonoBehaviour
{
    public SequenceUI sequenceUI;
    public RobotController robotController;

    public void PlaySequence()
    {
        StartCoroutine(PlaySequenceCoroutine());
    }

    public IEnumerator PlaySequenceCoroutine()
    {
        robotController.sequenceRunning = true;

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

        robotController.sequenceRunning = false;

        Debug.Log("Sequence finished. Checking caves...");

        CheckCavesAfterSequence();
    }

    private void CheckCavesAfterSequence()
    {
        List<SimpleCavePortal> caves = CaveManager.GetCaves();

        if (caves == null || caves.Count == 0)
        {
            Debug.Log("No caves registered");
            return;
        }

        foreach (SimpleCavePortal cave in caves)
        {
            if (cave != null && cave.robotInside)
            {
                Debug.Log($"{cave.gameObject.name}: Teleporting after sequence");
                cave.Teleport(robotController);
                return;
            }
        }

        Debug.Log("Robot not inside any cave");
    }
}