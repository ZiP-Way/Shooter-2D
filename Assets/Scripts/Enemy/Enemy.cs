using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject groundCheck;

    [SerializeField] private bool startPatrol;

    [SerializeField] private bool test = false;
    [SerializeField] private float startOfPatrolX = -15, endOfPatrolX = 1;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float range = 5f;

    private float localScaleX;

    private GroundCheck groundCheckObj;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator anim;

    private bool playerFinded;
    private bool onRight;

    private int amountOfBullets;
    private bool isShooting = true;

    void Start()
    {
        localScaleX = transform.localScale.x;

        player = GameObject.Find("Player");
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isShooting", true);

        groundCheckObj = GetComponentInChildren<GroundCheck>();
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < range)
        {
            playerFinded = true;
        }
        else
        {
            playerFinded = false;
        }

        if (startPatrol && !playerFinded)
        {
            anim.SetBool("isShooting", false);
            anim.SetBool("isRuning", true);
            Patrol();
        }
        else if (playerFinded)
        {
            Attack();

            anim.SetBool("isRuning", false);
            if (amountOfBullets >= 3)
            {
                StartCoroutine(WhenStartFire());
            }
        }
        else if (!startPatrol)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("isShooting", false);
            anim.SetBool("isRuning", false);
        }
    }

    public void TakeDamage(int damage)
    {
        //Получення урону 
    }
    void Patrol()
    {
        if (test)
        {
            if (transform.position.x > endOfPatrolX)
            {
                onRight = false;
            }
            else if (transform.position.x < startOfPatrolX)
            {
                onRight = true;
            }
        }
        else if (!test)
        {
            if (!groundCheckObj.isGrounded)
            {
                //onRight = !onRight;
                Flip();
                speed = -speed;
            }

            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        //if (onRight)
        //{
            
        //}
        //else
        //{
        //    rb.velocity = new Vector2(-speed, rb.velocity.y);
        //}
    }

    private void Flip()
    {
        localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, 1, 1);
    }
    void Attack()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (transform.position.x - player.transform.position.x > 0)
        {
            onRight = false;
        }
        else
        {
            onRight = true;
        }
    }

    private IEnumerator WhenStartFire()
    {
        isShooting = false;
        amountOfBullets = 0;
        anim.SetBool("isShooting", false);
        yield return new WaitForSeconds(2f);
        isShooting = true;
        anim.SetBool("isShooting", true);
    }

    public void Fire()
    {
        if (isShooting)
        {
            amountOfBullets += 1;
            Instantiate(bullet, shotPoint.position, transform.rotation);
        }
    }
}
