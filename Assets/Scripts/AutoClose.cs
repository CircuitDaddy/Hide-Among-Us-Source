using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AutoClose : MonoBehaviour
{
    public string sceneToLoad;
    AsyncOperation loadingOperation;
   [SerializeField] Slider progressBar;
    public Text percentLoaded;
    void OnEnable()
    {
     
    }

    private void Update()
    {
      
    }



}
