using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSpanwer : MonoBehaviour
{

    public static DataSpanwer Instance;

    [SerializeField] GameObject[] dataPrefab;
    [SerializeField] float spawntime = 1f;

    [SerializeField] int minRange, maxRange;
    [SerializeField] GameObject parent;
    int select;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(wave());
    }
    public void DeleteParent()
    {
        StopAllCoroutines();
       // Destroy(parent.gameObject);
    }
    private void spawn()
    {
        GameObject b = Instantiate(dataPrefab[select], transform.position, Quaternion.identity) as GameObject;
        b.transform.SetParent(parent.transform, false);
    }
    IEnumerator wave()
    {
        transform.position = new Vector2(Random.Range(minRange, maxRange), transform.position.y);
        select = Random.Range(0, 2);
        spawn();
        yield return new WaitForSeconds(spawntime);
        StartCoroutine(wave());
    }
}
