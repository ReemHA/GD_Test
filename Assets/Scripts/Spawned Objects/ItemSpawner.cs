using UnityEngine;

public class ItemSpawner : Spawner
{
    public SpawnedObject[] objectsToBeSpawned;
    private int randomIndex;

    new void Start()
    {
        base.Start();
    }

    protected override void OnGameStateChanged(GameStates gameState)
    {
        base.OnGameStateChanged(gameState);
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                SetSpawnedObjectsPositions();
                Invoke("Spawn", 0);
                break;
            case GameStates.GameEnds:
                CancelInvoke("Spawn");
                break;
            default:
                break;
        }
    }

    private void SetSpawnedObjectsPositions()
    {
        for (int i = 0; i < objectsToBeSpawned.Length; i++)
        {
            objectToBeSpawned = objectsToBeSpawned[i];
            objectToBeSpawned.referencePosition = mainCamera.transform.position;
            objectToBeSpawned.referencePosition.y = objectToBeSpawned.yOffset;
            objectToBeSpawned.spawnPosition = objectToBeSpawned.referencePosition;
            CreatePool(objectToBeSpawned.spawnLimit, objectToBeSpawned);
        }
    }

    protected override void Spawn()
    {
        objectToBeSpawned = spawnedObjects[randomIndex];
        if (!objectToBeSpawned.gameObject.activeInHierarchy)
        {
            if (objectToBeSpawned.respawn)
            {
                objectToBeSpawned.referencePosition = spawnedObjects[latestSpawnedObjectIndex].spawnPosition;
                objectToBeSpawned.xOffset = 3 * Random.Range(1.5f, 3f);
                objectToBeSpawned.spawnPosition.x = objectToBeSpawned.referencePosition.x;
                objectToBeSpawned.spawnPosition.x += objectToBeSpawned.xOffset;
                objectToBeSpawned.transform.position = objectToBeSpawned.spawnPosition;
                objectToBeSpawned.gameObject.SetActive(true);
                latestSpawnedObjectIndex = randomIndex;
            }
        }
        randomIndex = GetRandomIndex();
        Invoke("Spawn", objectToBeSpawned.spawnRate);
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, spawnedObjects.Count);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawSphere(objectToBeSpawned.spawnPosition, 2);
    }
}
