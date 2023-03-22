using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewTheChairManager : MonoBehaviour
{
    public static ScrewTheChairManager Instance;

    [SerializeField] GameObject[] screws;

    int screwCount = 0;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    bool once = true;

    [SerializeField] GameObject hideButton;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        Instance = this;
        screwCount = 0;
        taskCompleted = false;
        taskCompletedObject.SetActive(false);
        StartCoroutine(SetInitialRoutine());
    }

    IEnumerator SetInitialRoutine()
    {
        yield return new WaitForSeconds(0.001f);
        for (int i = 0; i < screws.Length; i++)
        {
            screws[i].GetComponent<ScrewGear>().SetInitial(screws[i]);
        }
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
    public void CheckScrew()
    {
        screwCount++;
        if (screwCount==screws.Length)
        {
            StartCoroutine(TaskCompletedRoutine());
        }
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;

        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);
        LevelManager.Instance.GameWin();
        taskCompletedObject.SetActive(false);
    }
}
