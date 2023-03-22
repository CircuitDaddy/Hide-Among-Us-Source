﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveItems : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] Vector2 initialPos;

    public GameObject snapPos;
    private void Awake()
    {
        initialPos = transform.localPosition;
    }
    public void SetInitial(GameObject gameObject)
    {
        transform.localPosition = new Vector3(initialPos.x, initialPos.y);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<MoveItems>().enabled = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        screenPoint.z = 10.0f;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        if ((transform.localPosition.x > 805f) || (transform.localPosition.x < -750f))
        {
            if ((transform.localPosition.x > 805f))
            {
                transform.localPosition = new Vector2(805, transform.localPosition.y);
            }
            if ((transform.localPosition.x < -750f))
            {
                transform.localPosition = new Vector2(-750f, transform.localPosition.y);
            }
            if ((transform.localPosition.y > 450f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, 450f);
            }
            if ((transform.localPosition.y < -450f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -450f);
            }

        }
        else if ((transform.localPosition.y > 450f) || (transform.localPosition.y < -450f))
        {
            if ((transform.localPosition.y > 450f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, 450f);
            }
            if ((transform.localPosition.y < -450f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -450f);
            }
            if ((transform.localPosition.x > 805f))
            {
                transform.localPosition = new Vector2(805, transform.localPosition.y);
            }
            if ((transform.localPosition.x < -750f))
            {
                transform.localPosition = new Vector2(-750f, transform.localPosition.y);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (snapPos != null)
        {
            transform.localPosition = new Vector3(snapPos.transform.localPosition.x, snapPos.transform.localPosition.y);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<MoveItems>().enabled = false;

            SortTheMedikitManager.Instance.CheckItems();
        }
        else
        {
            transform.localPosition = new Vector3(initialPos.x, initialPos.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //
    }
}
