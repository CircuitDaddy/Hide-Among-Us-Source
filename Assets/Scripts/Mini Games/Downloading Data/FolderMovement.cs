using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FolderMovement : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] Vector2 initialPos;
    private void Awake()
    {
        initialPos = transform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    public void SetInitial(GameObject gameObject)
    {
        transform.localPosition = new Vector3(initialPos.x, initialPos.y);
    }
    private void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, initialPos.y);
    }
    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        screenPoint.z = 10.0f;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        if ((transform.localPosition.x > 645f) || (transform.localPosition.x < -645f))
        {
            if ((transform.localPosition.x > 645f))
            {
                transform.localPosition = new Vector2(645f, transform.localPosition.y);
            }
            if ((transform.localPosition.x < -645f))
            {
                transform.localPosition = new Vector2(-645f, transform.localPosition.y);
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
                transform.localPosition = new Vector2(645f, transform.localPosition.y);
            }
            if ((transform.localPosition.x < -750f))
            {
                transform.localPosition = new Vector2(-645f, transform.localPosition.y);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Data"))
        {
            Destroy(collision.gameObject);
            DownloadDataManager.Instance.CheckData();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            LevelManager.Instance.GameLose();
        }
    }
}
