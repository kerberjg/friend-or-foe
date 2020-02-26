using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn density control")]
    public int minCount;
    public int maxCount;


    // singleton
    public static EnemySpawner instance;
    void OnEnable() {
        instance = this;
    }
    
    void OnDisable() {
        instance = null;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillSegment(GameObject segment, float difficulty) {
        // calculate how many entitites to spawn
        int count = Mathf.RoundToInt( Mathf.Lerp(minCount, maxCount, difficulty) );

        for(int i = 0; i < count; i++) {
            Vector3 pos = new Vector3(); // calculate random position
            Spawn(pos, segment);
        }
            
    }

    void Spawn(Vector3 position, GameObject parent) {
        // spawn code for single entities goes here
    }
}
