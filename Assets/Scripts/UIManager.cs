using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject hideButton;
    [SerializeField] GameObject unHideButton;
    [SerializeField] GameObject miniGamePanal;
    public Button attackButton;
    public TextMeshProUGUI timeForHidingTxt;
   public bool lostGame;
   public bool incrementTime;

    private void Update()
    {
        if(incrementTime == true)
        {
            LevelManager.Instance.timeInvestedInGame += Time.deltaTime;
        }
      
    }
    public void ShowButton()
    {
        hideButton.SetActive(true);
    }

    public void HideButton()
    {
        hideButton.SetActive(false);
    }

    public void OnHideClick()
    {
        LevelManager.Instance.shouldStop = true;
        StartCoroutine(OnHideClickDelay());
    }
    IEnumerator OnHideClickDelay()
    {
        LevelManager.Instance.hiding = true;
        LevelManager.Instance.PlayerVisibility();
        yield return new WaitForSeconds(2);
       
        LevelManager.Instance.HideClicked();
        StartCoroutine(ShowPlayerRoutine());
    }
    public void OnUnHideClick()
    {
        LevelManager.Instance.shouldStop = false;
        incrementTime = false;
        LevelManager.Instance.timeInvestedInGame = 0;
        timeForHidingTxt.gameObject.SetActive(false);
        unHideButton.SetActive(false);
        LevelManager.Instance.hiding = false;
        LevelManager.Instance.PlayerVisibility();
        LevelManager.Instance.OnUnHideClick();
        StopAllCoroutines();
        miniGamePanal.SetActive(false);
    }
    public void OnAttackClick()
    {
        attackButton.interactable = false;
        LevelManager.Instance.Attack();
    }
    public void ShowMiniGamePanal()
    {
        miniGamePanal.SetActive(true);
    }

    public void HideMiniGamePanal()
    {
        miniGamePanal.SetActive(false);
    }

    public void OnGameLose()
    {
        LevelManager.Instance.shouldStop = false;
        LevelManager.Instance.hiding = false;
        incrementTime = false;
        LevelManager.Instance.timeInvestedInGame = 0;
        unHideButton.SetActive(false);
        hideButton.SetActive(false);
        StopAllCoroutines();
       lostGame = true;
        miniGamePanal.SetActive(false);
     LevelManager.Instance.PlayerVisibility();
      
    }
      
    public void ShowUnHideButton()
    {
        unHideButton.SetActive(true);
        hideButton.SetActive(false);
    }

    public void ButtonHidden()
    {
        unHideButton.SetActive(false);
        hideButton.SetActive(false);
    }

    IEnumerator ShowPlayerRoutine()
    {
       
        yield return new WaitForSeconds(LevelManager.Instance.timeToHide);
        LevelManager.Instance.hiding = false;
        lostGame = true;
        incrementTime = false;
        timeForHidingTxt.gameObject.SetActive(false);
        LevelManager.Instance.timeInvestedInGame = 0;
        yield return new WaitForSecondsRealtime(1);
        LevelManager.Instance.PlayerVisibility();
        unHideButton.SetActive(false);
        //miniGamePanal.SetActive(false);
        LevelManager.Instance.OnUnHideClick();
    }
}
