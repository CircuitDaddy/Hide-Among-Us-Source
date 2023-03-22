using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignalSpeed : MonoBehaviour
{
    public float speed=1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Abs(speed);
        GetComponent<Image>().material.mainTextureOffset = new Vector2(speed*Time.time, 0);
    }
}
