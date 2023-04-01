using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public PlayerHealth playerHealth;
    public  Rigidbody2D PlayerBody;
    [SerializeField] int KBForce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!PlayerMove.isDashing)
            {
                PlayerBody = other.GetComponent<Rigidbody2D>();
            
                Vector2 difference = -(transform.position - other.transform.position).normalized;

                playerHealth.TakeDamage(damage);
                StartCoroutine(other.GetComponent<PlayerMove>().Knockback(difference, KBForce));
                PlayerMove.knockback = false;
            }
            else
            {
                gameObject.GetComponent<AudioSource>().Play();
                PlayerData.score++;
                StartCoroutine(Kill());
            }
            
        }
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds (gameObject.GetComponent<AudioSource>().clip.length);
        Destroy(transform.gameObject);
    }
}
