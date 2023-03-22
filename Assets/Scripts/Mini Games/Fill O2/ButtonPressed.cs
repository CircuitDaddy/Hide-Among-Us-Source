using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        FillO2Manager.Instance.ButtonPressed();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        FillO2Manager.Instance.ButtonUnPressed();
    }
}
