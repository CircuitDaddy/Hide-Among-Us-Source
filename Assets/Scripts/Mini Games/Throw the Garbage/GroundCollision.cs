using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Paper"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
}
