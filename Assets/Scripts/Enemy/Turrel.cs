using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrel : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private int turrelHealth = 5;
    [SerializeField] private GameObject turrelBody;

    [Header("Shooting Settings")]
    public float bulletSpeed = 0;

    [SerializeField] private float radiusOfDefeat = 0;
    [SerializeField] private float timeToFire = 4f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private Animator animController;
    private bool isShooting = false;

    private void Start()
    {
        animController = GetComponentInParent<Animator>();
    }
    private void FixedUpdate()
    {
        if (player != null) // if player is not dead
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= radiusOfDefeat)
            {
                Vector3 rotationSett = player.transform.position - transform.position;
                float rotationDegree = Mathf.Atan2(rotationSett.y, rotationSett.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationDegree);
                if (!isShooting)
                {
                    isShooting = true;
                    StartCoroutine(FireCouldown());
                }
            }
        }
    }

    private IEnumerator FireCouldown()
    {
        yield return new WaitForSeconds(timeToFire);
        Instantiate(bullet, bulletSpawnPoint.position, transform.rotation);
        isShooting = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusOfDefeat);
    }

    public void TakeDamage(int damage)
    {
        turrelHealth -= damage;
        if(turrelHealth <= 0)
        {
            animController.SetBool("isDestroyed", true);
            turrelBody.layer = 14;
            Destroy(gameObject);
        }
    }

}
