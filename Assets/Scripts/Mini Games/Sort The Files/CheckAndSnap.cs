using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndSnap : MonoBehaviour
{
    public int positionNumber;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SnapPoint>() != null)
        {
            if (collision.gameObject.GetComponent<SnapPoint>().positionNumber == positionNumber)
            {
                collision.gameObject.GetComponent<FileMovement>().snapPos = this.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SnapPoint>() != null)
        {
            if (collision.gameObject.GetComponent<SnapPoint>().positionNumber == positionNumber)
            {
                collision.gameObject.GetComponent<FileMovement>().snapPos = null;
            }
        }
    }
}
