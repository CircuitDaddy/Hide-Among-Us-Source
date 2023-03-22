using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWireManager : MonoBehaviour
{
    public static ConnectWireManager Instance;

    [SerializeField] GameObject[] images;

    [SerializeField] int[] rotateVal;

    [SerializeField] List<Vector3> wires;


    [SerializeField] GameObject hideButton;

    bool once = true;

    public bool win = false;

    [Header("Task Completed")]
    [SerializeField] bool taskCompleted = false;
    [SerializeField] GameObject taskCompletedObject;

    private void OnEnable()
    {
        hideButton = GameObject.FindGameObjectWithTag("Finish");
        for (int i=0; i<images.Length; i++)
        {
            wires[i] = images[i].gameObject.transform.localRotation.eulerAngles;
            //wires[i] = new Vector3(wires[i].x, wires[i].y, wires[i].z - 360);

        }
        Instance = this;
        taskCompleted = false;
        taskCompletedObject.SetActive(false);
        once = true;


        for (int i = 0; i < images.Length; i++)
        {
            images[i].transform.rotation = Quaternion.identity;
            int rotateIndex = Random.Range(0, rotateVal.Length);
            images[i].transform.Rotate(0, 0, rotateVal[rotateIndex]);
            images[i].gameObject.GetComponent<RotateImage>().enabled = true;
        }
    }

    private void Update()
    {
        if ((hideButton.GetComponent<HideButton>().timeCount <= 0) && taskCompleted == false)
        {
            if (once == true)
            {
                once = false;
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].gameObject.GetComponent<RotateImage>().enabled = false;
                }
                hideButton.GetComponent<HideButton>().IncompleteTask();
            }
        }
    }


    public void CheckWire()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if ((images[i].gameObject.transform.localRotation.eulerAngles==Vector3.zero)|| (images[i].gameObject.transform.localRotation.eulerAngles.z==180f))
            {
                win = true;
            }
            else
            {
                win = false;
                break;
            }
        }

        if(win)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.GetComponent<RotateImage>().enabled = false;
            }
            StartCoroutine(TaskCompletedRoutine());
        }
    }

    IEnumerator TaskCompletedRoutine()
    {
        hideButton.GetComponent<HideButton>().stoptimer = true;
        yield return new WaitForSeconds(1);
        taskCompleted = true;
        taskCompletedObject.SetActive(true);

        yield return new WaitForSeconds(1);
        LevelManager.Instance.GameWin();
        taskCompletedObject.SetActive(false);
    }
}
