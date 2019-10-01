using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public GameObject[] objectsToBeSpawned;
    public List<GameObject> spawnedObjects;
    public float spawnRate;
    public float startTime;
    public int maxSpawnedCount;
    public static string SPAWNED_OBJECT_TAG = "Spawned";
    protected Vector2 spawnPosition;
    protected float camHeight;
    protected float camWidth;
    protected Camera mainCamera;

    public void Start()
    {
        mainCamera = Camera.main;
        camHeight = 2 * mainCamera.orthographicSize;
        camWidth = mainCamera.aspect * camHeight;
        spawnedObjects = new List<GameObject>();
        for (int i = 0; i < maxSpawnedCount; i++)
        {
            GameObject obj = 
                Instantiate(objectsToBeSpawned[Random.Range(0, objectsToBeSpawned.Length)]);
            obj.SetActive(false);
            spawnedObjects.Add(obj);
        }
        InvokeRepeating("Spawn", startTime, spawnRate);
    }

    protected virtual void SetTagObjectsToBeSpawned()
    {
        for (int i = 0; i < objectsToBeSpawned.Length; i++)
        {
            objectsToBeSpawned[i].tag = SPAWNED_OBJECT_TAG;
        }
    }

    public abstract void Spawn();
}
