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
            Instantiate(player, MapGeneration.playerSpawnPos);
            playerSpawned = true;
        }
    }
}
