using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayVictory : MonoBehaviour
{
    private Boss BossSpawned;
    public TextMeshProUGUI messageText;
    public AudioSource victSrc;
    public AudioClip vicClip; 
    private void OnEnable()
    {
        EnemySpawner.OnVictory += HandleVictory;
    }

    private void OnDisable()
    {
        EnemySpawner.OnVictory -= HandleVictory;
    }

    private void HandleVictory()
    {
        StartCoroutine(DisplayVictoryText());
    }


    public IEnumerator DisplayVictoryText()
    {
        const float fadeInDuration = 1.0f; // Duration of the fade-in animation
        const float displayDuration = 3.0f; // Duration the message is displayed
        const float fadeOutDuration = 2.0f; // Duration of the fade-out animation

        float elapsedTime = 0;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            messageText.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            yield return null;
        }
        messageText.alpha = 1;
        
        victSrc.clip = vicClip;
        victSrc.Play();
        messageText.text = "VICTORY! ALL BOSSES DEFEATED!";
        
        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        messageText.alpha = 0;
        messageText.text = "";
        
    }
}
