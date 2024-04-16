using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameOverScreen GameOverUI;
    [SerializeField] public GameObject player;
    
    public static int totalDamageTaken;
    public static int healthCollected;
    public static int totalCatsExiled;
    public static int daysSurvived;

    public Slash slash;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        SceneManager.LoadScene("DeathScreen");
    }

    public void SetPlayScene()
    {
        SceneManager.LoadScene("ProofOfConcept");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }


    public void AddDamage(int moreDamage)
    {
        slash.AddDamage(moreDamage);
    }
    public void ReduceDamage(int LessDamage)
    {
        slash.ReduceDamage(LessDamage);
    }
    public void RestoreDamage()
    {
        slash.RestoreDamage();
    }


}
