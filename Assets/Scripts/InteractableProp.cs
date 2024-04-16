using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

// Generalized Interactable Script to use with whatever
public class InteractableProp : MonoBehaviour
{
    // Leave this as empty if we want to delete the game object on interaction
    [SerializeField] private GameObject newStateGameObject;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private AudioClip newStateSound;
    [SerializeField] private AudioClip deniedStateSound;
    [SerializeField] private string onTriggerText;
    
    [SerializeField] private bool needsKey;
    [SerializeField] private bool hasReward;
    
    private Player _player;
    private AudioSource _soundSrc;
    public delegate void RewardGivenEvent(string message);

    public static event RewardGivenEvent OnRewardGiven;

    void Start()
    {
        messageText.text = onTriggerText;
        messageText.alpha = 0;
        GameObject playerObject = GameObject.FindWithTag("Player");
        _player = playerObject.GetComponent<Player>();
        _soundSrc = GameObject.FindWithTag("GameSoundSource").GetComponent<AudioSource>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.transform.CompareTag("Player"))
        {
            return;
        }
        
        messageText.alpha = 1;
        OnInteraction();
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
            messageText.alpha = 0;
    }

    private void OnInteraction()
    {
        if (!Input.GetKey(KeyCode.E))
        {
            return;
        }

        if (needsKey && _player.GetKeys() <= 0)
        {
            _soundSrc.clip = deniedStateSound;
            _soundSrc.Play();
            return;
        }

        if (needsKey)
        {
            _player.UseKeys(1);
        }

        if (newStateGameObject != null)
        {
            GameObject newObject = Instantiate(newStateGameObject, transform.position, transform.rotation);
    
            newObject.transform.SetParent(transform.parent);
        }

        _soundSrc.clip = newStateSound;
        _soundSrc.Play();

        if (hasReward)
        {
            int randomReward = Random.Range(0, 4); // 0 for health, 1 for cat souls, 2 for a key

            // Apply the chosen reward
            switch (randomReward)
            {
                case 0:
                    float health = Random.Range(10f, 100f);
                    _player.AddHealth((float) Math.Round(health));
                    OnRewardGiven?.Invoke("Received " + health + " hp from the chest");
                    break;
                case 1:
                    // Give more cat souls
                    int souls = Random.Range(5, 500);
                    ScoreManager.instance.AddPoint(souls);
                    OnRewardGiven?.Invoke("Received " + souls + " souls from the chest");
                    break;
                case 2:
                    int keys = Random.Range(1, 4);
                    _player.AddKeys(keys);
                    OnRewardGiven?.Invoke("Received " + keys + " keys from the chest");
                    break;
                case 3:
                    OnRewardGiven?.Invoke("Received nothing from the chest");
                    break;
            }
            
        }

        Destroy(gameObject);
    }
}
