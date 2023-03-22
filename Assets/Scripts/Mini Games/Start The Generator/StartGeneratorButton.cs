using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StartGeneratorButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        StartGeneratorMiniGame.Instance.OnStartPress();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartGeneratorMiniGame.Instance.OnStartButtonClick();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
