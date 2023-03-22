using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartGeneratorMiniGame : MonoBehaviour
{
    public static StartGeneratorMiniGame Instance;

    [SerializeField] float waitForSeconds;
    [SerializeField] Slider slider;
    [SerializeField] GameObject tapPoint;
    [SerializeField] int minRange;
    [SerializeField] int maxRange;
    [SerializeField] int rangeoffset;

    [SerializeField] int tapPointPos;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;
    [SerializeField] Animator animator;

    bool once = true;

    [SerializeField] GameObject hideButton;

    void OnEnable()
    {
        Instance = this;
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        animator.enabled = false;

        tapPointPos = Random.Range(-200, 201);
        tapPoint.transform.localPosition = new Vector3(transform.localPosition.x, tapPointPos, transform.localPosition.z);

        if (tapPointPos < 0)
        {
            minRange = tapPointPos - rangeoffset;
            maxRange = tapPointPos + rangeoffset;
        }
        else
        {
            minRange = tapPointPos - rangeoffset;
            maxRange = tapPointPos + rangeoffset;
        }

        taskCompleted = false;
        taskCompletedObject.SetActive(false);

        StopAllCoroutines();
        slider.value = slider.minValue;
    }
    private void Update()
    {
        if ((hideButton.GetComponent<HideButton>().timeCount <= 0) && taskCompleted == false)
        {
            if (once == true)
            {
                once = false;
                hideButton.GetComponent<HideButton>().IncompleteTask();
            }
        }
    }

    public void OnStartPress()
    {
        StartCoroutine(ChangeSliderValueRoutine());
    }
    IEnumerator ChangeSliderValueRoutine()
    {
        while (true)
        {
            while (slider.value != slider.maxValue)
            {
                slider.value = slider.value + 5f;
                yield return new WaitForSeconds(waitForSeconds);
                if (slider.value == slider.maxValue)
                {
                    break;
                }
            }
            while (slider.value != slider.minValue)
            {
                slider.value = slider.value - 5f;
                yield return new WaitForSeconds(waitForSeconds);
                if (slider.value == slider.minValue)
                {
                    break;
                }
            }
        }
    }
    public void OnStartButtonClick()
    {
        if ((slider.value >= minRange) && (slider.value <= maxRange))
        {
            StopAllCoroutines();

            if (!taskCompleted)
            {
               TaskCompletedRoutine();
            }

        }
        else
        {
            hideButton.GetComponent<HideButton>().stoptimer = true;
            hideButton.GetComponent<HideButton>().IncompleteTask();
            StartCoroutine(GameLoseRoutine());
        }
        StopAllCoroutines();
    }

    IEnumerator GameLoseRoutine()
    {
        yield return new WaitForSeconds(2);
        LevelManager.Instance.GameLose();
    }

    void TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;
        Debug.LogError("nc");
        animator.enabled = true;
        Debug.LogError("nb");
        Invoke("completetask", 2.5f);
       
       

       

        
    }
   void completetask()
    {
        Debug.LogError("n");
        taskCompleted = true;
        taskCompletedObject.SetActive(true);
        Invoke("WinGame", 1.5f);
    }
    void WinGame()
    {
        LevelManager.Instance.GameWin();
    }
}