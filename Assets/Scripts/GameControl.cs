using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] GameOverScreen gameOverScreen;
    // Start is called before the first frame update
    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverScreen.Setup();
    }
}
