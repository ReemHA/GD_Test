using System;
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
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
    }

    protected virtual void OnGameStateChanged(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                camHeight = 2 * mainCamera.orthographicSize;
                camWidth = mainCamera.aspect * camHeight;
                spawnedObjects = new List<SpawnedObject>();
                break;
            case GameStates.GameEnds:
                spawnedObjects = new List<SpawnedObject>();
                break;
            default:
                break;
        }
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
