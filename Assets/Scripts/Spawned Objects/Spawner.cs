using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public SpawnedObject objectToBeSpawned;
    public List<SpawnedObject> spawnedObjects;
    protected int latestSpawnedObjectIndex;
    protected float camHeight;
    protected float camWidth;
    protected Camera mainCamera;

    public void Start()
    {
        mainCamera = Camera.main;
        camHeight = 2 * mainCamera.orthographicSize;
        camWidth = mainCamera.aspect * camHeight;
        spawnedObjects = new List<SpawnedObject>();
    }

    public void CreatePool(int objectCount, SpawnedObject objectToBeSpawned)
    {
        for (int i = 0; i < objectCount; i++)
        {
            GameObject obj = Instantiate(objectToBeSpawned.gameObject);
            obj.SetActive(false);
            if (obj.GetComponent<SpawnedObject>() == null)
            {
                obj.AddComponent<SpawnedObject>();
            }
            spawnedObjects.Add(obj.GetComponent<SpawnedObject>());
        }
    }

    protected abstract void Spawn();
}
