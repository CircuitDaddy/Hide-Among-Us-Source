using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class settingpanel : MonoBehaviour
{
    public Text audioState;
    public Slider VolumeSlider;
    int graphicNum;
    public Text GraphicText;
    private void OnEnable()
    {
        if(!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
    }
    private void Update()
    {
         
      
        SetGraphicText();
    }
    void SetGraphicText()
    {
        
       if(graphicNum == 0)
        {
            GraphicText.text = "Low";
        }
        if (graphicNum == 1)
        {
            GraphicText.text = "Medium";
        }
        if (graphicNum == 2)
        {
            GraphicText.text = "High";
        }
        if(graphicNum == 3)
        {
            GraphicText.text = "Ultra";
        }
    }
    public void TurnSoundOn()
    {
        AudioListener.volume = 1f;
        audioState.text = "ON";
        Debug.LogError("Working");
    }
    public void TurnSoundOff()
    {
        AudioListener.volume = 0f;
        audioState.text = "OFF";
    }
    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        SaveVolume();
    }
    void LoadVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", VolumeSlider.value);
    }
    public void IncGrapjic()
    {
        if(graphicNum >= 3)
        {
            graphicNum = 0;
            return;
        }
        graphicNum++;
    }
    public void DecGraphic()
    {
        if (graphicNum <= 0)
        {
            graphicNum = 3;
            return;
        }
        graphicNum--;
    }
}
