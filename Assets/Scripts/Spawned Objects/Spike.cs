using UnityEngine;

public class Spike : SpawnedObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<Player>().LivesCount--;
        }
    }
}
