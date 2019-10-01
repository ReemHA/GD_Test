using UnityEngine;

public class SpawnedObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(Spawner.SPAWNED_OBJECT_TAG))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
