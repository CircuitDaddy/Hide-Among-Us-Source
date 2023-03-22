using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortTheFileManager : MonoBehaviour
{
    public static SortTheFileManager Instance;

    [SerializeField] GameObject[] files;

    [SerializeField] GameObject[] greenShelf;
    [SerializeField] GameObject[] blueShelf;
    [SerializeField] GameObject[] goldenShelf;


    [SerializeField] bool win = false;

    int fileCount = 0;
    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;


    bool once = true;

    [SerializeField] GameObject hideButton;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        for (int i=0; i<goldenShelf.Length; i++)
        {
            goldenShelf[i].SetActive(true);
            blueShelf[i].SetActive(true);
            greenShelf[i].SetActive(true);
        }
        fileCount = 0;
        taskCompleted = false;
        taskCompletedObject.SetActive(false);
        int rand = Random.Range(0, goldenShelf.Length);
        goldenShelf[rand].SetActive(false);
        rand = Random.Range(0, goldenShelf.Length);
        blueShelf[rand].SetActive(false);
        rand = Random.Range(0, goldenShelf.Length);
        greenShelf[rand].SetActive(false);

        StartCoroutine(InitialPositionRoutine());
    }

    public void ActiveBlueShelf()
    {
        for (int i = 0; i < goldenShelf.Length; i++)
        {
            blueShelf[i].SetActive(true);
        }
    }

    public void ActiveGreenShelf()
    {
        for (int i = 0; i < goldenShelf.Length; i++)
        {
            greenShelf[i].SetActive(true);
        }
    }

    public void ActiveGoldenShelf()
    {
        for (int i = 0; i < goldenShelf.Length; i++)
        {
            goldenShelf[i].SetActive(true);
        }
    }

    private void Awake()
    {
        Instance = this;
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

    public void CheckFiles()
    {
        fileCount++;
        if (fileCount==files.Length)
        {
            win = true;
        }
        else
        {
            win = false;
        }
        if (win)
        {
            StartCoroutine(TaskCompletedRoutine());
        }
    }
    IEnumerator InitialPositionRoutine()
    {
        yield return new WaitForSeconds(0.01f);
        for (int i=0; i<files.Length; i++)
        {
            files[i].GetComponent<FileMovement>().SetInitial(files[i]);
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
