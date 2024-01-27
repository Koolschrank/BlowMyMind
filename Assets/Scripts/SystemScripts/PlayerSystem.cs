using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    // make this a singleton
    public static PlayerSystem instance;

    [SerializeField] PlayerConnector[] playerConnectors;
    List<GameObject> players = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }

    



    // get players
    public List<GameObject> GetPlayers()
    {
        return players;
    }

    // get player random
    public GameObject GetPlayerRandom()
    {
        if (players.Count > 0)
        {
            return players[UnityEngine.Random.Range(0, players.Count)];
        }
        return null;
    }

    public GameObject GetPlayer(int index)
    {
        if (index < players.Count)
        {
            return players[index];
        }
        return null;
    }

    public Vector3 minPlayerSpawnPosition = new Vector3(-5f, 10f, -5f);
    public Vector3 maxPlayerSpawnPosition = new Vector3(5f, 10f, 5f);
    public void AddPlayer(GameObject newPlayer)
    {
        // random position
        Respawn(newPlayer);


        if (players.Count >= playerConnectors.Length)
        {
            return;
        }
        playerConnectors[players.Count].ConnectListenersToPlayer(newPlayer);
        players.Add(newPlayer);

    }

    public void Respawn(GameObject playerToReSpawn)
    {
        Vector3 pos = new Vector3(UnityEngine.Random.Range(minPlayerSpawnPosition.x, maxPlayerSpawnPosition.x), UnityEngine.Random.Range(minPlayerSpawnPosition.y, maxPlayerSpawnPosition.y), UnityEngine.Random.Range(minPlayerSpawnPosition.z, maxPlayerSpawnPosition.z));
        playerToReSpawn.transform.position = pos;

    }
}

[Serializable]
public class PlayerConnector
{
    [SerializeField] ActionListener[] ammoListener;
    [SerializeField] ActionListener[] healthListener;

    public void ConnectListenersToPlayer(GameObject player)
    {
        PlayerCaracter playerMove = player.GetComponent<PlayerCaracter>();
        foreach (var listener in ammoListener)
        {
            playerMove.lives.AddListener(listener);
        }

        foreach (var listener in healthListener)
        {
            playerMove.hitMultiplier.AddListener(listener);
        }


        
    }
}
