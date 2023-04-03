using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public void Setup()
    {
        MapGeneration.src.Pause();
        gameObject.SetActive(true);
        //PlayerMove.IsPaused = true;
        Time.timeScale = 0;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        PlayerMove.IsPaused = false;
        MapGeneration.src.Play();
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
