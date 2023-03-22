using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndShoot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] GameObject pointPreFab;
    [SerializeField] GameObject[] points;


    [SerializeField] int noOfPoints;
    [SerializeField] float dotSpacing;

    [SerializeField] Vector2 initialPos;

    float timeStamp;

    Vector2 startPos, endPos, direction;
   Rigidbody2D myRigidbody2D;
    public float shootPower = 5f;

    void Start()
    {
        points = new GameObject[noOfPoints];
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
        {
            myRigidbody2D.isKinematic = true;
            myRigidbody2D.gravityScale = 0;
            for (int i = 0; i < noOfPoints; i++)
            {
                points[i] = Instantiate(pointPreFab, transform.position, Quaternion.identity);
                points[i].transform.SetParent(this.gameObject.transform, false);
            }
            for (int i = 0; i < noOfPoints; i++)
            {
                points[i].SetActive(false);
            }
            startPos = Input.mousePosition;
            for (int i = 0; i < noOfPoints; i++)
            {
                points[i].SetActive(true);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < points.Length; i++)
            {
                Destroy(points[i]);
            }
            endPos = Input.mousePosition;
            direction = startPos - endPos;
            myRigidbody2D.isKinematic = false;
            myRigidbody2D.gravityScale = 1;
            myRigidbody2D.AddForce(direction * shootPower);
        }
    }

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos = (Vector2)transform.position + (direction.normalized * 15f * t) + 0.5f * Physics2D.gravity * (t*t);

        return currentPointPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction2;
            endPos = Input.mousePosition;
            direction = startPos - endPos;

        direction2 = (startPos - endPos);


        for (int i=0; i<points.Length; i++)
        {
            points[i].transform.position = PointPosition(i * 0.1f);
            UpdateDots(gameObject.transform.position, direction2);
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {
        //Debug.Log(forceApplied);
        //forceApplied = new Vector2(Mathf.Clamp(forceApplied.x, -40, 40), Mathf.Clamp(forceApplied.y, -80, 80));
        timeStamp = dotSpacing;
        for (int i = 0; i < noOfPoints; i++)
        {
            initialPos.x = (ballPos.x + forceApplied.x * timeStamp/2);
            initialPos.y = (ballPos.y + forceApplied.y * timeStamp/2) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 0.09f;
            points[i].transform.position = initialPos;
            timeStamp += dotSpacing;
        }
    }

}
