using UnityEngine;

public class PlatformSpawner : Spawner
{
    private int latestSpawnedObjectIndex;
    private Vector2 referencePosition;

    new void Start()
    {
        SetTagObjectsToBeSpawned();
        base.Start();
        referencePosition = mainCamera.transform.position;
        referencePosition.y -= camHeight / 2;
        spawnPosition = referencePosition;
        latestSpawnedObjectIndex = 0;
    }

    public override void Spawn()
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (!spawnedObjects[i].activeInHierarchy)
            {
                spawnedObjects[i].transform.position = spawnPosition;
                spawnedObjects[i].SetActive(true);
                latestSpawnedObjectIndex = i;
                break;
            }
            else
            {
                if (latestSpawnedObjectIndex == i)
                {
                    referencePosition = spawnedObjects[i].transform.position;
                    spawnPosition = referencePosition;
                    spawnPosition.x += camWidth;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(referencePosition, 2);
    }
}
