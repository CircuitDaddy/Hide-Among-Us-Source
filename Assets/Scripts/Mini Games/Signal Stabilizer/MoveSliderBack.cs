using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveSliderBack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float waitforSec;
    [SerializeField] Slider slider;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(DecreaseSliderRoutine());
    }

    IEnumerator DecreaseSliderRoutine()
    {
        while ((slider.value != slider.minValue))
        {

            slider.value = slider.value - 0.1f;
            yield return new WaitForSeconds(waitforSec);
            if (slider.value == slider.minValue)
            {
                break;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        SignalStablizerManager.Instance.WiningCondition();
    }
}
