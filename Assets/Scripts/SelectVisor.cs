using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectVisor : MonoBehaviour, IPointerDownHandler
{
    public int number;

    private void OnEnable()
    {
      
      
     
    }
    public void UnlockVisor()
    {
        PlayerPrefs.SetInt("Visor" + number, 1);
        VisorLocking.ins.CheckIfUnlocked();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(CharacterAnimationController.Instance.photonView.IsMine)
        {
                SetColorManager.Instance.SetVisor(gameObject.transform.GetChild(0).gameObject);
        }
        
        //Debug.Log(eventData.pointerEnter.gameObject.name);
    }
    public void SetData()
    {
        if (CharacterAnimationController.Instance.photonView.IsMine)
        {
            SetColorManager.Instance.SetVisor(gameObject.transform.GetChild(0).gameObject);
        }
    }
}
