using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private CircleCollider2D bulletCollider;
    private float speed;
    private bool isTouchObject = false;
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<CircleCollider2D>();

        speed = FindObjectOfType<Turrel>().bulletSpeed;
    }

    private void FixedUpdate()
    {
        if (!isTouchObject)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(1);
        }

        bulletCollider.enabled = false;

        isTouchObject = true;
        rb2D.velocity = new Vector2(0, 0);
    }
}
