using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Animator animator;
    private Rigidbody2D rb;
    bool isJump = false;
    float face = 1;
    public float force = 5f;

    public int sumCounts = 100;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        if (x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        else if(x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if(x >= 0.05 ||x < -0.05)
        {
            animator.SetBool("Run", true);
            transform.position = new Vector3(transform.position.x + Time.deltaTime * speed * x, transform.position.y, transform.position.z);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        var info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Base Layer.Jump"))
        {
            isJump = true;
            animator.SetBool("Jump", false);
        }
        else
        {
            isJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            animator.SetBool("Jump", true);
            rb.AddForce(new Vector2(0, force));
        }
    }
    
}
