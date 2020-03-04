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
    private float speedScale = 1f;
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
        float scrollDelta = scrollSpeed * speedScale * Time.deltaTime;
        scrollPos = scrollPos + scrollDelta;

        corridorPrev.transform.Translate(scrollDir * scrollDelta);
        corridorCurrent.transform.Translate(scrollDir * scrollDelta);
        corridorNext.transform.Translate(scrollDir * scrollDelta);

        // check if finished scrolling
        if (elementIndex > corridorPrefabs.Length)
        {
            // slow down level scrolling
            speedScale = 1 - scrollPos / elementLength;
            //speedScale = 1 - Mathf.Sqrt(speedScale);    // make the speed curve nicer to look at
            //speedScale = Mathf.Pow(speedScale, 2);

            // move the player towards the exit
            GameObject door = GameObject.Find("ExitDoor");
            GameObject player = GameObject.Find("Player");

            if (player.transform.position.x < door.transform.position.x)
            {
                Vector3 dir = (door.transform.position - player.transform.position).normalized;
                player.transform.Translate(dir * scrollSpeed * Time.deltaTime);
            }
            else {
                Debug.Log("Player exit the level");
                Application.Quit();
            }
        }
        // load & unload
        else
        {
            if (scrollPos >= elementLength)
            {
                scrollPos -= elementLength;

                if (corridorPrev != null)
                    Destroy(corridorPrev);

                corridorPrev = corridorCurrent;
                corridorCurrent = corridorNext;
                corridorNext = LoadNext(corridorCurrent.transform.position.x);
            }
        }
    }

    GameObject LoadNext(float offset = 0f) {
        int index;
        float difficulty;
        if(random) {
            index = Random.Range(0, corridorPrefabs.Length);
            difficulty = 0.5f;  // TODO: make this settable from a GameManager
        } else {
            index = elementIndex++ % corridorPrefabs.Length;
            difficulty = index / corridorPrefabs.Length;
        }

        GameObject segment = Instantiate(corridorPrefabs[index], new Vector3(elementLength, 0f, 0f), Quaternion.identity, this.transform);
        if (WaveSpawner.Instance != null)
        {
            WaveSpawner.Instance.FillSegment(segment, difficulty);
        }
        else
        {
            Debug.Log("EnemySpawner is null");
        }
        return segment;
    }
}
