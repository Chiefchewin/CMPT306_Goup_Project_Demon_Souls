using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine;

public class GameOverScreen : MonoBehaviour
{

    // void Start()
    // {
        // gameObject.SetActive(false);
    // }
    
    public bool Setup()
    { 
        gameObject.SetActive(true);
        return true;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("ProofOfConcept");
        Debug.Log("ProofOfConcept UI");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu UI");
    }

    public void ScoreButton()
    {
        SceneManager.LoadScene("StatusScreen");
        Debug.Log("StatusScreen UI");
    }
    
}
