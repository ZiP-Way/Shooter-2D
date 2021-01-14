using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour
{
    [SerializeField] Sprite openCase;

    private SpriteRenderer spriteRenderer;

    private bool playerOnCase = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (playerOnCase)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                spriteRenderer.sprite = openCase;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnCase = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnCase = false;
        }
    }
}
