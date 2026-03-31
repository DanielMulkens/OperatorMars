using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceExecutor : MonoBehaviour
{
    public SequenceUI sequenceUI;
    public RobotController robotController;

    public void PlaySequence()
    {
        List<CommandType> commands = sequenceUI.GetSequence();
        StartCoroutine(ExecuteSequence(commands));
    }

    private IEnumerator ExecuteSequence(List<CommandType> commands)
    {
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
    }
}