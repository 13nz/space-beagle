using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void NextLevel()
    {
        PlayerData.level++;
        SceneManager.LoadScene("Game");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            gameObject.GetComponent<AudioSource>().Play();
            NextLevel();
        }
    }
}
