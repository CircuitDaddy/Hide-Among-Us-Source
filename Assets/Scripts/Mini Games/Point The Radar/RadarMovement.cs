using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] FixedJoystick joyStick;

    [SerializeField] float movementSpeed = 300;
    float horizontalAxis = 0;
    float verticalAxis = 0;
    public bool once = true;
    // Start is called before the first frame update
    void Start()
    {
        once = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if ((Mathf.Abs(transform.localPosition.x) < 50f) && ((Mathf.Abs(transform.localPosition.y) < 50f)))
        {
            transform.localPosition = new Vector3(0, 0, 0);
            if (once)
            {
                RadarManager.Instance.TaskComplete();

                once = false;
            }
        }

    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
        {
            horizontalAxis = Input.GetAxis("Horizontal");
            verticalAxis = Input.GetAxis("Vertical");
        }
        else
        {
            horizontalAxis = joyStick.Horizontal;
            verticalAxis = joyStick.Vertical;
        }

        if (Mathf.Abs(horizontalAxis)>Mathf.Abs(verticalAxis))
        {
            verticalAxis = 0;
        }
        else
        {
            horizontalAxis = 0;
        }

        rb.velocity = (new Vector2(horizontalAxis, verticalAxis) * movementSpeed) * Time.deltaTime;
    }
}
