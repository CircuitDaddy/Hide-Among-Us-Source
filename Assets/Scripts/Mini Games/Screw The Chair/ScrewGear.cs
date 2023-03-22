using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrewGear : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Vector2 initialScale;
    [SerializeField] Vector2 initialPos;
    [SerializeField] float speed = 0;
    [SerializeField] float offset = 0;
    [SerializeField] float timeToWait = 0;

    private void Awake()
    {
        initialScale = transform.localScale;
        initialPos = transform.localPosition;
    }

    public void SetInitial(GameObject gameObject)
    {
        gameObject.transform.localScale = new Vector2(initialScale.x, initialScale.y);
        gameObject.transform.localPosition = new Vector2(initialPos.x, initialPos.y);
        gameObject.GetComponent<ScrewGear>().enabled = true;
    }
    public void PointerUp()
    {
        StopAllCoroutines();
    }
    public void PointerDown()
    {
       
        StartCoroutine(StartRotatingRoutine());
        StartCoroutine(SetScaleRoutine());

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(StartRotatingRoutine());
        StartCoroutine(SetScaleRoutine());

    }
    IEnumerator StartRotatingRoutine()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
            yield return null;
        }
    }

    IEnumerator SetScaleRoutine()
    {
        while ((transform.localScale.x!=1f)&& (transform.localScale.y != 1f))
        {
            transform.localScale = new Vector2(transform.localScale.x - offset, transform.localScale.y - offset);
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + offset * 20f);
            yield return new WaitForSeconds(timeToWait);
            if (transform.localScale.x<2.05)
            {
                gameObject.GetComponent<ScrewGear>().enabled = false;
                StopAllCoroutines();
                transform.localScale = new Vector2(2,2);
                ScrewTheChairManager.Instance.CheckScrew();
            }
        }
    }
}
