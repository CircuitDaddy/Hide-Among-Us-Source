using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateImage : MonoBehaviour,  IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.Rotate(0, 0, 90f);
        if (transform.localRotation.eulerAngles.z==180)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        ConnectWireManager.Instance.CheckWire();
    }
}
