using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowThePattern : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] reference;
    [SerializeField] GameObject[] playerTurn;
    [SerializeField] float waitForSeconds;

    [SerializeField] Sprite buttonIdleSprite;
    [SerializeField] Sprite buttonGlowSprite;

    [SerializeField] GameObject correctObject;
    [SerializeField] GameObject inCorrectObject;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    bool once = true;

    [SerializeField] GameObject hideButton;

    int buttonCount = 0;
    int temp=0;
    private void Awake()
    {
        DisableButtons();
    }
    void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        correctObject.SetActive(false);
        inCorrectObject.SetActive(false);

        taskCompleted = false;
        taskCompletedObject.SetActive(false);

        for (int i = 0; i < reference.Length; i++)
        {
            reference[i] = null;
            playerTurn[i] = null;
        }
        buttonCount = 0;
        DisableButtons();
        StopAllCoroutines();
        StartCoroutine(PressButtonRoutine());
    }


    IEnumerator PressButtonRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        int random = Random.Range(0, buttons.Length);
        if (temp==random)
        {
            random = Random.Range(0, buttons.Length);
        }
        reference[buttonCount] = buttons[random].gameObject;
        buttons[random].GetComponent<Image>().sprite = buttonGlowSprite;
        yield return new WaitForSeconds(waitForSeconds);
        buttons[random].GetComponent<Image>().sprite = buttonIdleSprite;
        temp = random;

        if (buttonCount<3)
        {
            buttonCount++;
            StartCoroutine(PressButtonRoutine());
        }
        else
        {
            buttonCount = 0;
            EnableButtons();
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
    void DisableButtons()
    {
        for (int i=0; i<buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().interactable = false;
        }
    }

    void EnableButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().interactable = true;
        }
    }

    public void PlayerInput(GameObject gameObject)
    {
        if (gameObject == reference[buttonCount])
        {
            playerTurn[buttonCount] = gameObject;
            buttonCount++;
            if (buttonCount == 4)
            {
                DisableButtons();
                if (!taskCompleted)
                {
                    StartCoroutine("TaskCompletedRoutine");
                }
            }
        }
        else
        {
            for (int i = 0; i < reference.Length; i++)
            {
                reference[i] = null;
                playerTurn[i] = null;
            }
            buttonCount = 0;
            DisableButtons();

            inCorrectObject.SetActive(true);
            hideButton.GetComponent<HideButton>().stoptimer = true;
            hideButton.GetComponent<HideButton>().IncompleteTask();
            StartCoroutine(GameLoseRoutine());
        }
    }

    IEnumerator GameLoseRoutine()
    {
        yield return new WaitForSeconds(2);
        LevelManager.Instance.GameLose();
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;
        correctObject.SetActive(true);

        yield return new WaitForSeconds(2);

        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);

        LevelManager.Instance.GameWin();
    }
}
