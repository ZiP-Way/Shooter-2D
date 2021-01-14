using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0;

    private Rigidbody2D rb2D;
    private BoxCollider2D bulletCollider;
    private Player player;
    private Animator animController;

    private float moveInput;
    private bool dfa;
    private bool isTouchObject = false;
    private void Start()
    {
        rb2D                = GetComponent<Rigidbody2D>();
        animController      = GetComponent<Animator>();
        bulletCollider      = GetComponent<BoxCollider2D>();

        player              = FindObjectOfType<Player>();

        moveInput           = player.transform.localScale.x;
        dfa = player.isWatchingUp;
    }

    private void FixedUpdate()
    {
        if (!isTouchObject)
        {
            if (!dfa)
            {
                if (moveInput == 1)
                {
                    rb2D.velocity = new Vector2(Vector2.right.x * speed, rb2D.velocity.y);
                }
                else if (moveInput == -1)
                {
                    rb2D.velocity = new Vector2(Vector2.right.x * -speed, rb2D.velocity.y);
                }
            }
            else
            {
                rb2D.velocity = new Vector2(Vector2.up.x * speed, Vector2.up.y * speed);
            }
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrel"))
        {
            collision.GetComponent<BarrelController>().TakeDamage(1);
        }
        else if (collision.CompareTag("Turrel"))
        {
            collision.GetComponentInChildren<Turrel>().TakeDamage(1);
        }

        bulletCollider.enabled = false;
        isTouchObject = true;
        animController.SetBool("isTouchingObject", true);
        rb2D.velocity = new Vector2(0, 0);
    }
}
