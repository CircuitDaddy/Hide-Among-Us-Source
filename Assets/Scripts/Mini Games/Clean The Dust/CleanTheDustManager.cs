using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanTheDustManager : MonoBehaviour
{
    public static CleanTheDustManager Instance;

    [SerializeField] GameObject[] dustParticle;
    [SerializeField] GameObject[] swipeableImages;
    [SerializeField] GameObject hideButton;

    [SerializeField] int dustCount = 0;

    bool once = true;
    bool win = false;
    public bool done = false;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    private void OnEnable()
    {
        done = false;
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        Instance = this;
        dustCount = 0;
        once = true;
        win = false;
        taskCompleted = false;
        taskCompletedObject.SetActive(false);
        StartCoroutine(InitialPositionRoutine());
    }

    IEnumerator InitialPositionRoutine()
    {
        yield return new WaitForSeconds(0.01f);

        for (int i = 0; i < dustParticle.Length; i++)
        {
            dustParticle[i].GetComponent<DragAndShootCotton>().SetInitial(dustParticle[i]);
        }

        for (int i = 0; i < swipeableImages.Length; i++)
        {
            swipeableImages[i].GetComponent<SwipeToClean>().SetInitial(swipeableImages[i]);
        }
    }

    private void Update()
    {
        if (swipeableImages[0].GetComponent<Image>().color.a==0)
        {
            done = true;
        }
        if (((done) && (dustCount == dustParticle.Length))&&(once))
        {
            once = false;
            win = true;
            StartCoroutine(TaskCompletedRoutine());
        }
        else if ((hideButton.GetComponent<HideButton>().timeCount <= 0) && win == false)
        {
            if (once == true)
            {
                once = false;
                hideButton.GetComponent<HideButton>().IncompleteTask();
            }
        }
    }

    public void CheckDust()
    {
        dustCount++;
        if (dustCount==dustParticle.Length)
        {
            for (int i=0; i<swipeableImages.Length; i++)
            {
                swipeableImages[i].GetComponent<Image>().raycastTarget = true;
                swipeableImages[i].GetComponent<Image>().maskable = true;
            }
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
