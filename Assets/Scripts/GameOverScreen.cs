using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI score;

    public void Setup()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        score.text = "Score: " + PlayerData.score.ToString();
    }

    public void Restart()
    {
        PlayerData.lives = PlayerData.maxLives;
        PlayerData.score = 0;
        PlayerData.level = 1;
        PlayerMove.IsPaused = false;
        //Time.timeScale = 1;
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
