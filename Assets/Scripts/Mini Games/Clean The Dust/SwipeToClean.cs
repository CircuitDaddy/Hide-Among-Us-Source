using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeToClean : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Vector2 firstPressPos;
    [SerializeField] Vector2 secondPressPos;
    Vector2 currentSwipe;

    [SerializeField] float alphavalue = 1;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        currentSwipe.Normalize();

        if (firstPressPos != secondPressPos)
        {
            if (alphavalue > 0.4f)
            {
                alphavalue = alphavalue / 2;
            }
            else
            {
                alphavalue = 0;
            }
            gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, alphavalue);
        }
    }

    public void SetInitial(GameObject gameObject)
    {
        alphavalue = 1;
        gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, alphavalue);
    }
}
