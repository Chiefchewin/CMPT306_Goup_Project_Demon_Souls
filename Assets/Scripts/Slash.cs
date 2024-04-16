using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private Collider2D slashCollider;
    private GameObject player;
    private float swingInterval = 5f;
    [SerializeField] private float damage = 50.0f;
    [SerializeField] private float nightBuffFactor = 1.5f;
    [SerializeField] private float hitboxActiveTime = 0.5f;
    [SerializeField] private AudioClip slashSound;

    private AudioSource _soundSrc;
    private bool facingLeft;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _soundSrc = GameObject.FindWithTag("GameSoundSource").GetComponent<AudioSource>();
        StartCoroutine(RepeatEffect());
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
            // Every interval enable the collider and create the slashing effect
            yield return new WaitForSeconds(swingInterval);
            slashCollider.enabled = true;
            GameObject newSlashEffect = Instantiate(slashEffect, player.transform.position, transform.rotation);
            _soundSrc.clip = slashSound;
            _soundSrc.Play();
            // Use a coroutine to disable the collider and destroy the slash effect
            StartCoroutine(DestroyEffect(newSlashEffect));
        }
    }

    private IEnumerator DestroyEffect(GameObject effectToDestroy)
    {
        yield return new WaitForSeconds(hitboxActiveTime); 
        slashCollider.enabled = false;
        Destroy(effectToDestroy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
        if (other.transform.tag == "Boss")
        {
            other.GetComponent<Boss>().TakeDamage(damage);
        }
    }

   
    public void AddDamage(int moreDamage)
    {
        damage = damage + moreDamage;
    }
    public void ReduceDamage(int LessDamage)
    {
        damage = damage - LessDamage;
        if (damage < 50f) {
            damage = 50f;
        }
    }
    public void RestoreDamage() {
        damage = 50f;
    
    }

    
}
