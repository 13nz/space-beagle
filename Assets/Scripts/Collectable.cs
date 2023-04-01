using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Collectable : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] bool rare;
    AudioSource src;

    private void Start() 
    {
        src = GetComponent<AudioSource>();

        if (rare)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        }
    }
     private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            src.Play();
            PlayerData.score += value;
            StartCoroutine(DisableItem());
            
        }
    }

    IEnumerator DisableItem()
    {
        yield return new WaitForSeconds(src.clip.length);
        gameObject.SetActive(false);
    }   
}
