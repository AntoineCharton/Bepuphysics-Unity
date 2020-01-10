using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAtRandomPosition : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfPrefabs;
    public int instantiatedObjects;
    public float randomRange;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(InstantiateObject());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator InstantiateObject()
    {
        while(instantiatedObjects < numberOfPrefabs)
        {
            var position = new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
            Instantiate(prefab, position, Quaternion.identity);
            instantiatedObjects++;
            yield return new WaitForEndOfFrame();
            position = new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
            Instantiate(prefab, position, Quaternion.identity);
            instantiatedObjects++;
        }
    }
}
