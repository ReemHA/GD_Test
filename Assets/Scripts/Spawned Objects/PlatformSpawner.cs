using UnityEngine;

public class PlatformSpawner : Spawner
{
    new void Start()
    {
        base.Start();
        objectToBeSpawned.referencePosition = mainCamera.transform.position;
        objectToBeSpawned.referencePosition.y -= camHeight / 2;
        objectToBeSpawned.spawnPosition = objectToBeSpawned.referencePosition;
        latestSpawnedObjectIndex = 0;
        CreatePool(objectToBeSpawned.spawnLimit, objectToBeSpawned);
        InvokeRepeating("Spawn", objectToBeSpawned.startTime, objectToBeSpawned.spawnRate);
    }

    protected override void Spawn()
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (!spawnedObjects[i].gameObject.activeInHierarchy)
            {
                spawnedObjects[i].transform.position = objectToBeSpawned.spawnPosition;
                spawnedObjects[i].gameObject.SetActive(true);
                latestSpawnedObjectIndex = i;
                break;
            }
            else
            {
                if (latestSpawnedObjectIndex == i)
                {
                    objectToBeSpawned.referencePosition = spawnedObjects[i].transform.position;
                    objectToBeSpawned.spawnPosition = objectToBeSpawned.referencePosition;
                    objectToBeSpawned.spawnPosition.x += camWidth;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(objectToBeSpawned.referencePosition, 2);
    }
}
