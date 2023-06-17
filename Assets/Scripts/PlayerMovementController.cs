using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{
    public Rigidbody2D rb;
   Animator anim;

    [SerializeField] FixedJoystick joyStick;

    [SerializeField] float movementSpeed = 300;
    [SerializeField] GameObject shootPoint;
    float horizontalAxis = 0;
    float verticalAxis = 0;
    bool facingRight = true;
    public Sprite halfSprite;
    public bool isMine;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Flip(facingRight);

            joyStick = LevelManager.Instance.joyStick;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    void FixedUpdate()
    {
        if (LevelManager.Instance.shouldStop)
        {
            rb.velocity = Vector2.zero;
            joyStick.transform.GetChild(0).GetComponent<RectTransform>().position = Vector2.zero;
            horizontalAxis = 0;
            verticalAxis = 0;

          
            anim.SetFloat("Velocity", 0);
            return;
        }
           
        if (isMine)
        {
            //horizontalAxis = Input.GetAxis("Horizontal");
            //verticalAxis = Input.GetAxis("Vertical");

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
        }

        Vector3 moveVector = (Vector3.up * horizontalAxis - Vector3.right * verticalAxis);

        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
            shootPoint.transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector * Time.deltaTime);
        }

        if (horizontalAxis > 0)
        {
            Flip(true);
        }
        else if (horizontalAxis < 0)
        {
            Flip(false);
        }

        anim.SetFloat("Velocity", Mathf.Abs(horizontalAxis) + Mathf.Abs(verticalAxis));

        rb.velocity = (new Vector2(horizontalAxis, verticalAxis) * movementSpeed) * Time.deltaTime;
    }

    void Flip(bool FaceRight)
    {
        if (FaceRight)
        {
            transform.parent.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("River"))
        {
            if(isMine)
            {
                movementSpeed = 200f;
                Debug.Log("InWater");
                GetComponent<Animator>().enabled = false;
                transform.GetChild(0).transform.localPosition = new Vector2(0, -0.15f);
                GetComponent<SpriteRenderer>().sprite = halfSprite;
            }
           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("River"))
        {
            if (isMine)
            {
                transform.GetChild(0).transform.localPosition = Vector2.zero;
                movementSpeed = 300f;
                GetComponent<Animator>().enabled = true;
               
            }

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Boundry")
        {
            if(isMine)
            {
              //  Debug.LogError(collision.gameObject.name);
                if (collision.gameObject.GetComponent<ObjectProperties>() != null)
                {
                    if (collision.gameObject.GetComponent<ObjectProperties>().isObjectHideAble == true)
                    {
                        LevelManager.Instance.Hide();
                        LevelManager.Instance.SetHideableObject(collision.gameObject);
                    }
                    else
                    {
                        LevelManager.Instance.StopHide();
                    }
                }
            }
          
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Boundry")
        {
            if(isMine)
            {
               
                LevelManager.Instance.StopHide();
            }
           
        }
    }
 
}
