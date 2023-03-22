using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestTheCropManager : MonoBehaviour
{
    public static HarvestTheCropManager Instance;

    [SerializeField] GameObject[] crops;

    int cropCount=0;

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
        cropCount = 0;
        Instance = this;
        StartCoroutine(SetInitialRoutine());
    }

    IEnumerator SetInitialRoutine()
    {
        yield return new WaitForSeconds(0.001f);
        for (int i = 0; i < crops.Length; i++)
        {
            crops[i].GetComponent<CropHarvest>().SetInitial(crops[i]);
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

    public void CheckCropCount()
    {
        cropCount++;
        if (cropCount==crops.Length)
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
