using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RewardSubscriber : MonoBehaviour
{
    private TextMeshProUGUI _messageText;

    void Start()
    {
        _messageText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        InteractableProp.OnRewardGiven += HandleRewardGiven;
    }

    private void OnDisable()
    {
        InteractableProp.OnRewardGiven -= HandleRewardGiven;
    }

    private void HandleRewardGiven(string message)
    {
        StartCoroutine(ShowMessage(message));
    }
    
    private IEnumerator ShowMessage(string message)
    { 
        const float fadeInDuration = 0.0f; // Duration of the fade-in animation
        const float displayDuration = 2.0f; // Duration the message is displayed
        const float fadeOutDuration = 1.0f; // Duration of the fade-out animation
        
        float elapsedTime = 0;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            _messageText.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            yield return null;
        }

        _messageText.alpha = 1;

        _messageText.text = message;

        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            _messageText.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration);
            yield return null;
        }

        _messageText.alpha = 0;
        _messageText.text = "";
    }
}
