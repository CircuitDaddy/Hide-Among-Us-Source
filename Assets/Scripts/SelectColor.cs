using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectColor : MonoBehaviour, IPointerDownHandler
{
    public string colorName;
    public void OnPointerDown(PointerEventData eventData)
    {

        if (CharacterAnimationController.Instance.photonView.IsMine)
        {
            
            SetColorManager.Instance.SetColor(gameObject);
        }
        //Debug.Log(eventData.pointerEnter.gameObject.name);
    }
    public void SetNewColor()
    {
        if (CharacterAnimationController.Instance.photonView.IsMine)
        {

            SetColorManager.Instance.SetColor(gameObject);
        }
    }
}
