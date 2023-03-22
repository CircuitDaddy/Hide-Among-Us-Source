using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed;
    [SerializeField] float minLimit;
    [SerializeField] float maxLimit;

    public bool once = true;
    public bool once2 = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.localPosition.x <= minLimit) && (once))
        {
            once = false;
            once2 = true;
            moveSpeed *= -1;
        }
        else if ((transform.localPosition.x >= maxLimit) && (once2))
        {
            once2 = false;
            once = true;
            moveSpeed *= -1;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveSpeed * Time.deltaTime, 0);
    }
}
