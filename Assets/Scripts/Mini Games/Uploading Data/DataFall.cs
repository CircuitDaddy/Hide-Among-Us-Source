using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFall : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
            rb.velocity = new Vector2(0, -2.0f + speed);
    }
}
