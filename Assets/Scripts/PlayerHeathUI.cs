using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class PlayerHeathUI : MonoBehaviour
{
    [SerializeField] private GameObject Segment;
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerHealthCheck;
    [SerializeField] private List<GameObject> Segments;
    public Player player;
    private int indexseg = 9;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = (int)player.GetHealth();
        playerHealthCheck = (int)player.GetHealth();
        for (int i = 0; i < playerHealth; i = i + 10)
        {
            GameObject currentSegment = Instantiate(Segment);
            currentSegment.transform.SetParent(this.gameObject.transform);
            Segments.Add(currentSegment);
        }
    }


    // Remove one of the players health bars.
    private void UpdateUIDown()
    {
        Segments[indexseg].GetComponent<Image>().color = Color.black;
        indexseg = indexseg - 1;
    }


    // Remove one of the players health bars.
    public void UpdateUIUP()
    {
        int healthToShow = (int)player.GetHealth()/10;
        //Debug.Log("Health to show = " + healthToShow);

        indexseg = healthToShow - 1;

        for (int i = 0; i <healthToShow; i = i + 1)
        {
            Segments[i].GetComponent<Image>().color = Color.green;
        }


    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = (int)player.GetHealth(); // Get the current player health

        // If player health has gone down by more than 10 points, remove a health bar and sub 10 from the checksum. 
        if (playerHealthCheck - playerHealth >= 10)
        {
            int updateCount = playerHealthCheck - playerHealth;
            for (int i = 0; i < updateCount; i = i + 10)
            {
                UpdateUIDown();
            }
            playerHealthCheck = playerHealth;

        }

        // If player health has gone up by more than 10 points, add a health bar and add 10 from the checksum. 
        if (playerHealth > playerHealthCheck)
        {
            playerHealthCheck = playerHealth;
            UpdateUIUP();
        }

    }
}


