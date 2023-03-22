using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMedKit : MonoBehaviour
{
    public int positionNumber;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TagNumber>() != null)
        {
            if (collision.gameObject.GetComponent<TagNumber>().positionNumber == positionNumber)
            {
                collision.gameObject.GetComponent<MoveItems>().snapPos = this.gameObject.transform.parent.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TagNumber>() != null)
        {
            if (collision.gameObject.GetComponent<TagNumber>().positionNumber == positionNumber)
            {
                collision.gameObject.GetComponent<MoveItems>().snapPos = null;
            }
        }
    }
}
