using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private Joystick joystick;
    [Header("Main Settings")]
    [SerializeField] private float playerHealth = 5;

    [Header("Speeds Settings")]
    [SerializeField] private float playerSpeed = 0;
    [SerializeField] private float jumpForce = 0;
    [SerializeField] private float jumpForceOnRope = 0;
    private float reserveJumpForce;

    [Header("Shooting Settings")]
    [SerializeField] private Text textAmountOfBullets;
    [SerializeField] private int amountOfAllPatrons = 0;
    [SerializeField] private int amountOfPatronsInMagazine = 0;
    [SerializeField] private Transform gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float timeForRecharge = 0.5f;

    private int reserveAmountPatrons;

    [Header("Soul Settings")]
    [SerializeField] private GameObject soul;

    [Header("Particles Settings")]
    [SerializeField] private GameObject jumpDust;
    [SerializeField] private GameObject runDust;
    [SerializeField] private float timer = 3; // Timer for spawn runDust;

    private Rigidbody2D rb2D;
    private Animator animController;
    private Vector2 position;
    private GroundCheck groundCheck;

    private int moveInput;
    private float timerReserve;

    private bool isGrounded = false;
    private bool isMoving = false;
    private bool isShooting = false;
    private bool isDead = false;
    private bool isHurting = false;
    private bool isRecharching = false;
    private bool isCrouching = false;
    private bool isHagging = false;

    private Vector2 weaponPosition_Staying;
    private Vector2 weaponPosition_Crouching;
    private Vector2 weaponPosition_Hagging;
    private Vector2 weaponPosition_WatchingUp;
    private Vector2 weaponPosition_HaggingWatchingUp;

    public bool isWatchingUp = false;
    public bool hagging_isWatchingDown = false;

    private bool isOnPlatform = false;

    private GameObject test;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animController = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();

        timerReserve = timer;
        reserveJumpForce = jumpForce;

        textAmountOfBullets.text = $"{amountOfPatronsInMagazine}/{amountOfAllPatrons}";
        reserveAmountPatrons = amountOfPatronsInMagazine;

        weaponPosition_Staying = new Vector2(1.6f, -0.5f);
        weaponPosition_Crouching = new Vector2(1.8f, -1.25f);
        weaponPosition_Hagging = new Vector2(1.45f, -0.3f);
        weaponPosition_WatchingUp = new Vector2(0.1f, 1.4f);
        weaponPosition_HaggingWatchingUp = new Vector2(-0.4f, 1.5f);
    }

    private void FixedUpdate()
    {
        if (!isHurting && !isDead)
        {
            if (joystick.Vertical < -0.8f)
            {
                //Debug.Log("DOWN");
                animController.SetBool("isCrouching", true);

                rb2D.velocity = new Vector2(0, rb2D.velocity.y);

                isCrouching = true;
                isMoving = false;
                if(isHagging)
                    hagging_isWatchingDown = true;

                if (!hagging_isWatchingDown)
                    gun.transform.localPosition = weaponPosition_Crouching;
            }
            else if (joystick.Vertical > 0.8f)
            {
                //Debug.Log("UP");
                animController.SetBool("isWatchingUp", true);
                animController.SetBool("isRunFire", false);

                hagging_isWatchingDown = false;

                rb2D.velocity = new Vector2(0, rb2D.velocity.y);

                isWatchingUp = true;
                isMoving = false;

                if (isHagging)
                {
                    gun.transform.localPosition = weaponPosition_HaggingWatchingUp;
                }
                else
                {
                    gun.transform.localPosition = weaponPosition_WatchingUp;
                }
            }

            else
            {
                animController.SetBool("isCrouching", false);
                animController.SetBool("isWatchingUp", false);
                hagging_isWatchingDown = false;

                isWatchingUp = false;
                isCrouching = false;

                if (joystick.Horizontal > 0)
                {
                    //Debug.Log("RIGHT");
                    transform.localScale = new Vector3(1, 1, 1);
                    bullet.transform.localScale = new Vector3(1, 1, 1);
                    runDust.transform.localScale = new Vector3(1, 1, 1);

                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    gun.transform.localPosition = weaponPosition_Staying;

                    isMoving = false;

                    if (joystick.Horizontal > 0.6f)
                    {
                        moveInput = 1;
                        isMoving = true;
                    }
                }
                else if (joystick.Horizontal < 0)
                {
                    //Debug.Log("LEFT");
                    transform.localScale = new Vector3(-1, 1, 1);
                    bullet.transform.localScale = new Vector3(-1, 1, 1);
                    runDust.transform.localScale = new Vector3(-1, 1, 1);

                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    gun.transform.localPosition = weaponPosition_Staying;

                    isMoving = false;

                    if (joystick.Horizontal < -0.6f)
                    {
                        moveInput = -1;
                        isMoving = true;
                    }
                }
                else
                {
                    animController.SetBool("isRunFire", false);

                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    gun.transform.localPosition = weaponPosition_Staying;

                    isMoving = false;
                    moveInput = 0;

                    if (isShooting && !isRecharching)
                    {
                        animController.SetBool("isStayFire", true);
                    }
                }
            }

            isGrounded = groundCheck.isGrounded;

            if (isMoving && isShooting && !isRecharching && !isCrouching && !isWatchingUp)
            {
                if (!isHagging)
                {
                    animController.SetBool("isStayFire", false);
                    animController.SetBool("isRunFire", true);
                }
                else
                {
                    animController.SetBool("isStayFire", true);
                }

                if (isHagging)
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                else
                    rb2D.velocity = new Vector2(moveInput * playerSpeed, rb2D.velocity.y);

                if (isGrounded && !isHagging)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        timer = timerReserve;

                        if (runDust.transform.localScale.x == 1)
                        {
                            runDust.transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y - 0.25f);
                        }
                        else
                        {
                            runDust.transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y - 0.25f);
                        }

                        Instantiate(runDust, runDust.transform.position, Quaternion.identity);
                    }
                }
            }
            else if (isMoving && !isCrouching && !isWatchingUp)
            {
                animController.SetBool("isMoving", true);

                rb2D.velocity = new Vector2(moveInput * playerSpeed, rb2D.velocity.y);

                if (isGrounded && !isHagging)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        timer = timerReserve;

                        if (runDust.transform.localScale.x == 1)
                        {
                            runDust.transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y - 0.25f);
                        }
                        else
                        {
                            runDust.transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y - 0.25f);
                        }

                        Instantiate(runDust, runDust.transform.position, Quaternion.identity);
                    }
                }
            }
            else
            {
                animController.SetBool("isMoving", false);
                if (isCrouching && !hagging_isWatchingDown)
                {
                    gun.transform.localPosition = weaponPosition_Crouching;
                }
            }

            if (!isShooting)
            {
                animController.SetBool("isRunFire", false);
            }
        }
    }

    public void OnTheRope(Transform position)
    {
        if (!isDead)
        {
            if (rb2D.velocity.y < 0 && Mathf.Abs(position.transform.position.y - transform.position.y) <= 0.2f &&
                Mathf.Abs(position.transform.position.y - transform.position.y) >= 0)//2.4 
            {
                transform.position = new Vector2(transform.position.x, position.transform.position.y - 1);
                isHagging = true;
                animController.SetBool("isHagging", true);

                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                gun.localPosition = weaponPosition_Hagging;

                rb2D.gravityScale = 0;
                jumpForce = jumpForceOnRope;
            }
        }
    }

    public void OutOfTheRope()
    {
        isHagging = false;

        animController.SetBool("isHagging", false);

        rb2D.gravityScale = 5;
        jumpForce = reserveJumpForce;

        gun.localPosition = weaponPosition_Staying;
    }

    public void PutPatrons(int amount)
    {
        amountOfAllPatrons += amount;
        textAmountOfBullets.text = $"{amountOfPatronsInMagazine}/{amountOfAllPatrons}";

        if (amountOfPatronsInMagazine == 0)
            StartCoroutine(Recharge());
    }

    public void OnJumpButtonDown()
    {
        if (!isHurting && !isDead)
        {
            if ((isGrounded || isHagging) && !hagging_isWatchingDown)
            {
                if(isCrouching && isOnPlatform)
                {
                    StartCoroutine(DisableCollider());
                    isOnPlatform = false;
                    return;
                }
                gun.transform.localPosition = weaponPosition_Staying;

                animController.SetBool("isCrouching", false);
                animController.SetBool("isWatchingUp", false);

                isWatchingUp = false;
                isCrouching = false;


                animController.SetBool("isJumping", true);
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);

                if (!isHagging)
                {
                    jumpDust.transform.position = new Vector2(transform.position.x, transform.position.y - 0.74f);
                    Instantiate(jumpDust, jumpDust.transform.position, Quaternion.identity);
                }
            }
            else if(isHagging && hagging_isWatchingDown)
            {
                isHagging = false;

                animController.SetBool("isHagging", false);

                rb2D.gravityScale = 5;
                jumpForce = reserveJumpForce;

                gun.localPosition = weaponPosition_Staying;
            }
        }
    }

    private IEnumerator DisableCollider()
    {
        test.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        test.GetComponent<Collider2D>().enabled = true;
    }

    public void OnRightButtonDown()
    {
        if (!isHurting && !isDead)
        {
            transform.localScale = new Vector3(1, 1, 1);
            bullet.transform.localScale = new Vector3(1, 1, 1);
            runDust.transform.localScale = new Vector3(1, 1, 1);

            moveInput = 1;

            isMoving = true;
        }
    }

    public void OnLeftButtonDown()
    {
        if (!isHurting && !isDead)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            bullet.transform.localScale = new Vector3(-1, 1, 1);
            runDust.transform.localScale = new Vector3(-1, 1, 1);

            moveInput = -1;

            isMoving = true;
        }
    }

    public void OnButtonUp()
    {
        if (!isHurting && !isDead)
        {
            isMoving = false;
            moveInput = 0;

            animController.SetBool("isRunFire", false);

            if (isShooting)
            {
                animController.SetBool("isStayFire", true);
            }

            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
    }

    public void OnFireButtonDown()
    {
        if (!isHurting && !isDead && !isRecharching)
        {
            isShooting = true;
            animController.SetBool("isStayFire", true);
        }
    }


    public void OnFireButtonUp()
    {
        isShooting = false;
        animController.SetBool("isStayFire", false);
    }

    public void CreateBullet()
    {
        if (isShooting && !isRecharching)
        {
            amountOfPatronsInMagazine--;
            textAmountOfBullets.text = $"{amountOfPatronsInMagazine}/{amountOfAllPatrons}";
            if (amountOfPatronsInMagazine <= 0)
            {
                StartCoroutine(Recharge());
            }
            Quaternion rotateBullet = Quaternion.Euler(0, 0, 90);
            position = new Vector2(gun.position.x, gun.position.y + Random.Range(-0.1f, 0.1f));
            if (isWatchingUp)
            {
                Instantiate(bullet, position, rotateBullet);
            }
            else
            {
                Instantiate(bullet, position, Quaternion.identity);
            }
        }
    }

    IEnumerator Recharge()
    {
        isRecharching = true;

        animController.SetBool("isStayFire", false);
        animController.SetBool("isRunFire", false);

        yield return new WaitForSeconds(timeForRecharge);

        if (amountOfAllPatrons <= 0)
        {
            isRecharching = true;
        }
        else if (amountOfAllPatrons <= reserveAmountPatrons)
        {
            amountOfPatronsInMagazine = amountOfAllPatrons;
            amountOfAllPatrons -= amountOfAllPatrons;
            isRecharching = false;
        }
        else
        {
            amountOfPatronsInMagazine = reserveAmountPatrons;
            amountOfAllPatrons -= reserveAmountPatrons;
            isRecharching = false;
        }

        textAmountOfBullets.text = $"{amountOfPatronsInMagazine}/{amountOfAllPatrons}";
        if (!isRecharching)
        {
            if (isShooting && isMoving)
            {
                animController.SetBool("isRunFire", true);
            }
            else if (isShooting)
            {
                animController.SetBool("isStayFire", true);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isHurting && !isDead)
        {
            playerHealth -= damage;

            StartCoroutine(Hurting());

            if (playerHealth <= 0 && !isDead)
            {
                gameObject.tag = "Enemy";
                isDead = true;
                StartCoroutine(Dead());
            }
        }
    }

    public void ToKillPlayer()
    {
        Instantiate(soul, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator Hurting()
    {
        isHurting = true;

        animController.SetTrigger("isHurting2");
        animController.SetBool("isHurting", true);

        OutOfTheRope();

        rb2D.velocity = new Vector2(0, rb2D.velocity.y);

        yield return new WaitForSeconds(0.5f);

        isHurting = false;
        animController.SetBool("isHurting", false);
    }

    private IEnumerator Dead()
    {
        animController.SetTrigger("isDead");

        yield return new WaitForSeconds(3f);

        Instantiate(soul, transform.position, Quaternion.identity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpDust.transform.position = new Vector2(transform.position.x, transform.position.y - 0.74f);
            Instantiate(jumpDust, jumpDust.transform.position, Quaternion.identity);
        }
        else if (collision.gameObject.CompareTag("Platform"))
        {
            test = collision.gameObject;
            isOnPlatform = true;
        }
    }
}
