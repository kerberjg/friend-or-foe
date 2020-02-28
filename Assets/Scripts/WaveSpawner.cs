﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawn density control")]
    [SerializeField]
    private int minCount = 1;
    [SerializeField]
    private int maxCount = 5;
    private int numberOfSpawnedBullies = 0;
    private int maxNumberOfSpawnedBullies = 0;

    [Header("WaveSettings")]
    public GameObject bullyPrefab;
    public GameObject nonBullyPrefab;
    [SerializeField]
    private float bullyProbabilityPercentage = 30;

    List<Vector3> availableSpawnPositions;


    // singleton
    private static WaveSpawner instance;

    public static WaveSpawner Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            availableSpawnPositions = new List<Vector3>();
        }
    }

    void OnDisable()
    {
        instance = null;
    }

    public void FillSegment(GameObject segment, float difficulty)
    {
        // calculate how many entitites to spawn
        //int count = Mathf.RoundToInt(Mathf.Lerp(minCount, maxCount, difficulty));
        int count = Random.Range(minCount, maxCount);
        numberOfSpawnedBullies = 0;
        maxNumberOfSpawnedBullies = Mathf.RoundToInt(bullyProbabilityPercentage * count * 0.01f);

        // calculate spawning positions and add them to a list to prevent spawning at the same position.
        SetAvailableSpawnPositions(segment);

        for (int i = 0; i < count; i++)
        {
            // pick a random index from the list
            int index = Random.Range(0, availableSpawnPositions.Count);
            Vector3 pos = availableSpawnPositions[index]; // calculate random position
            availableSpawnPositions.RemoveAt(index);    // Remove the position from the list so we make sure we don't pick it again.
            Spawn(pos, segment);
        }
    }

    void Spawn(Vector3 position, GameObject parent)
    {
        if (numberOfSpawnedBullies < maxNumberOfSpawnedBullies)
        {
            GameObject bully = GameObject.Instantiate(bullyPrefab, position, Quaternion.Euler(90.0f, 0.0f, 0.0f), parent.transform);
            numberOfSpawnedBullies++;
        }
        else
        {
            GameObject nonBully = GameObject.Instantiate(nonBullyPrefab, position, Quaternion.Euler(90.0f, 0.0f, 0.0f), parent.transform);
        }
    }

    void SetAvailableSpawnPositions(GameObject segment)
    {
        availableSpawnPositions.Clear();
        MeshRenderer floorMesh = segment.transform.Find("Floor").GetComponent<MeshRenderer>();

        float xMin = floorMesh.bounds.min.x + 0.2f;
        float xMax = floorMesh.bounds.max.x - 0.2f;
        float yMin = floorMesh.bounds.min.y + 0.2f;
        float yMax = floorMesh.bounds.max.y - 0.2f;
        float z = floorMesh.bounds.min.z;
        float offset = 0.5f;

        for (float x = xMin; x < xMax; x++)
        {
            for (float y = yMin; y < yMax; y += offset)
            {
                Vector3 spawnPosition = new Vector3(x, y, z + 0.3f);
                availableSpawnPositions.Add(spawnPosition);
            }
        }
    }
}
