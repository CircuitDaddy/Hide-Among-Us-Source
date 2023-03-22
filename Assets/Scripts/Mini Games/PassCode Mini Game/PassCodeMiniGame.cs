using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassCodeMiniGame : MonoBehaviour
{
    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    [SerializeField] Text noteText;

    [SerializeField] GameObject[] numpadButton;
    [SerializeField] string correctAnswer;
    [SerializeField] Text displayText;

    [SerializeField] GameObject correct;
    [SerializeField] GameObject incorrect;
    bool win = false;


    bool once = true;

    [SerializeField] GameObject hideButton;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;

        incorrect.SetActive(false);
        correct.SetActive(false);
        taskCompleted = false;
        int rand = Random.Range(1000, 9999);
        noteText.text = rand.ToString();
        correctAnswer = rand.ToString();
        for (int i = 0; i < numpadButton.Length; i++)
        {
            numpadButton[i].gameObject.GetComponent<Button>().interactable = true;
        }
        taskCompletedObject.SetActive(false);
        displayText.text = null;
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

    IEnumerator CheckingRoutine()
    {
        if (displayText.text.Length == 0)
        {
            win = false;

            if (win)
            {
                for (int i = 0; i < numpadButton.Length; i++)
                {
                    numpadButton[i].gameObject.GetComponent<Button>().interactable = false;
                }
                displayText.text = null;
                correct.SetActive(true);
                incorrect.SetActive(false);
                yield return new WaitForSeconds(0.5f);

                StartCoroutine("TaskCompletedRoutine");
            }
            else
            {
                displayText.text = null;
                incorrect.SetActive(true);
                correct.SetActive(false);
                hideButton.GetComponent<HideButton>().stoptimer = true;
                hideButton.GetComponent<HideButton>().IncompleteTask();

                yield return new WaitForSeconds(1.5f);
                LevelManager.Instance.GameLose();
            }
        }
        else
        {
            for (int i = 0; i < correctAnswer.Length; i++)
            {
                if (displayText.text[i] == correctAnswer[i])
                {
                    win = true;
                }
                else
                {
                    win = false;
                    break;
                }
            }
            if (win)
            {
                for (int i = 0; i < numpadButton.Length; i++)
                {
                    numpadButton[i].gameObject.GetComponent<Button>().interactable = false;
                }
                displayText.text = null;
                correct.SetActive(true);
                incorrect.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                StartCoroutine("TaskCompletedRoutine");
            }
            else
            {
                displayText.text = null;
                incorrect.SetActive(true);
                correct.SetActive(false);
                hideButton.GetComponent<HideButton>().stoptimer = true;
                hideButton.GetComponent<HideButton>().IncompleteTask();
                yield return new WaitForSeconds(1.5f);
                LevelManager.Instance.GameLose();
            }
        }
    }

    public void AddNumber(int number)
    {
       displayText.text += number;
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;

        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);

        LevelManager.Instance.GameWin();
    }

    public void BackSpaceClick()
    {
        if (displayText.text.Length != 0)
        {
            displayText.text = displayText.text.ToString().Remove(displayText.text.Length - 1);
        }
    }

    public void OnEnterClick()
    {
        StartCoroutine(CheckingRoutine());
    }
}
