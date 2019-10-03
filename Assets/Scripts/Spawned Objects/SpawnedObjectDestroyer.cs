using UnityEngine;
using System;

public class SpawnedObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Enum.IsDefined(typeof(ObjectsTags), collision.tag))
        {
            collision.gameObject.SetActive(false);
            if (collision.tag.Equals(ObjectsTags.Coin))
            {
                collision.gameObject.GetComponent<Coin>().respawn = false;
            }
        }
    }
}
