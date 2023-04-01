using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallDetector : MonoBehaviour
{
    EnemyBehavior parentScript;
    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<EnemyBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Tilemap") || other.CompareTag("RoomTilemap") || other.CompareTag("Enemy"))
        {
            parentScript.ChildCollision();
        }
        
    }
}
