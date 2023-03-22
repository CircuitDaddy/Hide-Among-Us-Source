using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadingDataManager : MonoBehaviour
{
    public static UploadingDataManager Instance;

    [SerializeField] GameObject data;
 
    [SerializeField] int dataCount = 0;

    [SerializeField] int maxLimit=0;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    bool once = true;

    [SerializeField] GameObject hideButton;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        taskCompleted = false;
        taskCompletedObject.SetActive(false);
        dataCount = 0;
        Instance = this;
        StartCoroutine(SetInitialRoutine());
    }

    IEnumerator SetInitialRoutine()
    {
        yield return new WaitForSeconds(0.001f);
        data.GetComponent<UploadData>().SetInitial();
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
    public void CheckCount()
    {
        dataCount++;
        if (dataCount==maxLimit)
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
