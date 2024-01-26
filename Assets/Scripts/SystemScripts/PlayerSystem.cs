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

    public void AddPlayer(GameObject newPlayer)
    {
        
        if (players.Count >= playerConnectors.Length)
        {
            return;
        }
        playerConnectors[players.Count].ConnectListenersToPlayer(newPlayer);
        players.Add(newPlayer);

    }
}

[Serializable]
public class PlayerConnector
{
    [SerializeField] ActionListener[] ammoListener;


    public void ConnectListenersToPlayer(GameObject player)
    {
        PlayerShoot playerShoot = player.GetComponent<PlayerShoot>();
        foreach (var listener in ammoListener)
        {
            playerShoot.Ammunition.AddListener(listener);
        }
        
    }
}
