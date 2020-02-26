using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorScroller : MonoBehaviour
{
    [Header("Corridor elements")]
    public GameObject[] corridorPrefabs;
    public GameObject corridorPrev;
    public GameObject corridorCurrent;
    public GameObject corridorNext;

    [Header("Scrolling settings")]
    public bool random = false;
    public float elementLength = 10;
    public float scrollSpeed = 2f;
    public Vector3 scrollDir = Vector3.left;
    public float scrollPos = 0;
    public int elementIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        corridorPrev = LoadNext();
        corridorPrev.transform.position = scrollDir * elementLength;

        corridorCurrent = LoadNext();
        corridorCurrent.transform.position = scrollDir * 0;

        corridorNext = LoadNext();
        corridorNext.transform.position = scrollDir * -elementLength;
    }

    // Update is called once per frame
    void Update()
    {
        // scroll
        float scrollDelta = scrollSpeed * Time.deltaTime;
        scrollPos = scrollPos + scrollDelta;

        corridorPrev.transform.Translate(scrollDir * scrollDelta);
        corridorCurrent.transform.Translate(scrollDir * scrollDelta);
        corridorNext.transform.Translate(scrollDir * scrollDelta);

        // load & unload
        if(scrollPos >= elementLength) {
            scrollPos -= elementLength;

            if(corridorPrev != null)
                Destroy(corridorPrev);

            corridorPrev = corridorCurrent;
            corridorCurrent = corridorNext;
            corridorNext = LoadNext();
        }
    }

    GameObject LoadNext() {
        int index;
        if(random) {
            index = Random.Range(0, corridorPrefabs.Length);
        } else {
            index = elementIndex++ % corridorPrefabs.Length;
        }

        return Instantiate(corridorPrefabs[index], new Vector3(elementLength, 0f, 0f), Quaternion.identity, this.transform);
    }
}
