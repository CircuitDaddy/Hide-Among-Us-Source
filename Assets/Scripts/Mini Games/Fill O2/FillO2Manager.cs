using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillO2Manager : MonoBehaviour
{
    public static FillO2Manager Instance;

    bool once = true;

    [SerializeField] GameObject hideButton;
    [SerializeField] float waitForSeconds;
    [SerializeField] Slider slider;
    [SerializeField] Slider slider2;
    [SerializeField] GameObject tapPoint;
    [SerializeField] int minRange;
    [SerializeField] int maxRange;
    [SerializeField] int rangeoffset;

    [SerializeField] GameObject fill;

    [SerializeField] ButtonPressed buttonPress;

    [SerializeField] int tapPointPos;


    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;


    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        Instance = this;
        taskCompleted = false;
        once = true;
        taskCompletedObject.SetActive(false);
        if (tapPointPos < 0)
        {
            minRange = tapPointPos - rangeoffset;
            maxRange = tapPointPos + rangeoffset;
        }
        else
        {
            minRange = tapPointPos - rangeoffset;
            maxRange = tapPointPos + rangeoffset;
        }

        StopAllCoroutines();
        slider.value = slider.minValue;
        slider2.value = slider2.minValue;
    }
    public void ButtonUnPressed()
    {
        StopAllCoroutines();
        StartCoroutine(DescreaseSliderValueRoutine());
    }

    IEnumerator DescreaseSliderValueRoutine()
    {
            while ((slider.value != slider.minValue)&& buttonPress.buttonPressed == false)
            {

                slider.value = slider.value - 8f;
                yield return new WaitForSeconds(waitForSeconds);
                if (slider.value == slider.minValue)
                {
                    break;
                }

            if ((slider.value >= minRange) && (slider.value <= maxRange))
            {
                slider2.value = slider2.value + 5f;
                if (slider2.value == slider2.maxValue)
                {
                    StartCoroutine("TaskCompletedRoutine");
                }
                yield return new WaitForSeconds(waitForSeconds);
            }
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

    public void ButtonPressed()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSliderValueRoutine());
    }

    IEnumerator ChangeSliderValueRoutine()
    {
            while ((slider.value != slider.maxValue) && buttonPress.buttonPressed == true)
            {
                slider.value = slider.value + 8f;
                yield return new WaitForSeconds(waitForSeconds);
                if (slider.value == slider.maxValue)
                {
                    break;
                }
                if ((slider.value >= minRange) && (slider.value <= maxRange))
                {
                    slider2.value = slider2.value + 5f;
                    if (slider2.value==slider2.maxValue)
                    {
                        StartCoroutine("TaskCompletedRoutine");
                    }
                    yield return new WaitForSeconds(waitForSeconds);
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
    }
}
