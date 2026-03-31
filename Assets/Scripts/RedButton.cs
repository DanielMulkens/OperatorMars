using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    private Animator animator;

    [Header("References")]
    public SequenceUI sequenceUI;           // Reference to your sequence UI
    public RobotController robotController; // Reference to your robot

    private bool isSequenceRunning = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Call this either via UI OnClick() or from PlayerMovement keybind
    public void Interact()
    {
        if (isSequenceRunning)
            return; // prevent double execution

        animator.SetTrigger("Press");
        StartCoroutine(RunSequence());
    }

    private System.Collections.IEnumerator RunSequence()
    {
        isSequenceRunning = true;

        // Lock the sequence so player can't move blocks while executing
        sequenceUI.enabled = false;

        var commands = sequenceUI.GetSequence();

        foreach (var command in commands)
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

        // Unlock sequence after execution
        sequenceUI.enabled = true;
        isSequenceRunning = false;
    }
}