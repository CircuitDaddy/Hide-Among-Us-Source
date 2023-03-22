using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KillInsect : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Vector2 initialPos;

    [SerializeField] Sprite flySprite;
    [SerializeField] Sprite deadFlySprite;

    [SerializeField] GameObject Swatter;

    public void SetInitial(GameObject gameObject)
    {
        Swatter.SetActive(false);
        gameObject.GetComponent<Image>().sprite = flySprite;
        // gameObject.GetComponent<Image>().enabled = true;
        transform.localPosition = new Vector3(initialPos.x, initialPos.y);
        gameObject.GetComponent<InsectMovement>().enabled = true;
        gameObject.GetComponent<Image>().raycastTarget = true;
        gameObject.GetComponent<Image>().maskable = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Swatter.transform.localScale = new Vector3(0.8f,0.8f);
        StopAllCoroutines();
        Swatter.SetActive(true);
        StartCoroutine(SwatterProperties());
        Swatter.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        gameObject.GetComponent<Image>().sprite = deadFlySprite;
       // gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponent<InsectMovement>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Image>().raycastTarget = false;
        gameObject.GetComponent<Image>().maskable = false;
        KillTheInsectsManager.Instance.KillInsect();

    }

    IEnumerator SwatterProperties()
    {
        while (Swatter.transform.localScale.x > 0.4f)
        {
            Swatter.transform.localScale = new Vector3(Swatter.transform.localScale.x - 0.1f, Swatter.transform.localScale.y - 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        Swatter.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
    }
}
