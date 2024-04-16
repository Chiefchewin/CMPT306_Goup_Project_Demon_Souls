using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI highScoreText;
    
    private Player player;
    public int score = 0;

    public int highScore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);

        if (scoreText != null)
        {
            scoreText.text = "Cat souls: " + score;    
        }

        if (highScoreText != null)
        {
            highScoreText.text = "Most Cat souls: " + highScore;
        }
    }
    
    public void AddPoint(int n)
    {
        score += n;
        scoreText.text = "Cat souls: " + score;
        if (highScore < score)
        {
            PlayerPrefs.SetInt("highScore", score);
        }

        //Cumulative cats exiled
        if (n > 0)
        {
            GameManager.totalCatsExiled += n;
        }
        
        if (score < 0)
            score = 0;
    }

    public void BackToGameOverScreen()
    {
        SceneManager.LoadScene("DeathScreen");
        Debug.Log("DeathScreen UI");
    }
    
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu UI");
    }
}
