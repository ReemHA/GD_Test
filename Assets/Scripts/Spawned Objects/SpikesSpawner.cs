using UnityEngine;

public class SpikesSpawner : Spawner
{
    public float yOffset;
    public float xOffset;

    new void Start()
    {
        SetTagObjectsToBeSpawned();
        base.Start();
        spawnPosition = mainCamera.transform.position;
        spawnPosition.y = yOffset;
    }

    public override void Spawn()
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (!spawnedObjects[i].activeInHierarchy)
            {
                spawnPosition.x += xOffset;
                spawnedObjects[i].transform.position = spawnPosition;
                spawnedObjects[i].SetActive(true);
                xOffset = 50 * Random.Range(1, 3);
                break;
            }
        }
    }

    protected override void SetTagObjectsToBeSpawned()
    {
        for (int i = 0; i < objectsToBeSpawned.Length; i++)
        {
            if (objectsToBeSpawned[i].GetComponent<Spike>() != null)
            {
                objectsToBeSpawned[i].tag = SPAWNED_OBJECT_TAG;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnPosition, 2);
    }
}
