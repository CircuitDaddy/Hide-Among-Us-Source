using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CropHarvest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    Vector2 lastMousePosition;
    [SerializeField] Vector2 initialPos;
    [SerializeField] bool drag = false;
    [SerializeField] bool insideBasket = false;
    bool pointerDown = false;

    int angle = 35;
    int angle2 = -35;

    bool once = true;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    private void Awake()
    {
        initialPos = transform.localPosition;
    }
    private void Update()
    {
        if ((transform.position.y<-6.5f) && (pointerDown == false))
        {
            if (gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {
                int ran = Random.Range(0, 2);
                if ((ran == 1) && (once))
                {
                    once = false;
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + angle);
                }
                else
                {
                    once = false;
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + angle2);
                }
            }
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            once = true;
        }
        if (drag && pointerDown)
        {
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            screenPoint.z = 10.0f;
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
            if ((transform.localPosition.x>675)||(transform.localPosition.x<-675f))
            {
                if ((transform.localPosition.x > 675f))
                {
                    transform.localPosition = new Vector2(675, transform.localPosition.y);
                }
                if ((transform.localPosition.x < -675f))
                {
                    transform.localPosition = new Vector2(-675f, transform.localPosition.y);
                }
                if ((transform.localPosition.y > 180))
                {
                    transform.localPosition = new Vector2(transform.localPosition.x, 180f);
                }
                if ((transform.localPosition.y < -210f))
                {
                    transform.localPosition = new Vector2(transform.localPosition.x, -210f);
                }

            }
            else if((transform.localPosition.y > 180) || (transform.localPosition.y < -210f))
            {
                if ((transform.localPosition.y > 180f))
                {
                    transform.localPosition = new Vector2(transform.localPosition.x,180f);
                }
                if ((transform.localPosition.y < -210f))
                {
                    transform.localPosition = new Vector2(transform.localPosition.x, -210f);
                }
                if ((transform.localPosition.x > 675f))
                {
                    transform.localPosition = new Vector2(675, transform.localPosition.y);
                }
                if ((transform.localPosition.x < -675f))
                {
                    transform.localPosition = new Vector2(-675f, transform.localPosition.y);
                }
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

        currentSwipe.x = 0;
        //currentSwipe.Normalize();
       // Debug.Log(currentSwipe.x);
        //swipe upwards
        if (currentSwipe.y >150)
        {
            drag = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void SetInitial(GameObject gameObject)
    {
        drag = false;
        pointerDown = false;
        transform.localPosition = new Vector3(initialPos.x, initialPos.y);
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<CropHarvest>().enabled = true;
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Basket"))
        {
            insideBasket = true;
            InsideBasket();
        }
    }

    void InsideBasket()
    {
        if ((insideBasket == true) && (pointerDown==false))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CropHarvest>().enabled = false;
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            HarvestTheCropManager.Instance.CheckCropCount();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Basket"))
        {
            insideBasket = false;
        }
    }
}
