using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float timeToHide;
    public float timeInvestedInGame;
    public GameObject player;
    public UIManager uImanager;
    [SerializeField] GameObject hideableObject;

    public FixedJoystick joyStick;

    public bool hiding=false;
    public bool shouldStop;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayerVisibility()
    {
        StartCoroutine(PlayerVisDelay());
    }
    IEnumerator PlayerVisDelay()
    {
        if (hiding)
        {
            Debug.LogError("Hiding");
            joyStick.gameObject.SetActive(false);
            player.transform.GetChild(1).GetComponent<CapsuleCollider2D>().enabled = false;
            player.GetComponent<PlayerNetworkController>().PlayerHidingStatus(hiding);
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(1).GetComponent<Animator>().SetBool("Hide",true);
           
           // yield return new WaitForSeconds(3);
            
           // player.GetComponent<PlayerNetworkController>().PlayerHidingStatus(hiding);
            //yield return new WaitForSeconds(1);
            //player.SetActive(false);
        }
        else
        {
           
            //player.GetComponent<PlayerNetworkController>().PlayerHidingStatus(hiding);
            //player.SetActive(true);
            if(uImanager.lostGame)
            {
                Debug.LogError("lost game");
             //   yield return new WaitForSeconds(2);
              player.GetComponent<PlayerNetworkController>().PlayerDisappear(false);

                yield return new WaitForSeconds(3);
                  player.transform.GetChild(1).GetComponent<Animator>().SetBool("Hide", false);
               
                yield return new WaitForSeconds(2);
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.GetComponent<PlayerNetworkController>().PlayerHidingStatus(hiding);
                uImanager.lostGame = false;
                joyStick.gameObject.SetActive(true);
                player.transform.GetChild(1).GetComponent<CapsuleCollider2D>().enabled = true;
                yield break;
            }
        
         player.GetComponent<PlayerNetworkController>().PlayerDisappear(false);
          
            player.transform.GetChild(1).GetComponent<Animator>().SetBool("Hide", false);
          

            yield return new WaitForSeconds(2);
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.GetComponent<PlayerNetworkController>().PlayerHidingStatus(hiding);
               joyStick.gameObject.SetActive(true);
               player.transform.GetChild(1).GetComponent<CapsuleCollider2D>().enabled = true;
        }
       
    }
    public void Attack()
    {
        StartCoroutine(AttackDelay()); 
    }
   IEnumerator AttackDelay()
    {
        player.transform.GetChild(1).GetComponent<Animator>().SetBool("Attack", true);
        player.GetComponent<CharacterAnimationController>().spitAnimator.SetBool("Spit", true);
        yield return new WaitForSeconds(2);
        player.transform.GetChild(1).GetComponent<Animator>().SetBool("Attack", false);
        player.GetComponent<CharacterAnimationController>().spitAnimator.SetBool("Spit", false);
        uImanager.attackButton.interactable = true;
    }
    public void Hide()
    {
        uImanager.ShowButton();
    }

    public void SetHideableObject(GameObject gameObject)
    {
        hideableObject = gameObject;
    }

    public void HideableObjectNull()
    {
        hideableObject = null;
    }

    public void StopHide()
    {
        uImanager.HideButton();
    }

    public void HideButton()
    {
        uImanager.ButtonHidden();
    }

    public void ShowHideButton()
    {
        uImanager.ShowUnHideButton();
    }

    public void HideClicked()
    {
        if (hideableObject.GetComponent<ObjectProperties>() != null)
        {
          
            if (hideableObject.GetComponent<ObjectProperties>().allGamesDone == false)
            {
                uImanager.ShowMiniGamePanal();
                hideableObject.GetComponent<ObjectProperties>().SelectGame();
                hideableObject.GetComponent<ObjectProperties>().PlayerHidden();
                uImanager.incrementTime = true;
            }
            else if (hideableObject.GetComponent<ObjectProperties>().allGamesDone == true)
            {
                player.GetComponent<PlayerNetworkController>().PlayerDisappear(true);
                uImanager.timeForHidingTxt.gameObject.SetActive(true);
                uImanager.ShowUnHideButton();
            }
        }
      //  StartCoroutine(HideClickedDelay());
    }
    
    public void OnUnHideClick()
    {
        if (hideableObject.GetComponent<ObjectProperties>() != null)
        {
            hideableObject.GetComponent<ObjectProperties>().DisableGames();
        }
        //hideableObject = null;
    }

    public void GameWin()
    {
        if (hideableObject.GetComponent<ObjectProperties>() != null)
        {
            hideableObject.GetComponent<ObjectProperties>().PlayerWin();
        }
         player.GetComponent<PlayerNetworkController>().PlayerDisappear(true);
        uImanager.HideMiniGamePanal();
    }

    public void GameLose()
    {
        if (hideableObject.GetComponent<ObjectProperties>() != null)
        {
           
            hideableObject.GetComponent<ObjectProperties>().PlayerLose();
        }
        
        uImanager.OnGameLose();
    }
}
