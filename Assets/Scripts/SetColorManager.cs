using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SetColorManager : MonoBehaviourPunCallbacks
{
    public static SetColorManager Instance;

    [SerializeField] GameObject[] visorImages;
    [SerializeField] GameObject visorHolder;


    [SerializeField] Color[] colors;
    [SerializeField] GameObject[] colorss;
    [SerializeField] GameObject characterImage;

    [SerializeField] GameObject temp;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
    
        for (int i = 0; i < colorss.Length; i++)
        {
           
            if (colorss[i].GetComponent<SelectColor>().colorName == PlayerPrefs.GetString("CurrentColor"))
            {
                colorss[i].GetComponent<SelectColor>().SetNewColor();
            }
        }

        int currentVisorNum = PlayerPrefs.GetInt("SelectedVisor");

        visorImages[currentVisorNum - 1].transform.parent.GetComponent<SelectVisor>().SetData();

        
       
    }

    public void SetVisor(GameObject visor)
    {
        for (int i = 0; i < visorImages.Length; i++)
        {
            if (visor.gameObject == visorImages[i].gameObject)
            {
                visorImages[i].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                temp = visorImages[i];
                characterImage.transform.GetChild(0).GetComponent<Image>().sprite = visorImages[i].GetComponent<Image>().sprite;
                SendVisor(visorImages[i].gameObject);
            }
            else
            {
                visorImages[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void SendVisor(GameObject visor)
    {
        CharacterAnimationController.Instance.SetVisor(visor.transform.parent.GetComponent<SelectVisor>().number);
       
    }

    public void SetColor(GameObject color)
    {
        for (int i=0; i<colorss.Length; i++)
        {
            if (color.gameObject==colorss[i].gameObject)
            {
                colorss[i].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                SendColor(colorss[i].gameObject);
            }
            else
            {
                colorss[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void SendColor(GameObject color)
    {
        switch (color.GetComponent<SelectColor>().colorName)
        {
            case "Blue":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[0].r,colors[0].g, colors[0].b, 1);
                for (int i=0; i< visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color= new Color(colors[0].r, colors[0].g, colors[0].b, 1);
                }
                break;

            case "Red":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[1].r, colors[1].g, colors[1].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[1].r, colors[1].g, colors[1].b, 1);
                }
                break;

            case "Yellow":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[2].r, colors[2].g, colors[2].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[2].r, colors[2].g, colors[2].b, 1);
                }
                break;

            case "Green":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[3].r, colors[3].g, colors[3].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[3].r, colors[3].g, colors[3].b, 1);
                }
                break;

            case "Pink":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[4].r, colors[4].g, colors[4].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[4].r, colors[4].g, colors[4].b, 1);
                }
                break;

            case "Purple":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[5].r, colors[5].g, colors[5].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[5].r, colors[5].g, colors[5].b, 1);
                }
                break;

            case "Orange":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[6].r, colors[6].g, colors[6].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[6].r, colors[6].g, colors[6].b, 1);
                }
                break;

            case "Turquoise":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[7].r, colors[7].g, colors[7].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[7].r, colors[7].g, colors[7].b, 1);
                }
                break;

            case "Gray":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[8].r, colors[8].g, colors[8].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[8].r, colors[8].g, colors[8].b, 1);
                }
                break;

            case "Dark Gray":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[9].r, colors[9].g, colors[9].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[9].r, colors[9].g, colors[9].b, 1);
                }
                break;

            case "Dark Green":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[10].r, colors[10].g, colors[10].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[10].r, colors[10].g, colors[10].b, 1);
                }
                break;

            case "Dark Blue":
                characterImage.gameObject.GetComponent<Image>().color = new Color(colors[11].r, colors[11].g, colors[11].b, 1);
                for (int i = 0; i < visorHolder.transform.childCount; i++)
                {
                    visorHolder.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(colors[11].r, colors[11].g, colors[11].b, 1);
                }
                break;
        }
        CharacterAnimationController.Instance.SetColor(color.GetComponent<SelectColor>().colorName);

        
    }
}
