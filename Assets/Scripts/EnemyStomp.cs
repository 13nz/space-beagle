using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{

    public float bounceForce;
    public Rigidbody2D PlayerBody;
    [SerializeField] GameObject heart;
    [SerializeField] GameObject diamond;
    AudioSource src;
    ParticleSystem ps;

    private void Start() 
    {
        src = transform.parent.gameObject.GetComponent<AudioSource>();    
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "EnemyCheck")
        {
            PlayerBody = other.transform.parent.GetComponent<Rigidbody2D>();
            PlayerBody.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
            PlayerData.score++;
            if (Random.Range(0, 15) == 1)
            {
                gameObject.GetComponent<AudioSource>().Play();
                Instantiate(heart, new Vector3Int((int)transform.position.x, (int)transform.position.y + 1, 0), Quaternion.identity);
            }
            else if (Random.Range(0, 20) == 1)
            {
                gameObject.GetComponent<AudioSource>().Play();
                Instantiate(diamond, new Vector3Int((int)transform.position.x, (int)transform.position.y + 1, 0), Quaternion.identity);
            }
            
            src.Play();
            
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        
        yield return new WaitForSeconds (src.clip.length);
        Destroy(transform.parent.gameObject);
    }
}
