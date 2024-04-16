using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] public  float health = 100.0f;
    [SerializeField] private float currentHealth;
    public GameObject HealthDrop;
    [SerializeField] private float size = 15.0f; 
    
    [SerializeField] private float damageTime = 1f; 
    [SerializeField] private float damageRate = 0.2f; 
    [SerializeField] private float damageToPlayer = 5f;
    
    public delegate void BossKilledEvent();

    public static event BossKilledEvent OnBossKilled;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    
    private void Movement()
    {
        if (GameManager.instance.player)
        {
            Vector3 targetPosition = GameManager.instance.player.transform.position;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Flip the sprite if moving to the left
            if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-size, size, 1); // Flip the sprite horizontally
            }
            // Reset the sprite orientation if moving to the right
            else if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(size, size, 1); // Reset the sprite's orientation
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && Time.time > damageTime)
        {
            other.transform.GetComponent<Player>().TakeDamage(damageToPlayer);
            damageTime = Time.time + damageRate;
        }
    }

    public float GetHealth()
    {
        return currentHealth; 
    }

    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        GetComponent<Animator>().Play("bossHurt");
        
        if (currentHealth <= 0)
        {
            ScoreManager.instance.AddPoint(10);
            Destroy(this.gameObject);

            // Chance to drop one health pick-up
            int randomNumber = Random.Range(1, 100);
            if (randomNumber > 95) {
                Instantiate(HealthDrop, transform.position, transform.rotation);
            }
            
            // Check to Call Victory
            OnBossKilled?.Invoke();
        }
    }
}
