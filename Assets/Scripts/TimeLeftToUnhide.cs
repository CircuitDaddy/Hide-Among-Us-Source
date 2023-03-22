using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeLeftToUnhide : MonoBehaviour
{
    float checkTime;
    TextMeshProUGUI myText;
    private void OnEnable()
    {
        checkTime = LevelManager.Instance.timeToHide - LevelManager.Instance.timeInvestedInGame;
        
    }
    // Start is called before the first frame update
    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();   
    }

    // Update is called once per frame
    void Update()
    {
        checkTime -= Time.deltaTime;
        DisplayTime(checkTime);
        if(checkTime <= 0)
        {
            LevelManager.Instance.shouldStop = false;
        }
      
    }
    void DisplayTime(float timeToDisplay)
    {
       // timeToDisplay += 1;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        myText.text = "Unhiding In: " + seconds.ToString();
    }
}
