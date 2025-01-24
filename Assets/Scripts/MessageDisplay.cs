using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageDisplay : MonoBehaviour
{
    public TMP_Text messageToDisplay;
    public float displayDuration = 2f;

    public void ShowMessage(string message)
    {
        messageToDisplay.text = message;
        messageToDisplay.gameObject.SetActive(true);
        StartCoroutine(HideTextAfterDelay());
    }

    /// <summary>
    /// This procedure is used by a Coroutine which hides the message after a short delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        messageToDisplay.gameObject.SetActive(false);
    }
}
