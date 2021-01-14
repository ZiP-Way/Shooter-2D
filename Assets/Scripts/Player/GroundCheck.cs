using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public bool isGrounded;

    private Animator animController;

    private void Start()
    {
        animController = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Layer 10 is ground
        {
            isGrounded = true;
            animController.SetBool("isJumping", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Layer 10 is ground
        {
            isGrounded = false;
        }
    }
}
