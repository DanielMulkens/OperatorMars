using UnityEngine;
using System.Collections;

public class NarrativeUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public TypewriterEffectTMP typewriter;

    [Header("Timing")]
    public float hideDelayAfterTyping = 3f;

    private Coroutine hideRoutine;

    void Awake()
    {
        // Ensure clean start state
        if (panel != null)
            panel.SetActive(false);

        // Always register event ONCE
        if (typewriter != null)
            typewriter.OnTypingComplete += OnTypingComplete;
        else
            Debug.LogError("Typewriter reference missing in NarrativeUI!");
    }

    public void ShowNarrative(LevelData level)
    {
        if (panel == null || typewriter == null)
        {
            Debug.LogError("NarrativeUI missing references!");
            return;
        }

        // Stop any previous hide coroutine
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        panel.SetActive(true);
        typewriter.ShowMessage(level.narrativeText);
    }

    private void OnTypingComplete()
    {
        Debug.Log("Typing complete received");

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelayAfterTyping);
        panel.SetActive(false);
    }

    public void Hide()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        panel.SetActive(false);
    }
}