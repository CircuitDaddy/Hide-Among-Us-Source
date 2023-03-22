using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignalStablizerManager : MonoBehaviour
{
    public static SignalStablizerManager Instance;

    [SerializeField] GameObject waveToFix;
    [SerializeField] Slider slider;

    float offset;

    [SerializeField] float dist;
    [SerializeField] float winTime;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    [SerializeField] GameObject hideButton;

    float winingPoint;
    float minRange;
    float maxRange;
    float lastPos;
    float temp = 0;
    float temp2 = 0;
    float temp3 = 0;

    bool moving = false;


    bool once = true;

    private void OnEnable()
    {
        moving = false;
        Instance = this;
        once = true;
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        taskCompleted = false;
        taskCompletedObject.SetActive(false);

        //slider.value = slider.maxValue / 2;
        lastPos = slider.value;
        winingPoint = Random.Range(2f, slider.maxValue-1f);
        winingPoint= Mathf.Round(winingPoint * 10.0f) * 0.1f;
        dist = winingPoint - slider.value;
        if (dist <= 1.5f)
        {
            winingPoint = Random.Range(2f, slider.maxValue - 1f);
            winingPoint = Mathf.Round(winingPoint * 10.0f) * 0.1f;
        }
        temp = dist;
        if (winingPoint >= 10)
        {
            winingPoint = 10f;
            minRange = winingPoint - 1f;
            maxRange = 10f;
        }
        else
        {
            minRange = winingPoint - 2f;
            maxRange = winingPoint + 2f;
        }

        //waveToFix.GetComponent<SignalSpeed>().speed = Random.Range(2, 6);
        temp2 = waveToFix.GetComponent<SignalSpeed>().speed;
        offset = waveToFix.GetComponent<SignalSpeed>().speed / slider.maxValue;
        //offset = offset / 10f;
        waveToFix.GetComponent<Image>().material.mainTextureScale = new Vector2(Random.Range(3, 4f), 1);
        temp3 = waveToFix.GetComponent<Image>().material.mainTextureScale.x;

        //Debug.Log((int)winingPoint);
    }

    IEnumerator Start()
    {
        while(true)
        {
            dist = winingPoint - slider.value;
            if (dist <= 1.5f)
            {
                winingPoint = Random.Range(5, slider.maxValue - 1f);
                winingPoint = Mathf.Round(winingPoint * 10.0f) * 0.1f;
            }
            else
            {
                break;
            }
            temp = dist;
            yield return null;
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

    public void WiningCondition()
    {
        if (((int)slider.value == (int)winingPoint))
        {
            slider.value = winingPoint;
            // waveToFix.GetComponent<SignalSpeed>().speed = 1f;
            waveToFix.GetComponent<Image>().material.mainTextureScale = new Vector2(1, 1);
            StartCoroutine(CheckWin());
        }
        else
        {
            //StartCoroutine(MoveSliderToCenterRoutine());
        }
    }

    IEnumerator MoveSliderToCenterRoutine()
    {
        if (slider.value>slider.maxValue/2)
        {
            while ((int)slider.value != (int)slider.maxValue / 2 -1)
            {
                slider.value = slider.value - 0.1f;
                yield return new WaitForSeconds(0.01f);
                if (slider.value == slider.maxValue)
                {
                    break;
                }
            }
        }
        else
        {
            while ((int)slider.value != (int)slider.maxValue / 2)
            {
                slider.value = slider.value + 0.1f;
                yield return new WaitForSeconds(0.01f);
                if (slider.value == slider.maxValue)
                {
                    break;
                }
            }
        }
        yield return null;
    }


    public void OnSliderValueChange()
    {
        if (slider.value==slider.minValue)
        {
            //waveToFix.GetComponent<SignalSpeed>().speed = temp2;
            waveToFix.GetComponent<Image>().material.mainTextureScale = new Vector2(temp3/0.5f, 1);

        }
        else if (slider.value == slider.maxValue)
        {
            waveToFix.GetComponent<Image>().material.mainTextureScale = new Vector2(temp3/1.5f, 1);
           // waveToFix.GetComponent<SignalSpeed>().speed = temp2;
        }
        else
        {
            StopAllCoroutines();
           // StopCoroutine(CheckWin());
            if (slider.value > lastPos)
            {
                dist = winingPoint - slider.value;
                lastPos = slider.value;
            }
            else
            {
                dist = -winingPoint + slider.value;
                lastPos = slider.value;
            }
            //waveToFix.GetComponent<SignalSpeed>().speed = Mathf.Clamp(waveToFix.GetComponent<SignalSpeed>().speed, 1, 6);
            waveToFix.GetComponent<Image>().material.mainTextureScale = new Vector2(Mathf.Clamp(waveToFix.GetComponent<Image>().material.mainTextureScale.x, 1, 5), 1);
            dist = Mathf.Abs(dist);
            if (dist<temp)
            {
                    //waveToFix.GetComponent<SignalSpeed>().speed -= offset;
                    waveToFix.GetComponent<Image>().material.mainTextureScale = new Vector2(waveToFix.GetComponent<Image>().material.mainTextureScale.x - (offset/5f), 1);
            }
            else
            {
                waveToFix.GetComponent<Image>().material.mainTextureScale = new Vector2(waveToFix.GetComponent<Image>().material.mainTextureScale.x + (offset/5f), 1);
                //waveToFix.GetComponent<SignalSpeed>().speed += offset;
            }

            temp = dist;
        }
    }
    IEnumerator CheckWin()
    {
        yield return new WaitForSeconds(winTime);
        StartCoroutine("TaskCompletedRoutine");
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;

        yield return new WaitForSeconds(2);

        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);

        LevelManager.Instance.GameWin();
    }
}
