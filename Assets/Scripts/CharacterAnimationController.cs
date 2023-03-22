using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class CharacterAnimationController : MonoBehaviourPunCallbacks
{
    public static CharacterAnimationController Instance;

    [SerializeField] Color[] colors;
    //  [SerializeField] GameObject[] visors;
    [SerializeField] List<Sprite> visorSprites;
    [SerializeField] SpriteRenderer mainVisorSprite;
    [SerializeField] Animator bodyAnimator;
    public Animator visorAnimator;
    public Animator spitAnimator;
    public Animator lightAnimator;

   
   // [SerializeField] GameObject UI;

    Color mycolor;


    private void OnEnable()
    {
       
        if (photonView.IsMine)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                photonView.RPC("DisableLight", RpcTarget.AllBuffered, false);
                photonView.RPC("MakePlayerBig", RpcTarget.AllBuffered, new Vector3(1.3f, 1.3f, 1.3f));
            }
           
            if (!PlayerPrefs.HasKey("SelectedVisor"))
            {
                PlayerPrefs.SetInt("SelectedVisor", 1);
            }
            if (!PlayerPrefs.HasKey("CurrentColor"))
            {
                PlayerPrefs.SetString("CurrentColor", "Gray");
            }
            //Debug.LogError(PlayerPrefs.GetInt("SelectedVisor"));
            SetNewColor(PlayerPrefs.GetString("CurrentColor"));
            SetVisor(PlayerPrefs.GetInt("SelectedVisor"));
        }
       
        
    }
    private void Awake()
    {
        if(photonView.IsMine)
        {
            if (Instance == null)
                Instance = this;
        }
       
    }

    [PunRPC]
   void MakePlayerBig(Vector3 scale )
    {
        transform.localScale = scale;
    }
    [PunRPC]
    void DisableLight(bool disable = false)
    {
        transform.GetChild(0).gameObject.SetActive(disable);

    }
    public void PlayWalkAnimation()
    {
        bodyAnimator.SetBool("Walk", true);
        visorAnimator.SetBool("Walk", true);
    }

    public void StopWalkAnimation()
    {
        bodyAnimator.SetBool("Walk", false);
        visorAnimator.SetBool("Walk", false);
    }

    public void PlayHideAnimation()
    {
        bodyAnimator.Play("HideAnimation");
        visorAnimator.Play("HideAnimation");
        lightAnimator.Play("HideAnimation");
    }

    public void PlayUnHideAnimation()
    {
        bodyAnimator.Play("UnHideAnimation");
        visorAnimator.Play("UnHideAnimation");
        lightAnimator.Play("UnHideAnimation");
    }

    public void PlayKillAnimation()
    {
        bodyAnimator.Play("KillAnimation");
        visorAnimator.Play("KillAnimation");
        spitAnimator.Play("KillAnimation");
    }

    public void PlayDeathAnimation()
    {
        bodyAnimator.Play("DeathAnimation");
        visorAnimator.Play("DeathAnimation");
        spitAnimator.Play("DeathAnimation");
    }
    

    [PunRPC]
     void SendVisorInfo(int visorNumber)
    {
        mainVisorSprite.sprite = visorSprites[visorNumber-1];

        //if(SceneManager.GetActiveScene().buildIndex != 0)
      //  visorAnimator.runtimeAnimatorController = controllerParameter[visorNumber-1];
        //for (int i = 0; i < visors.Length; i++)
        //{
        //    if (i == visorNumber - 1)
        //    {
        //        visors[i].gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        visors[i].gameObject.SetActive(false);
        //    }
        //}
        //visorAnimator = visors[visorNumber - 1].gameObject.GetComponent<Animator>();
    }
    public void SetVisor(int visorNumber)
    {
        PlayerPrefs.SetInt("SelectedVisor", visorNumber);
       
        photonView.RPC("SendVisorInfo", RpcTarget.AllBuffered, visorNumber);
        //  for (int i = 0; i < currentVisor.Count; i++)
        //  {
        //      Destroy(currentVisor[i].gameObject);
        //  }
        //  currentVisor.Clear();
        //  GameObject newVisor = Instantiate(visors[visorNumber - 1], visorParent.transform);
        //  currentVisor.Add(newVisor);
        //newVisor.transform.SetParent(visorParent.transform);




        //switch (visorNumber)
        //{
        //    case 0:
        //        Debug.Log(visorNumber);
        //        for(int i=0; i<visors.Length; i++)
        //        {
        //            if (i == visorNumber)
        //            {
        //                visors[visorNumber].gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //                visors[visorNumber].gameObject.SetActive(false);
        //            }
        //        }
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 1:
        //        Debug.Log(visorNumber);
        //        for (int i = 0; i < visors.Length; i++)
        //        {
        //            if (i == visorNumber)
        //            {
        //                visors[visorNumber].gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //                visors[visorNumber].gameObject.SetActive(false);
        //            }
        //        }
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 2:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 3:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 4:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 5:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 6:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 7:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 8:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 9:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 10:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;

        //    case 11:
        //        visorAnimator = visors[visorNumber].gameObject.GetComponent<Animator>();
        //        break;
        //}
    }

    public void SetColor(string color)
    {
      if(photonView.IsMine)
      {
             SetNewColor(color);
      }
      
    }

    public void SetNewColor(string color)
    {
        PlayerPrefs.SetString("CurrentColor", color);
        switch (color)
        {
            case "Blue":
            mycolor = colors[0];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Red":
                mycolor = colors[1];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Yellow":
               mycolor = colors[2];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Green":
                mycolor = colors[3];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Pink":
            
                mycolor = colors[4];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Purple":
                mycolor = colors[5];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Orange":
                mycolor = colors[6];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Turquoise":
               mycolor = colors[7];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Gray":
                mycolor = colors[8];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Dark Gray":
                mycolor = colors[9];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(colors[5].r, mycolor.g, mycolor.b));
                break;

            case "Dark Green":
                mycolor = colors[10];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;

            case "Dark Blue":
                mycolor = colors[11];
             photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(mycolor.r, mycolor.g, mycolor.b));
                break;
        }
    }
     [PunRPC]
    private void RPC_SendColor(Vector3 randomColor)
    {
       
         Color color = new Color(randomColor.x, randomColor.y, randomColor.z);
         bodyAnimator.gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
