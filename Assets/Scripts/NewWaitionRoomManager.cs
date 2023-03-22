using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class NewWaitionRoomManager : MonoBehaviour
{
    PhotonView myPv;

    [SerializeField] float timeToStart;
    float timerToStart;
    bool readyToStart;

    [SerializeField] GameObject startBtn;
    [SerializeField] Text countDownTxt;

    [SerializeField] int nextLevel;
   
    void Start()
    {
        myPv = GetComponent<PhotonView>();
        timerToStart = timeToStart;
    }

    private void Update()
    {
        startBtn.SetActive(PhotonNetwork.IsMasterClient);

        if(readyToStart)
        {
            timerToStart -= Time.deltaTime;
            countDownTxt.text = timerToStart.ToString();
        }
        if(timerToStart <= 0)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(nextLevel);
        }
    }

    public void Play()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            readyToStart = !readyToStart;
        }
    }
}
