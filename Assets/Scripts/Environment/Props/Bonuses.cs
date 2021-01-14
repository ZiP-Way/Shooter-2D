using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    [SerializeField] private Material blinkEffect;
    [SerializeField] private float moveDistanceY = 0.015f;

    private SpriteRenderer spriteRend;
    private Material deffaultMaterial;
    private Vector2 startPosition;
    private bool isBlinking = false;
    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        deffaultMaterial = spriteRend.material;

        startPosition = transform.position;
    }
    private void FixedUpdate()
    {
        if(gameObject.tag == "BoxOfPatrons")
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + moveDistanceY);
            if (transform.position.y >= startPosition.y + 0.25f)
            {
                moveDistanceY = -moveDistanceY;
            }
            else if(transform.position.y <= startPosition.y)
            {
                moveDistanceY = -moveDistanceY;
            }

            if(!isBlinking)
                StartCoroutine(BlinkEffect());
        }
    }

    IEnumerator BlinkEffect()
    {
        isBlinking = true;

        spriteRend.material = blinkEffect;
        yield return new WaitForSeconds(0.1f);
        spriteRend.material = deffaultMaterial;
        yield return new WaitForSeconds(1.5f);

        isBlinking = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.tag == "BoxOfPatrons")
            {
                collision.GetComponent<Player>().PutPatrons(20);
                Destroy(gameObject);
            }
        }
    }
}
