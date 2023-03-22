using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{

    [SerializeField] List<MiniGamesData> miniGamesList = new List<MiniGamesData>();
    [SerializeField] int timeToMakeItHideable = 0;
    int randomGame;
   
    public bool isObjectHideAble = true;
    public bool allGamesDone = false;
   
    private void Update()
    {
        if (miniGamesList.Count==0 && allGamesDone==false)
        {
            allGamesDone = true;
        }

       
    }

    IEnumerator MakeObjectHidableRoutine()
    {

       
       yield return new WaitForSeconds(timeToMakeItHideable);
        Debug.LogError("kjksj" + timeToMakeItHideable);
        isObjectHideAble = true;
    }

    public void PlayerHidden()
    {
        Debug.LogError("Player is Hiding");
        isObjectHideAble = false;
        StartCoroutine(MakeObjectHidableRoutine());
    }

    public void SelectGame()
    {
        if (miniGamesList.Count != 0)
        {
            for (int i = 0; i < miniGamesList.Count; i++)
            {
                miniGamesList[i].miniGame.SetActive(false);
            }
            randomGame = Random.Range(0, miniGamesList.Count);
            miniGamesList[randomGame].miniGame.SetActive(true);
        }
    }

    public void PlayerWin()
    {
        if (miniGamesList.Count != 0)
        {
            miniGamesList[randomGame].miniGame.SetActive(false);
            miniGamesList.Remove(miniGamesList[randomGame]);
        }

    }
    public void PlayerLose()
    {
        if (miniGamesList.Count != 0)
        {
            miniGamesList[randomGame].miniGame.SetActive(false);
            LevelManager.Instance.OnUnHideClick();
        }
    }
    
    public void DisableGames()
    {
        for (int i = 0; i < miniGamesList.Count; i++)
        {
            miniGamesList[i].miniGame.SetActive(false);
        }
    }
}
[System.Serializable]
public class MiniGamesData
{
    public GameObject miniGame;
}
