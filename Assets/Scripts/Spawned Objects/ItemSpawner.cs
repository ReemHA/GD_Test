using UnityEngine;

public class ItemSpawner : Spawner
{
    public SpawnedObject[] objectsToBeSpawned;
    public float yOffset = -8.6f;
    public float xOffset;
    private int randomIndex;
    private int randomIndexOld;

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
            objectToBeSpawned.referencePosition.y = yOffset;
            objectToBeSpawned.spawnPosition = objectToBeSpawned.referencePosition;
            CreatePool(objectToBeSpawned.spawnLimit, objectToBeSpawned);
        }
    }

    protected override void Spawn()
    {
        if (!spawnedObjects[randomIndex].gameObject.activeInHierarchy 
            && spawnedObjects[randomIndex].respawn)
        {
            objectToBeSpawned.spawnPosition.x += xOffset;
            spawnedObjects[randomIndex].transform.position = objectToBeSpawned.spawnPosition;
            spawnedObjects[randomIndex].gameObject.SetActive(true);
            latestSpawnedObjectIndex = randomIndex;
            xOffset = 3 * Random.Range(1.5f, 3f);
        }
        randomIndex = GetRandomIndex();
        Invoke("Spawn", objectToBeSpawned.spawnRate);
        randomIndexOld = randomIndex;
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
