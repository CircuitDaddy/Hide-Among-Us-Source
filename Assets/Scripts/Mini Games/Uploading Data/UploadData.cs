using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class UploadData : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Vector2 initialPos;
    Vector2 startPos, endPos, direction;
    Rigidbody2D myRigidbody2D;

    [SerializeField] float shootPower = 5f;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetInitial()
    {
        transform.localPosition = new Vector3(initialPos.x, initialPos.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointer down");
        startPos = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPos = Input.mousePosition;
        direction = startPos - endPos;
        myRigidbody2D.isKinematic = false;
            myRigidbody2D.AddForce(-direction * shootPower);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Data"))
        {
            if (collision.gameObject.name == "Cloud Computer")
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.GetComponent<UploadData>().SetInitial();
                UploadingDataManager.Instance.CheckCount();
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.GetComponent<UploadData>().SetInitial();
            }
        }
    }
}
