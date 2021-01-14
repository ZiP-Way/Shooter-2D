using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    [Header("Head Settings")]
    [SerializeField] private int health = 2;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask whoToDamage;
    [SerializeField] private int damageForObjects;
    [SerializeField] private float radiusOfDefeat;

    [Header("Blink Settings")]
    [SerializeField] private Material blinktMaterial;
    [SerializeField] private float timeToBlink = 0.1f;

    private Collider2D[] objectsInTheRadius;
    private Material deffaultMaterial;

    private SpriteRenderer spriteRend;
    private Vector2 explosionPosition;

    private bool isDead = false;

    private void Start()
    {
        spriteRend          = GetComponent<SpriteRenderer>();

        deffaultMaterial    = spriteRend.material;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        spriteRend.material = blinktMaterial;
        StartCoroutine(Blink());

        if (health <= 0 && !isDead)
        {
            isDead = true;

            explosionPosition = new Vector2(transform.position.x, transform.position.y + 1.5f); // change position of explosion
            Instantiate(explosion, explosionPosition, Quaternion.identity);

            StartCoroutine(Explosion());
        }
    }

    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(timeToBlink);
        spriteRend.material = deffaultMaterial;
    }

    private void WhoIsInRadius()
    {
        objectsInTheRadius = Physics2D.OverlapCircleAll(transform.position, radiusOfDefeat, whoToDamage);

        for (int i = 0; i < objectsInTheRadius.Length; i++)
        {
            if (objectsInTheRadius[i].CompareTag("Player"))
            {
                objectsInTheRadius[i].GetComponent<Player>().TakeDamage(damageForObjects);
            }
            else if (objectsInTheRadius[i].CompareTag("Barrel") && objectsInTheRadius[i].name != gameObject.name)
            {
                objectsInTheRadius[i].GetComponent<BarrelController>().TakeDamage(damageForObjects);
            }
            else if (objectsInTheRadius[i].CompareTag("Turrel"))
            {
                objectsInTheRadius[i].GetComponentInChildren<Turrel>().TakeDamage(damageForObjects);
            }
            else if (objectsInTheRadius[i].CompareTag("Enemy"))
            {
                objectsInTheRadius[i].GetComponent<Enemy>().TakeDamage(damageForObjects);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusOfDefeat);
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.2f);
        WhoIsInRadius();
        Destroy(gameObject);
    }
}
