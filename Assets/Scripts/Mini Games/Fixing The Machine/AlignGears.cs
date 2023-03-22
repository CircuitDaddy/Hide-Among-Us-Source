using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignGears : MonoBehaviour
{
    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;
    GameObject gearGameObject;

    [SerializeField] Animator animator;


    bool once = true;

    [SerializeField] GameObject hideButton;

    private void Start()
    {
        
    }

    void OnEnable()
    {
        once = true;
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        taskCompleted = false;
        taskCompletedObject.SetActive(false);
        animator.enabled = false;
    }
    private void Update()
    {
        if ((hideButton.GetComponent<HideButton>().timeCount <= 0) && taskCompleted == false)
        {
            if (once == true)
            {
                once = false;
                hideButton.GetComponent<HideButton>().IncompleteTask();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JoinPoint"))
        {
            gearGameObject = collision.gameObject;
            gameObject.transform.parent.gameObject.GetComponent<MoveGears>().StopAllCoroutines();
            collision.gameObject.transform.parent.gameObject.GetComponent<MoveGears>().StopAllCoroutines();
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            if (!taskCompleted)
            {
                StartCoroutine("TaskCompletedRoutine");
            }
        }
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;
        animator.enabled = true;

        yield return new WaitForSeconds(2);

        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(2);

        gameObject.transform.parent.gameObject.GetComponent<MoveGears>().SetInitial();
        gameObject.transform.parent.gameObject.GetComponent<MoveGears>().enabled = true;
        gearGameObject.gameObject.transform.parent.gameObject.GetComponent<MoveGears>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gearGameObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;

        LevelManager.Instance.GameWin();
        taskCompletedObject.SetActive(false);
    }
}
