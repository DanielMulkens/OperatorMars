using UnityEngine;
using TMPro;
using System.Collections;

public class InteractableButton : MonoBehaviour
{
    private Animator animator;

    [Header("References")]
    public SequenceUI sequenceUI;
    public RobotController robotController;
    public SequenceExecutor sequenceExecutor;
    public LevelData currentLevel;

    [Header("UI")]
    public TextMeshPro sequenceSendText;

    private int sendCount = 0;
    private bool isSequenceRunning = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        UpdateSendText();
    }

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

        if (sequenceUI != null)
            sequenceUI.enabled = false;

        yield return sequenceExecutor.PlaySequenceCoroutine();

        if (sequenceUI != null)
            sequenceUI.ClearSequence();

        if (sequenceUI != null)
            sequenceUI.enabled = true;

        isSequenceRunning = false;
    }

    public void ResetSendCount()
    {
        sendCount = 0;
        UpdateSendText();
    }

    private void UpdateSendText()
    {
        if (sequenceSendText != null && currentLevel != null)
            sequenceSendText.text = $"Sends: {sendCount}/{currentLevel.maxSequenceSends}";
    }
}