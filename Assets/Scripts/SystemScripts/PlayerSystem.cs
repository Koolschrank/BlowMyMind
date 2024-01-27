using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace SystemScripts
{
    public class PlayerSystem : MonoBehaviour
    {
        // make this a singleton
        public static PlayerSystem instance;

        [SerializeField] private List<Color> playerColors;
        [SerializeField] private VictoryScreen victoryScreen;
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

        public bool CheckForWinner()
        {
            PlayerCharacter playerToWin = null;
            int alivePlayers = 0;
            foreach (var player in players)
            {
                var playerObj = player.GetComponent<PlayerCharacter>();
                if (playerObj.lives.Value >0)
                {
                    alivePlayers++;
                    playerToWin = playerObj;
                }
            }
            if (alivePlayers <= 1)
            { 
                PlayerWin(playerToWin);
                return true;
            }

            return false;
        }

        public void PlayerWin(PlayerCharacter playerToWin)
        {
            victoryScreen.Activate(playerToWin != null ? playerToWin.name : "Nobody");
            // debug log
            Debug.Log("Player " + (playerToWin != null ? playerToWin.name : "null") + " wins!");
        }
    }

    [Serializable]
    public class PlayerConnector
    {
        [SerializeField] ActionListener[] ammoListener;
        [SerializeField] ActionListener[] healthListener;

        public void ConnectListenersToPlayer(GameObject player)
        {
            PlayerCharacter playerMove = player.GetComponent<PlayerCharacter>();
            foreach (var listener in ammoListener)
            {
                playerMove.lives.AddListener(listener);
            }

            foreach (var listener in healthListener)
            {
                playerMove.knockBackMultiplier.AddListener(listener);
            }
        }
    }

    
}