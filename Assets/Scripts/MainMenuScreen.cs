using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public void Start()
    {
        MapGeneration.src.Pause();
        Time.timeScale = 0;
        //PlayerMove.IsPaused = true;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            StartGame();
        }
    }
    // Update is called once per frame
    public void StartGame()
    {
        Time.timeScale = 1;
        //PlayerMove.IsPaused = false;
        gameObject.SetActive(false);
        MapGeneration.src.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
