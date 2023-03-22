using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    public static RadarManager Instance;

    [SerializeField] GameObject radar;

    [SerializeField] float minLimitX;
    [SerializeField] float maxLimitX;
    [SerializeField] float minLimitY;
    [SerializeField] float maxLimitY;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;


    bool once = true;

    [SerializeField] GameObject hideButton;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        radar.GetComponent<RadarMovement>().once = true;

        Instance = this;

        taskCompleted = false;
        taskCompletedObject.SetActive(false);

        float xPos = Random.Range(minLimitX, maxLimitX);
        float yPos = Random.Range(minLimitY, maxLimitY);
        if (((xPos == 0) || (yPos == 0)))
        {
            while ((xPos != 0) || (yPos != 0))
            {
                xPos = Random.Range(minLimitX, maxLimitX);
                yPos = Random.Range(minLimitY, maxLimitY);
            }
        }
        radar.gameObject.transform.localPosition = new Vector3(xPos, yPos);
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

    public void TaskComplete()
    {
        StartCoroutine(TaskCompletedRoutine());
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
