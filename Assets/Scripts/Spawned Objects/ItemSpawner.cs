using UnityEngine;

public class ItemSpawner : Spawner
{
    public SpawnedObject[] objectsToBeSpawned;
    public float yOffset = -1;
    public float xOffset;
    private int randomIndex;
    private int randomIndexOld;

    new void Start()
    {
        base.Start();
        for (int i = 0; i < objectsToBeSpawned.Length; i++)
        {
            objectToBeSpawned = objectsToBeSpawned[i];
            objectToBeSpawned.referencePosition = mainCamera.transform.position;
            objectToBeSpawned.referencePosition.y = yOffset;
            objectToBeSpawned.spawnPosition = objectToBeSpawned.referencePosition;
            CreatePool(objectToBeSpawned.spawnLimit, objectToBeSpawned);
        }
        Invoke("Spawn", 0);
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
            xOffset = 50 * Random.Range(1, 4);
        }
        else
        {
            if (latestSpawnedObjectIndex == randomIndexOld)
            {
                objectToBeSpawned.referencePosition.x = spawnedObjects[randomIndex].transform.position.x;
                objectToBeSpawned.spawnPosition = objectToBeSpawned.referencePosition;
                objectToBeSpawned.spawnPosition.x += xOffset;
                xOffset = 50 * Random.Range(1, 4);
            }
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
        Gizmos.DrawSphere(objectToBeSpawned.spawnPosition, 2);
    }
}
