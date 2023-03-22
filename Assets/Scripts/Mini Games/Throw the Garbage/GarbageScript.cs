using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GarbageScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPressed = false;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Rigidbody2D Hook;
    float releaseTime = .15f;
    [SerializeField] float maxDragDistance = 2f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
	{
		if (isPressed)
		{
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector3.Distance(mousePos, Hook.position) > maxDragDistance)
                rb.position = Hook.position + (mousePos - Hook.position).normalized * maxDragDistance;
            else
                rb.position = mousePos;
        }
	}
    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime);
        GetComponent<SpringJoint2D>().enabled = false;
       // this.enabled = false;
        yield return new WaitForSeconds(0.1f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        rb.isKinematic = false;
        StartCoroutine(Release());
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        rb.isKinematic = true;
    }
}
