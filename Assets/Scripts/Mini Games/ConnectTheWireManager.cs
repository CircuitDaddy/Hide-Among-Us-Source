using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectTheWireManager : MonoBehaviour
{
    [SerializeField] GameObject[] connectWireVarients;
    private void OnEnable()
    {
        for (int i=0; i<connectWireVarients.Length; i++)
        {
            connectWireVarients[i].SetActive(false);
        }
        int rand = 0;
        rand = Random.Range(0, connectWireVarients.Length);
        connectWireVarients[rand].SetActive(true);
    }
}
