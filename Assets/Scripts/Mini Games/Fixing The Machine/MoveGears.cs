using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveGears : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float speed = 0;
    void OnEnable()
    {
        StopAllCoroutines();
      //  gameObject.transform.rotation = Quaternion.Euler(0, 0, 7);
      // otherGear.gameObject.transform.rotation = Quaternion.Euler(0, 0, 186);
        if (gameObject.transform.GetChild(0).GetComponent<AlignGears>()!=null)
        {
            gameObject.transform.GetChild(0).GetComponent<AlignGears>().enabled = true;
        }
        
    }
    void Start()
    {
       
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1, 1, 1);
        StopAllCoroutines();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1);
        StartCoroutine(StartRotatingRoutine());
    }
    public void SetInitial()
    {
        StopAllCoroutines();
       //gameObject.transform.rotation = Quaternion.Euler(0, 0, 7);
      //gameObject.transform.rotation = Quaternion.Euler(0, 0, 186);
    }
    IEnumerator StartRotatingRoutine()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
            yield return null;
        }
    }
}
