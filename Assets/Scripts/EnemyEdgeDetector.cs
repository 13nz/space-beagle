using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEdgeDetector : MonoBehaviour
{
    EnemyBehavior parentScript;
    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<EnemyBehavior>();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        parentScript.ChildCollision();
    }
}
