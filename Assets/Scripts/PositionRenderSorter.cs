using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderSorter : MonoBehaviour
{
    [SerializeField] int sortingOrderBase = 500;
    [SerializeField] int offset = 0;

    [SerializeField] bool runOnlyOnce = false;

    Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myRenderer.sortingOrder = (int)(sortingOrderBase- transform.position.y-offset);

        if(runOnlyOnce)
        {
            Destroy(this);
        }
    }
}
