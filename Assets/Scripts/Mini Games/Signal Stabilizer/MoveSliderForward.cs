using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveSliderForward : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float waitforSec;
    [SerializeField] Slider slider;

    public void OnPointerDown(PointerEventData eventData)
    {
       // SignalStablizerManager.Instance.StopAllCoroutines();
        StartCoroutine(IncreaseSliderRoutine());
    }

   IEnumerator IncreaseSliderRoutine()
    {
        while ((slider.value != slider.maxValue))
        {
            slider.value = slider.value + 0.1f;
            yield return new WaitForSeconds(waitforSec);
            if (slider.value == slider.maxValue)
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
