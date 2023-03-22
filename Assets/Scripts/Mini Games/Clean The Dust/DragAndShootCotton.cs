using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndShootCotton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Vector2 initialPos;

    Vector2 startPos, endPos, direction;
    Rigidbody2D myRigidbody2D;
    public float shootPower = 5f;
    private void Awake()
    {
        initialPos = transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            direction = startPos - endPos;
            myRigidbody2D.isKinematic = false;
            myRigidbody2D.gravityScale = 0;
            myRigidbody2D.AddForce(-direction * shootPower);
        }
    }

    public void SetInitial(GameObject gameObject)
    {
        gameObject.transform.localPosition = new Vector3(initialPos.x, initialPos.y);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            CleanTheDustManager.Instance.CheckDust();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
