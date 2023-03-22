using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisorLocking : MonoBehaviour
{
    public static VisorLocking ins;


    [SerializeField] List<GameObject> visors;


    private void Awake()
    {
        
            ins = this;
       
    }
    private void OnEnable()
    {
        PlayerPrefs.SetInt("Visor1", 1);
        for (int i=0; i<visors.Count; i++)
        {
            
            if (!PlayerPrefs.HasKey("Visor" + (i+1)))
            {
                PlayerPrefs.SetInt("Visor" + (i+1), 0);
            }
            if (PlayerPrefs.GetInt("Visor"+(i+1)) == 1)
            {
                visors[i].transform.GetChild(1).gameObject.SetActive(false);
            }
          
        }
    }
  public void CheckIfUnlocked()
    {
        for (int i = 0; i < visors.Count; i++)
        {
            if (PlayerPrefs.GetInt("Visor" + (i + 1)) == 1)
            {
                visors[i].transform.GetChild(1).gameObject.SetActive(false);
            }

        }
    }
}
