using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    AudioSource src;
    private void Start() 
    {
        src = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerData.lives < PlayerData.maxLives)
            {
                src.Play();
                PlayerData.lives++;
                StartCoroutine(DisableHeart());
            }
        }
    }

    IEnumerator DisableHeart()
    {
        yield return new WaitForSeconds(src.clip.length);
        gameObject.SetActive(false);
    }
}
