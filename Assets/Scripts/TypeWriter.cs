using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class TypewriterEffectTMP : MonoBehaviour
{
    public TMP_Text uiText;
    public float characterDelay = 0.05f;
    public float punctuationPause = 1f;

    public event Action OnTypingComplete;

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        uiText.text = "";
        foreach (char c in message)
        {
            uiText.text += c;

            if (c == '.' || c == '!' || c == '?')
                yield return new WaitForSeconds(punctuationPause);
            else
                yield return new WaitForSeconds(characterDelay);
        }

        OnTypingComplete?.Invoke();
    }
}