using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public float magnetSpeed = 5f; // Speed at which the health pickup moves towards the player
    public float magnetRange = 5f; // Distance within which the health pickup starts moving towards the player
    private bool isMagnetized = false; // Flag to check if coin should move towards player
    private GameObject player;
    //public Player player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Check distance from the health pickup to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // If the player is within range, set isMagnetized to true
            if (distanceToPlayer < magnetRange)
            {
                isMagnetized = true;
            }

            // If isMagnetized is true, move the health pickup towards the player
            if (isMagnetized)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, magnetSpeed * Time.deltaTime);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.AddHealth(10); 
                Destroy(gameObject);
                GameManager.healthCollected += 10;
            }
            else
            {
                Debug.LogError("Player script not found on the player GameObject, I broke somthing and don't know how to fix it.");
            }
        }
    }




}