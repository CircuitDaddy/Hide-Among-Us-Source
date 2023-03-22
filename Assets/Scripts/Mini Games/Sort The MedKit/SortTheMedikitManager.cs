using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortTheMedikitManager : MonoBehaviour
{
    public static SortTheMedikitManager Instance;

    [SerializeField] GameObject[] items;

    [SerializeField] GameObject hideButton;

    int radomRotation=0;

    int itemCount = 0;
    bool win = false;

    bool once = true;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        itemCount = 0;
        Instance = this;
        taskCompleted = false;
        once = true;
        taskCompletedObject.SetActive(false);
        StartCoroutine(InitialPositionRoutine());
    }
    IEnumerator InitialPositionRoutine()
    {
        yield return new WaitForSeconds(0.01f);
        for (int i = 0; i < items.Length; i++)
        {
            radomRotation = Random.Range(90, 361);
            items[i].transform.localRotation = Quaternion.Euler(0, 0, radomRotation);
            items[i].GetComponent<MoveItems>().SetInitial(items[i]);
        }
    }

    private void Update()
    {
        if ((hideButton.GetComponent<HideButton>().timeCount<=0) && win == false)
        {
            if (once==true)
            {
                once = false;
                hideButton.GetComponent<HideButton>().IncompleteTask();
            }
        }
    }

    public void CheckItems()
    {
        itemCount++;
        if (itemCount == items.Length)
        {
            win = true;
        }
        else
        {
            win = false;
        }
        if (win)
        {
            hideButton.GetComponent<HideButton>().stoptimer = true;
            StartCoroutine(TaskCompletedRoutine());
        }
    }

    IEnumerator TaskCompletedRoutine()
    {
        yield return new WaitForSeconds(1);
        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);
        LevelManager.Instance.GameWin();
        taskCompletedObject.SetActive(false);
    }
}
