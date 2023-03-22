using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDelay : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
