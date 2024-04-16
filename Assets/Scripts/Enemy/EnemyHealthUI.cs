using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject Segment;
    [SerializeField] private int EnemyHealth;
    [SerializeField] private int EnemyHealthCheck;
    [SerializeField] private List<GameObject> Segments;
    [SerializeField] private Boss enemy;
    
    private int indexseg;

    void Start()
    {
        EnemyHealth = (int)enemy.GetHealth();
        EnemyHealthCheck = (int)enemy.GetHealth();
        for (int i = 0; i < EnemyHealth/10; i++)
        {
            GameObject currentSegment = Instantiate(Segment);
            currentSegment.transform.SetParent(this.gameObject.transform);
            Segments.Add(currentSegment);
        }
        indexseg = Segments.Count - 1;
    }

    private void UpdateUIDown()
    {
        Segments[indexseg].GetComponent<Image>().color = Color.black;
        indexseg -= 1;
    }

    public void UpdateUIUP()
    {
        int healthToShow = (int)enemy.GetHealth() / 10;
        indexseg = healthToShow - 1;

        for (int i = 0; i < healthToShow; i = i + 1)
        {
            Segments[i].GetComponent<Image>().color = Color.magenta;
        }

    }

    // Update is called once per frame
    void Update()
    {
        EnemyHealth = (int)enemy.GetHealth();

        if (EnemyHealthCheck - EnemyHealth >= 10)
        {
            int updateCount = EnemyHealthCheck - EnemyHealth;
            for (int i = 0; i < updateCount / 10; i = i + 1)
            {
                UpdateUIDown();
            }
            EnemyHealthCheck = EnemyHealth;
        }
        if (EnemyHealth > EnemyHealthCheck)
        {
            EnemyHealthCheck = EnemyHealth; 
            UpdateUIUP();
        }
    }
}
