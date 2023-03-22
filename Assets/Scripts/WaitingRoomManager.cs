using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visyde;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingRoomManager : MonoBehaviour
{
    public CustomGamePlayerItem playerItemPrefab;
    public Transform playerItemHandler;
    public ChatSystem chatSystem;
    public Text chosenMapText;
    public Text chosenPlayerNumberText;
    public Text enableBotsText;
    public Text currentNumberOfPlayersInRoomText;
    //public Button startBTN;
    //public string gameSceneName = "";

    void OnEnable()
    {
        Connector.instance.onRoomListChange += onRoomListUpdate;
        Connector.instance.onCreateRoomFailed += onCreateRoomFailed;
        Connector.instance.onJoinRoom += OnJoinedRoom;
        Connector.instance.onLeaveRoom += OnLeftRoom;
        Connector.instance.onPlayerJoin += OnPlayerJoined;
        Connector.instance.onPlayerLeave += OnPlayerLeft;
    }
    void OnDisable()
    {
        Connector.instance.onRoomListChange -= onRoomListUpdate;
        Connector.instance.onCreateRoomFailed -= onCreateRoomFailed;
        Connector.instance.onJoinRoom -= OnJoinedRoom;
        Connector.instance.onLeaveRoom -= OnLeftRoom;
        Connector.instance.onPlayerJoin -= OnPlayerJoined;
        Connector.instance.onPlayerLeave -= OnPlayerLeft;
    }
    private void Update()
    {
        RefreshPlayerList();
        chosenMapText.text = Connector.instance.maps[(int)PhotonNetwork.CurrentRoom.CustomProperties["map"]];
        chosenPlayerNumberText.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("customAllowBots")) enableBotsText.text = (bool)PhotonNetwork.CurrentRoom.CustomProperties["customAllowBots"] ? "Yes" : "No";
    }
    void onRoomListUpdate(int roomCount)
    {
       
    }
  
    void OnPlayerJoined(Player player)
    {

        Invoke("delay", 5);

    }
    void delay()
    {
        Debug.LogError("Player Joinedddd");
        // When a player connects, update the player list:
        //c  RefreshPlayerList();
        Invoke("RefreshPlayerList", 5);
        // Notify other players through chat:
       // chatSystem.SendSystemChatMessage(player.NickName + " joined the game.", false);
    }
    void onCreateRoomFailed()
    {
        // Display error:
        DataCarrier.message = "Custom game creation failed.";
    }
    // Subscribed to Connector's "OnJoinRoom" event:
    void OnJoinedRoom()
    {
        // Update the player list when we join a room:
        Invoke("delay2", 5);

    }
    void delay2()
    {
        Debug.LogError("joined roomasdsd");
    }
    void OnPlayerLeft(Player player)
    {
        // When a player disconnects, update the player list:
        RefreshPlayerList();

        // Notify other players through chat:
        chatSystem.SendSystemChatMessage(player.NickName + " left the game.", true);
    }
    void OnLeftRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
           
        }
    }
    public void RefreshPlayerList()
    {

        // Clear list first:
        foreach (Transform t in playerItemHandler)
        {
            Destroy(t.gameObject);
        }

        // Repopulate:
       Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            CustomGamePlayerItem cgp = Instantiate(playerItemPrefab, playerItemHandler, false);
            cgp.Set(players[i]);
        }

        // Player number in room text:
        currentNumberOfPlayersInRoomText.text = "Players (" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")";

      bool  allowBots = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("customAllowBots") && (bool)PhotonNetwork.CurrentRoom.CustomProperties["customAllowBots"];
      // startBTN.interactable = PhotonNetwork.IsMasterClient && ((players.Length > 1 && !allowBots) || (allowBots));
        // Enable/disable start button:
    }
    public void StartGame()
    {
        Connector.instance.whenIsayStart = true;
        Connector.instance.StartCustomGame();
        Debug.LogError("Starting game");
    }
    public void Leave()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }
    }
}
