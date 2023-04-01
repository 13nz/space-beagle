using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public bool playerSpawned = false;
    // Update is called once per frame
    void Update()
    {
        if (MapGeneration.ready && !playerSpawned)
        {
            GameObject go = Instantiate(player, MapGeneration.playerSpawnPos);
            go.transform.parent = GameObject.Find("PlayerParent").transform;
            playerSpawned = true;
        }
    }
}
