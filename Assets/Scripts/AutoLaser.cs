using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoLaser : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float fireInterval = 7f;
    [SerializeField] private float autoAimRange = 10f;
    [SerializeField] private float nightBuffFactor = 2.0f;
    [SerializeField] private AudioClip laserSound;

    private float damage = 50.0f;

    private AudioSource _soundSrc;
    // Start is called before the first frame update
    void Start()
    {
        _soundSrc = GameObject.FindWithTag("LaserSound").GetComponent<AudioSource>();
        StartCoroutine(RepeatEffect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnEnable()
    {
        DayNightManager.OnDayCycleEnd += HandleDayCycleEnd;
        DayNightManager.OnNightTime += HandleNightStart;
    }

    private void OnDisable()
    {
        DayNightManager.OnDayCycleEnd -= HandleDayCycleEnd;
        DayNightManager.OnNightTime -= HandleNightStart;
    }

    private void HandleDayCycleEnd(int daysSurvived)
    {
        // Reset damage back on day end
        StartCoroutine(DelayedDebuffDamageStart(7));
    }
    
    IEnumerator DelayedDebuffDamageStart(int seconds)
    {
        // Actually takes 7 seconds before day shows
        yield return new WaitForSeconds(seconds);
        damage = 50.0f;
    }
    
    private void HandleNightStart()
    {
        // Buff damage up on night start
        damage *= nightBuffFactor;
    }
    

    private IEnumerator RepeatEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            Shoot();
        }
    }

    private void Shoot()
    {
        // Find all colliders within  the specified range
        Collider2D[] allColliders = Physics2D.OverlapCircleAll(transform.position, autoAimRange);

        // Filter colliders based on tags
        Collider2D[] enemyColliders = allColliders
            .Where(collider => collider.CompareTag("Enemy") || collider.CompareTag("Boss"))
            .ToArray();

        if (enemyColliders.Length > 0)
        {
            // Choose a random enemy from the list
            int randomIndex = Random.Range(0, enemyColliders.Length);
            Transform targetEnemy = enemyColliders[randomIndex].transform;

            // Calculate the direction to the target enemy
            Vector3 direction = (targetEnemy.position - transform.position).normalized;

            _soundSrc.clip = laserSound;
            _soundSrc.Play();
            // Instantiate the projectile with the calculated direction
            GameObject laser = Instantiate(projectile, transform.position, Quaternion.LookRotation(direction));
            laser.GetComponent<Laser>().damage = damage;
        }
    }
}
