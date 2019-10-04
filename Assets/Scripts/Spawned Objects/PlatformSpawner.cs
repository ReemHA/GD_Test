using UnityEngine;

public class PlatformSpawner : Spawner
{
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
                InvokeRepeating("Spawn", objectToBeSpawned.startTime, objectToBeSpawned.spawnRate);
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
        objectToBeSpawned.referencePosition = mainCamera.transform.position;
        objectToBeSpawned.referencePosition.y -= camHeight / 2;
        objectToBeSpawned.spawnPosition = objectToBeSpawned.referencePosition;
        latestSpawnedObjectIndex = 0;
        CreatePool(objectToBeSpawned.spawnLimit, objectToBeSpawned);
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
