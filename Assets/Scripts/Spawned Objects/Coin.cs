using UnityEngine;

public class Coin : SpawnedObject
{
    private void Update()
    {
        transform.Rotate(transform.up, 360 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<Player>().CoinsCollected++;
        }
        this.gameObject.SetActive(false);
        respawn = false;
    }
}
