using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float playerHealth = 100.0f;
    private int keysObtained;
    
    private Vector2 movment;
    private Vector2 lastHorizontalMovement = Vector2.left; 
    private bool facingLeft = false;
    
    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            GetComponent<SpriteRenderer>().flipX = false;
            facingLeft = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            GetComponent<SpriteRenderer>().flipX = true;
            facingLeft = true;
        }
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

    public bool PlayerFaceing()
    {
        return facingLeft;
    }



    // Remove health from player
    // float damage: The ammount of damage the player is to take.
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        //Debug.Log("Damage taken! Current health: " + GetHealth());
        if (playerHealth <= 0)
        {
            //GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
            //Destroy(effect, 1.0f);
            Destroy(this.gameObject);
            GameManager.instance.GameOver();
        }

        //Cumulative damage taken
        GameManager.totalDamageTaken += (int) damage;
    }

    // Restore players health
    public void AddHealth(float health)
    {
        // As long as the player 
        if (playerHealth < 100f)
        {

            playerHealth = health + playerHealth;
            //Debug.Log("Health added in player script! Current health: " + GetHealth());
            //Player Health cannot go above 100, if it does, then the health ui breaks. 
            if (playerHealth > 100f)
            {
                playerHealth = 100f;
            }
        }

    }

    public float GetHealth()
    {
        return playerHealth;
    }

    public void AddKeys(int n)
    {
        keysObtained += n;
        
        if (keysObtained < 0)
            keysObtained = 0;
    }
    
    public void UseKeys(int n)
    {
        keysObtained -= n;

        if (keysObtained < 0)
        {
            keysObtained = 0;
        }
    }

    public int GetKeys()
    {
        return keysObtained;
    }
}
