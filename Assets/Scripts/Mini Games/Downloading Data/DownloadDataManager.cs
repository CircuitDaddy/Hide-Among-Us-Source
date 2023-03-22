using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadDataManager : MonoBehaviour
{
    public static DownloadDataManager Instance;

    [SerializeField] GameObject hideButton;

    bool once = true;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    [SerializeField] int DataCount=0;
    [SerializeField] float maxLimit;

    [SerializeField] float increameant;

    [SerializeField] Slider downloadSlider;
    [SerializeField] Text percenatge;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        downloadSlider.value = 0;
        percenatge.text = null;
        percenatge.text = "0%";
        increameant = 100 / maxLimit;
        Instance = this;
        DataCount = 0;
        taskCompleted = false;
        taskCompletedObject.SetActive(false);
    }

    public void CheckData()
    {
        downloadSlider.value += increameant;
        percenatge.text = null;
        percenatge.text=(int)downloadSlider.value + "%" .ToString();
        DataCount++;
        if (DataCount==maxLimit)
        {
            downloadSlider.value = downloadSlider.maxValue;
            DataSpanwer.Instance.DeleteParent();
            StartCoroutine(TaskCompletedRoutine());
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


    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;
        yield return new WaitForSeconds(1);

        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);
        LevelManager.Instance.GameWin();
        taskCompletedObject.SetActive(false);
    }
}
