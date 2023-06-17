using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SelectVisor : MonoBehaviour, IPointerDownHandler
{
    public int number;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("com.petersords.hidingamongussource.visor" + number))
        {
            PlayerPrefs.SetString("Visor" + number, "com.petersords.hidingamongussource.visor" + number);
        }
    }
    private void OnEnable()
    {
      
    }
    public void UnlockVisor()
    {
        IAPPlugin.Instance.BuyProductID("Visor" + number);
        IAPPlugin.Instance.OnSuccessfulPurchase.RemoveAllListeners();
        IAPPlugin.Instance.OnSuccessfulPurchase.AddListener(delegate {
            PlayerPrefs.SetInt("Visor" + number, 1);
            VisorLocking.ins.CheckIfUnlocked();
        });
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
