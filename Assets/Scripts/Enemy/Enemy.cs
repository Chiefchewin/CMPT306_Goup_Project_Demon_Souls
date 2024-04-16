using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] public float health = 100;
    [SerializeField] private float currentHealth = 100;
    public GameObject HealthDrop;
    [SerializeField] private float size = 5.0f;
    [SerializeField] private float damageTime = 1f;
    [SerializeField] private float damageRate = 0.2f;
    [SerializeField] private float damageToPlayer = 2f;
    private int HealthDropChance;
    private GameObject player;


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
        GetComponent<Animator>().Play("catHurt1"); 
        GetComponent<Animator>().Play("catHurt3");
        GetComponent<Animator>().Play("catHurt4");
        GetComponent<Animator>().Play("catHurt5");
        GetComponent<Animator>().Play("catHurt6");
      
        if (currentHealth <= 0)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            ScoreManager.instance.AddPoint(1);
            Destroy(this.gameObject);
            // Chance to drop one health pick-up
            int player_health = (int) player.GetComponent<Player>().GetHealth();
            // Player at or below 50 HP
            if (player_health <= 50)
            {    
                HealthDropChance = Random.Range(80, 100);
            }
            // Player above 50 HP
            if (player_health > 50)
            {
                HealthDropChance = Random.Range(1, 100);
            }
            if (HealthDropChance > 95)
            {
                Instantiate(HealthDrop, transform.position, transform.rotation);
            }
        }
    }
    
}
