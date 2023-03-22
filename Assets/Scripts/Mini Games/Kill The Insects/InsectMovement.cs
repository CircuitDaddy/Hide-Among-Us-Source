using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectMovement : MonoBehaviour
{
    enum EnemyMovement
    {
        horizontal,
        vertical
    }

    [SerializeField] EnemyMovement enemyState;

    Rigidbody2D rb;

    public float moveSpeed;
    [SerializeField] float minLimit;
    [SerializeField] float maxLimit;

    public bool once = true;
   public bool once2 = true;

    [SerializeField] float minLimitY;
    [SerializeField] float maxLimitY;

    private void OnEnable()
    {
        enemyState = EnemyMovement.horizontal;
        StartCoroutine(SwitchMovement());
        rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator SwitchMovement()
    {
        yield return new WaitForSeconds(10f);
        if (enemyState==EnemyMovement.horizontal)
        {
            if (once2 == true)
            {
                transform.eulerAngles = new Vector3(0, 0, -90);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
            enemyState = EnemyMovement.vertical;
        }
        else
        {
            if (once2 == true)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);

            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            enemyState = EnemyMovement.horizontal;
        }
        StartCoroutine(SwitchMovement());
    }
    private void Update()
    {
        switch (enemyState)
        {
            case EnemyMovement.horizontal:
                if ((transform.localPosition.x <= minLimit)&& (once))
                {
                    once = false;
                    once2 = true;
                    moveSpeed *= -1;
                    transform.eulerAngles = new Vector3(0, 0, 180);
                }
                else if ((transform.localPosition.x >= maxLimit) && (once2))
                {
                    once2 = false;
                    once = true;
                    moveSpeed *= -1;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                break;

            case EnemyMovement.vertical:
                if ((transform.localPosition.y <= minLimitY) && (once))
                {
                    once = false;
                    once2 = true;
                    moveSpeed *= -1;
                    transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else if ((transform.localPosition.y >= maxLimitY) && (once2))
                {
                    once2 = false;
                    once = true;
                    moveSpeed *= -1;
                    transform.eulerAngles = new Vector3(0, 0, 90);
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (enemyState == EnemyMovement.horizontal)
        {
            rb.velocity = new Vector2(moveSpeed * Time.deltaTime, 0);
        }
        else
        {
            rb.velocity = new Vector2(0, moveSpeed * Time.deltaTime);
        }
        
    }
}
