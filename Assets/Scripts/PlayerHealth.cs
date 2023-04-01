using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.level == 1)
        {
            PlayerData.lives = PlayerData.maxLives;
        }
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log("Invincible: " + PlayerData.invincible.ToString());
        if (!PlayerData.invincible)
        {
            PlayerData.lives -= damage;
            PlayerData.invincible = true;
            Invoke("ResetInvulnerability", 2);
        }
        
        if (PlayerData.lives <= 0)
        {
            Time.timeScale = 0;
            GameObject.Find("PlayerParent").GetComponent<GameControl>().GameOver();
            //DestroyImmediate(gameObject, true);
            //SceneManager.LoadScene("Game");
        }
    }

    void ResetInvulnerability()
    {
        PlayerData.invincible = false;
    }
}
