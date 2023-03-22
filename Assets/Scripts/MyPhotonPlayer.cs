using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.IO;
using Photon.Pun;

public class MyPhotonPlayer : MonoBehaviour
{
    PhotonView myPv;
    GameObject myPlayerAvatar;

    Player[] allPlayers;
    int mynumberInRoom;

    // Start is called before the first frame update
    void Start()
    {
        myPv = GetComponent<PhotonView>();

        allPlayers = PhotonNetwork.PlayerList;

        foreach(Player p in allPlayers)
        {
            if(p!= PhotonNetwork.LocalPlayer)
            {
                mynumberInRoom++;
            }
        }

        if(myPv.IsMine)
        {
          //  myPlayerAvatar = PhotonNetwork.Instantiate("Gray character", LobbySpawner.inst.spawnPoints[mynumberInRoom].position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
