using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner Instance;

    [SerializeField] Transform spawnBallPos;
    [SerializeField] GameObject paperBall;
    [SerializeField] GameObject ball;

    [SerializeField] Sprite[] paperBallArt;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;


    bool once = true;

    [SerializeField] GameObject hideButton;
    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        once = true;
        taskCompletedObject.SetActive(false);
        Instance = this;
        taskCompleted = false;
        ball = Instantiate(paperBall);
        int rand = Random.Range(0, paperBallArt.Length);
        ball.GetComponent<Image>().sprite = paperBallArt[rand];
        ball.transform.localScale = new Vector3(1, 1, 1);
        ball.transform.SetParent(spawnBallPos.transform, false);

        ball.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }
    private void OnDisable()
    {
        Destroy(ball.gameObject);
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
    public void OnLose()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;
        hideButton.GetComponent<HideButton>().IncompleteTask();
        StartCoroutine(GameLoseRoutine());
    }

    IEnumerator GameLoseRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        LevelManager.Instance.GameLose();
    }

    public void OnWin()
    {
        StartCoroutine("TaskCompletedRoutine");
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;

        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);
        LevelManager.Instance.GameWin();
        taskCompletedObject.SetActive(false);
    }
}