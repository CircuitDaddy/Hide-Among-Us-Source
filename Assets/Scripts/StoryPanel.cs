using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel : MonoBehaviour
{
    public TW_MultiStrings_All storytext;
    public GameObject skipBtn,closeBtn;
    private void OnEnable()
    {
        if(!PlayerPrefs.HasKey("SeenStory"))
        {
            PlayerPrefs.SetInt("SeenStory", 0);
        }
        if(PlayerPrefs.GetInt("SeenStory") == 0)
        {
            storytext.StartTypewriter();
            skipBtn.SetActive(true);
            closeBtn.SetActive(false);
            PlayerPrefs.SetInt("SeenStory", 1);
        }
        else
        {
            storytext.SkipTypewriter();
            skipBtn.SetActive(false);
            closeBtn.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
