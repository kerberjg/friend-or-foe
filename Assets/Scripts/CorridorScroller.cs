using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum GameState {
    warmup, running, cooldown, end,
    infinite
}

public class CorridorScroller : MonoBehaviour
{
    [Header("Corridor elements")] 
    public GameObject startPrefab;
    public GameObject[] corridorPrefabs;
    public GameObject endPrefab;
    private GameObject nextPrefab;


    [ReadOnly] public GameObject corridorPrev;
    [ReadOnly] public GameObject corridorCurrent;
    [ReadOnly] public GameObject corridorNext;

    [Header("Scrolling settings")]
    public bool random = false;
    public float elementLength = 10;
    public float scrollSpeed = 2f;
    private float speedScale = 1f;
    public Vector3 scrollDir = Vector3.left;
    public float scrollPos = 0;
    public int elementIndex = 0;
    public int firstPlayableIndex = 2;

    [Header("Animations")]
    private GameState state;
    public float warmupTime = 3f;
    public float cooldownTime = 3f;
    private float _timer;

    private GameObject door;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // initialize segments
        // corridorPrev = LoadNext();
        // if(corridorPrev) corridorPrev.transform.position = scrollDir * elementLength;

        corridorCurrent = LoadNext(0f, false, startPrefab);
        scrollPos = 5f;

        corridorNext = LoadNext(+elementLength, false, GetRandomPrefab());

        nextPrefab = GetRandomPrefab();

        // start timer
        if(!random) {
            state = GameState.warmup;
            _timer = warmupTime;
        } else {
            state = GameState.infinite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // debug
        Debug.Log("index: " + elementIndex);
        Debug.Log("status: " + state.ToString());

        // scroll
        if(state != GameState.end) {
            float scrollDelta = scrollSpeed * speedScale * Time.deltaTime;
            scrollPos = scrollPos + scrollDelta;

            // set segment position
            if(corridorPrev) corridorPrev.transform.position = scrollDir * (scrollPos + elementLength);
            if(corridorCurrent) corridorCurrent.transform.position = scrollDir * (scrollPos + 0);
            if(corridorNext) corridorNext.transform.position = scrollDir * (scrollPos + -elementLength);

            if (scrollPos >= elementLength) {
                scrollPos -= elementLength;
                ++elementIndex;

                if (corridorPrev != null)
                    Destroy(corridorPrev);

                corridorPrev = corridorCurrent;
                corridorCurrent = corridorNext;
                corridorNext = LoadNext(elementLength);
            }
        }


        // scroller state
        switch(state) {
            // we're in the first room
            case GameState.warmup: {
                // disable player movement
                TrySetPlayer(false);

                // ramp up scrolling speed
                speedScale = 1f - (_timer / warmupTime);

                if(_timer <= 0f && elementIndex > 0) {
                    state = GameState.running;
                }
            } break;

            // check if finished scrolling
            case GameState.infinite:
            case GameState.running: {
                TrySetPlayer(true);
                
                if (CheckLevelObjective()) {
                    nextPrefab = endPrefab;
                } else {
                    nextPrefab = GetRandomPrefab();
                }
            } break;

            case GameState.cooldown: {
                // slow down level scrolling
                speedScale = 1f - scrollPos / (elementLength / 1.5f);
                speedScale = Mathf.Sqrt(speedScale);    // make the speed curve nicer to look at
                speedScale = Mathf.Ceil(speedScale * 10f) / 10f;

                // move the player towards the exit
                if(!door) door = GameObject.Find("ExitDoor");
                if(!player) player = GameObject.Find("Player");

                TrySetPlayer(false);

                float playerWidth = player.transform.lossyScale.x;
                if (player.transform.position.x + playerWidth < door.transform.position.x)
                {
                    Vector3 dir = (door.transform.position - player.transform.position).normalized;
                    player.transform.Translate(dir * scrollSpeed * Time.deltaTime);
                }
                else {
                    state = GameState.end;
                }
            } break;

            //default:
            case GameState.end: {
                Debug.Log("Player exit the level");
                this.enabled = false;

                // TODO: go to a game over scene
                Application.Quit();
            } break;
        }



        // timer
        _timer -= Time.deltaTime;
    }

    GameObject LoadNext(float offset = 0f, bool spawnEnemies = true, GameObject prefab = null) {
        if(!prefab) {
            prefab = nextPrefab;
            nextPrefab = null;
            if(!prefab) {
                Debug.Log("Segment is null");
                return null;
            }
        }

        GameObject segment = Instantiate(prefab, new Vector3(offset, 0f, 0f), Quaternion.identity, this.transform);

        if (spawnEnemies && WaveSpawner.Instance != null)
        {
            WaveSpawner.Instance.FillSegment(segment, 0.5f);
        }

        return segment;
    }

    private void TrySetPlayer(bool enabled) {
        if(!player) player = GameObject.Find("Player");

        // disable the movement script
        player_scr mov;
        if(player && player.TryGetComponent<player_scr>(out mov)) {
            mov.enabled = enabled;
        }
    }

    private GameObject GetRandomPrefab() {
        return corridorPrefabs[Random.Range(0, corridorPrefabs.Length)];
    } 
    
    private bool CheckLevelObjective() {
        return false;
    }
}
