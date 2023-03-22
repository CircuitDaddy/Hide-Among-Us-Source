using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideButton : MonoBehaviour
{
    float seconds = 0;
    public float timeCount = 0;
    public bool stoptimer = false;
    [SerializeField] Text timerText;

    [Header("Task Incompleted")]
    [SerializeField] GameObject taskIncompletedObject;

    [Header("Task completed")]
    [SerializeField] GameObject taskcompletedObject;

    private void OnEnable()
    {
        stoptimer = false;
        seconds = 0;
        timerText.text = null;
        timeCount = LevelManager.Instance.timeToHide;
        taskIncompletedObject.SetActive(false);
        taskcompletedObject.SetActive(false);
        LevelManager.Instance.HideButton();
    }


    private void Update()
    {
        if ((timeCount>=0) && stoptimer==false)
        {
            timeCount -= Time.deltaTime;
            DisplayTime(timeCount);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = seconds.ToString();
    }

    public void IncompleteTask()
    {
        StartCoroutine(TaskInompletedRoutine());
    }

    IEnumerator TaskInompletedRoutine()
    {
        taskIncompletedObject.SetActive(true);
        yield return new WaitForSeconds(2);
        taskIncompletedObject.SetActive(false);
        LevelManager.Instance.GameLose();
        LevelManager.Instance.HideableObjectNull();
    }
    private void OnDisable()
    {
        if (LevelManager.Instance.hiding==true)
        {
            LevelManager.Instance.uImanager.timeForHidingTxt.gameObject.SetActive(true);
           
            LevelManager.Instance.ShowHideButton();
        }
    }
}
