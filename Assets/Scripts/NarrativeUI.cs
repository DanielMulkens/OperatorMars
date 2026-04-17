using UnityEngine;
using System.Collections;

public class NarrativeUI : MonoBehaviour
{
    public TypewriterEffectTMP typewriter;
    public float displayDuration = 3f;

    void OnEnable()
    {
        typewriter.OnTypingComplete += OnTypingComplete;
    }

    void OnDisable()
    {
        typewriter.OnTypingComplete -= OnTypingComplete;
    }

    public void ShowNarrative(LevelData level)
    {
        gameObject.SetActive(true);
        typewriter.ShowMessage(level.narrativeText);
    }

    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void OnTypingComplete()
    {
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        Hide();
    }
}