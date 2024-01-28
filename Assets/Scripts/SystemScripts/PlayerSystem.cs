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

        [SerializeField] private List<UI_Points> playerLivePoints;
        [SerializeField] private List<Material> playerPantMaterials;
        [SerializeField] private List<Material> playerShirtMaterials;
        [SerializeField] private VictoryScreen victoryScreen;
        [SerializeField] PlayerConnector[] playerConnectors;
        List<GameObject> players = new List<GameObject>();
        [SerializeField] public Transform[] spawnPoints;
        [SerializeField] public GameObject StartUI;
        [SerializeField] public SoundEffectValue playerDeathSound;
        [SerializeField] public SlowDownValue playerDeathSlowDown;
        bool gameStarted = false;

        //unity event when player dies
        public UnityEngine.Events.UnityEvent OnPlayerDie;



        public void StartGame()
        {
            gameStarted = true;
            StartUI.SetActive(false);

            foreach (var player in players)
            {
                player.GetComponent<PlayerCharacter>().SetInputEnabled(true);
            }
        }

        public bool IsGameStarted()
        {
            return gameStarted;
        }
    
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
            // spawn player at spanw point
            newPlayer.transform.position = spawnPoints[players.Count].position;
            newPlayer.transform.rotation = spawnPoints[players.Count].rotation;
            newPlayer.GetComponent<PlayerCharacter>().title = (players.Count + 1).ToString();


            if (players.Count >= playerConnectors.Length)
            {
                return;
            }

            newPlayer.GetComponent<PlayerCharacter>().SetClothsMaterials(playerPantMaterials[players.Count],
                playerShirtMaterials[players.Count]);
            playerConnectors[players.Count].ConnectListenersToPlayer(newPlayer);
            playerLivePoints[players.Count].SetUiPointsColor(playerShirtMaterials[players.Count].color);
            players.Add(newPlayer);
            newPlayer.GetComponent<PlayerCharacter>().SetInputEnabled(gameStarted);


        }

        public void Respawn(GameObject playerToReSpawn)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(minPlayerSpawnPosition.x, maxPlayerSpawnPosition.x), UnityEngine.Random.Range(minPlayerSpawnPosition.y, maxPlayerSpawnPosition.y), UnityEngine.Random.Range(minPlayerSpawnPosition.z, maxPlayerSpawnPosition.z));
            playerToReSpawn.transform.position = pos;

        }



        public bool CheckForWinner()
        {
            // player die event
            OnPlayerDie.Invoke();
            playerDeathSlowDown.Play();
            playerDeathSound.Play();



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
            if (alivePlayers == 1)
            { 
                PlayerWin(playerToWin);
                return true;
            }

            return false;
        }

        public void PlayerWin(PlayerCharacter playerToWin)
        {
            victoryScreen.Activate(playerToWin != null ? playerToWin.title : "Nobody", playerToWin.color);
            // debug log
            Debug.Log("Player " + (playerToWin != null ? playerToWin.title : "null") + " wins!");
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
                playerMove.damage.AddListener(listener);
            }
        }
    }

    
}