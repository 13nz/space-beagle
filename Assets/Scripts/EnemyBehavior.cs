using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rb;
    CapsuleCollider2D cap;
    BoxCollider2D box;
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cap = GetComponent<CapsuleCollider2D>();
        box = GetComponent<BoxCollider2D>();
        ps = GetComponent<ParticleSystem>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //sr.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        sr.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            rb.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, 0f);
        }
    }


    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    public void ChildCollision()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
    }
    
}
