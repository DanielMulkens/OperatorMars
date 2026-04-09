using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffectTMP : MonoBehaviour
{
    public TMP_Text uiText;               // Assign your TMP Text component
    public float characterDelay = 0.05f;  // Time between each character

    /// <summary>
    /// Starts typing the message
    /// </summary>
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
            yield return new WaitForSeconds(characterDelay);
        }
    }
}