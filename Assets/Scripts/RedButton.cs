using UnityEngine;
using TMPro;
using System.Collections;

public class InteractableButton : MonoBehaviour
{
    private Animator animator;

    [Header("References")]
    public SequenceUI sequenceUI;          // Your sequence UI
    public RobotController robotController;
    public LevelData currentLevel;

    [Header("UI")]
    public TextMeshPro sequenceSendText;   // World-space TMP text

    private int sendCount = 0;
    private bool isSequenceRunning = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        UpdateSendText();
    }

    /// <summary>
    /// Called when player clicks the send button
    /// </summary>
    public void Interact()
    {
        if (isSequenceRunning)
            return;

        if (currentLevel != null && sendCount >= currentLevel.maxSequenceSends)
        {
            Debug.Log("No more sequence sends allowed!");
            return;
        }

        animator.SetTrigger("Press");
        StartCoroutine(RunSequence());

        sendCount++;
        UpdateSendText();
    }

    private IEnumerator RunSequence()
    {
        isSequenceRunning = true;

        // Lock sequence UI while running
        if (sequenceUI != null)
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

        // Clear the sequence line after execution
        if (sequenceUI != null)
            sequenceUI.ClearSequence();

        // Unlock sequence UI
        if (sequenceUI != null)
            sequenceUI.enabled = true;

        isSequenceRunning = false;
    }

    /// <summary>
    /// Resets the send count (called when a new level is loaded)
    /// </summary>
    public void ResetSendCount()
    {
        sendCount = 0;
        UpdateSendText();
    }

    private void UpdateSendText()
    {
        if (sequenceSendText != null && currentLevel != null)
        {
            sequenceSendText.text = $"Sends: {sendCount}/{currentLevel.maxSequenceSends}";
        }
    }
}