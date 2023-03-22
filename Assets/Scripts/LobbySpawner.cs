using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Visyde;
using TMPro;
public class LobbySpawner : MonoBehaviour
{
    public static LobbySpawner inst;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
    }
    [SerializeField] List<Transform> spawnPoints;
    public string playerPrefab;
    int numberPlayers;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void SpawnPlayer()
    {
        CheckPlayers();
    }
    public void LoadNextScene()
    {
        Connector.instance.StartCustomGame();
    }
   
    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(numberPlayers);
        //if the number of player is heigher than the number of spawnpoint in the game (in this case 4),
        //spawn the players in round order
        if (numberPlayers == 0)
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[0].position, Quaternion.identity);
            //  testplayer.GetComponentInChildren<TextMeshPro>().text = PhotonNetwork.LocalPlayer.NickName;
        }
        else if (numberPlayers == 1)
        {
           PhotonNetwork.Instantiate(playerPrefab, spawnPoints[1].position, Quaternion.identity);
            // testplayer.GetComponentInChildren<TextMeshPro>().text = PhotonNetwork.LocalPlayer.NickName;
        }
        else if (numberPlayers == 2)
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[2].position, Quaternion.identity);
        }
        else if (numberPlayers == 3)
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[3].position, Quaternion.identity);
        }
        else if (numberPlayers == 4)
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[4].position, Quaternion.identity);
        }
        else if (numberPlayers == 5)
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[5].position, Quaternion.identity);
        }
        else if (numberPlayers == 6)
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[6].position, Quaternion.identity);
        }
        else if (numberPlayers == 7)
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[7].position, Quaternion.identity);
        }
        else
        {
            int rnd = Random.Range(0, spawnPoints.Count);
            PhotonNetwork.Instantiate(playerPrefab, spawnPoints[rnd].position, Quaternion.identity);
        }
    }
}
