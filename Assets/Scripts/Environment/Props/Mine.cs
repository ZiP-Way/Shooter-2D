using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    private Vector2 explosionPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            explosionPosition = new Vector2(transform.position.x, transform.position.y + 1.5f); // change position of explosion
            Instantiate(explosion, explosionPosition, Quaternion.identity);

            collision.GetComponent<Player>().ToKillPlayer();

            Destroy(gameObject);
        }
    }
}
