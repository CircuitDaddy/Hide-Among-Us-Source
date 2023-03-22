using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerView;
    [SerializeField] GameObject playerTorch;

    [SerializeField] PhotonView photonViewObject;
    [SerializeField] GameObject playerBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerHidingStatus(bool Status=false)
    {
        photonViewObject.RPC("PlayerHidingStatusRPCCall", RpcTarget.All, Status);
    }
    
    [PunRPC]
    void PlayerHidingStatusRPCCall(bool Status=false)
    {
        playerTorch.SetActive(!Status);
      playerBody.GetComponent<CapsuleCollider2D>().enabled = !Status;
      //  playerTorch.SetActive(!Status);
        
    }
      public void PlayerDisappear(bool Status = false)
      {
             photonViewObject.RPC("DisappearPlayer", RpcTarget.All, Status);
      }
   [PunRPC]
   void DisappearPlayer(bool Status=false)
   {
        playerBody.GetComponent<SpriteRenderer>().enabled = !Status;
   }

    
}
