using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FileMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] Vector2 initialPos;

    public GameObject snapPos;

    private void Awake()
    {
        initialPos = transform.localPosition;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (snapPos != null)
        {
            transform.localPosition = new Vector3(snapPos.transform.parent.gameObject.transform.localPosition.x, snapPos.transform.parent.gameObject.transform.localPosition.y);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<FileMovement>().enabled = false;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<SnapPoint>().positionNumber==1)
            {
                SortTheFileManager.Instance.ActiveGreenShelf();
            }
            else if (gameObject.GetComponent<SnapPoint>().positionNumber == 2)
            {
                SortTheFileManager.Instance.ActiveBlueShelf();
            }
            else
            {
                SortTheFileManager.Instance.ActiveGoldenShelf();
            }

            SortTheFileManager.Instance.CheckFiles();
        }
        else
        {
            transform.localPosition = new Vector3(initialPos.x, initialPos.y);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    public void SetInitial(GameObject gameObject)
    {
        transform.localPosition = new Vector3(initialPos.x, initialPos.y);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<FileMovement>().enabled = true;
        gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        screenPoint.z = 10.0f;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        if ((transform.localPosition.x > 450f) || (transform.localPosition.x < -450f))
        {
            if ((transform.localPosition.x > 450f))
            {
                transform.localPosition = new Vector2(450f, transform.localPosition.y);
            }
            if ((transform.localPosition.x < -450f))
            {
                transform.localPosition = new Vector2(-450f, transform.localPosition.y);
            }
            if ((transform.localPosition.y > 350f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, 350f);
            }
            if ((transform.localPosition.y < -350f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -350f);
            }

        }
        else if ((transform.localPosition.y > 350f) || (transform.localPosition.y < -350f))
        {
            if ((transform.localPosition.y > 350f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, 350f);
            }
            if ((transform.localPosition.y < -350f))
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -350f);
            }
            if ((transform.localPosition.x > 450f))
            {
                transform.localPosition = new Vector2(450f, transform.localPosition.y);
            }
            if ((transform.localPosition.x < -450f))
            {
                transform.localPosition = new Vector2(-450f, transform.localPosition.y);
            }
        }
    }
}
