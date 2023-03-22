using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTheInsectsManager : MonoBehaviour
{
    public static KillTheInsectsManager Instance;

    [SerializeField] GameObject[] insects;

    int insectCount = 0;

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
        insectCount = 0;
        Instance = this;
        StartCoroutine(SetInitialRoutine());
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
    IEnumerator SetInitialRoutine()
    {
        yield return new WaitForSeconds(0.001f);
        for (int i = 0; i < insects.Length; i++)
        {
            insects[i].GetComponent<KillInsect>().SetInitial(insects[i]);
        }
    }

    public void KillInsect()
    {
        insectCount++;
        if (insectCount == insects.Length)
        {
            StartCoroutine(TaskCompletedRoutine());
        }
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;
        taskCompleted = true;

        yield return new WaitForSeconds(1);

        taskCompletedObject.SetActive(true);
        LevelManager.Instance.GameWin();
        taskCompletedObject.SetActive(false);
    }
}
