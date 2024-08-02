using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class JerriController : MonoBehaviour
{
    private Animator anim;

    private Rigidbody2D rb;

    [SerializeField]
    private Transform projPos, rainPos, groundCheck;


    [SerializeField]
    private LayerMask whatIsPlayer, whatIsGround;

    [SerializeField]
    private GameObject projectile;

    private Vector2 botLeft, topRight;

    private bool isAttacking;
    private bool isAwake;
    private bool isFacingRight;
    private bool onFloor;

    [SerializeField]
    private float maxHP, jumpForce, dashSpeed, walkSpeed, groundCheckRadius;
    private float currentHP;

    private float facingDirection = -1;

    [SerializeField]
    private GameObject player;

    private Transform target;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        target = player.transform;
        currentHP = maxHP;
    }

    private void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("3hitCombo");
        }
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("BackupShot");
        }
        if(Input.GetKeyUp(KeyCode.C) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("ArrowRain");
        }
        if (Input.GetKeyUp(KeyCode.Z) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("JumpShot");
        }
    }

    public float GetFacingDirection()
    {
        return facingDirection;
    }

    private void spawnProjectile()
    {
        float rotation = 0;
        if(isFacingRight)
        {
            rotation = 180;
        }
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, rotation));
    }

    private void RainArrow()
    {
        float rotation = 0;
        if (isFacingRight)
        {
            rotation = 135;
        }
        Instantiate(projectile, rainPos.position, Quaternion.Euler(0.0f, 0.0f, 10 + rotation));
        Instantiate(projectile, rainPos.position, Quaternion.Euler(0.0f, 0.0f, 25 + rotation));
        Instantiate(projectile, rainPos.position, Quaternion.Euler(0.0f, 0.0f, 40 + rotation));
    }

    private void JumpShot()
    {
        float rotation = 0;
        if (isFacingRight)
        {
            rotation = 135;
        }
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, 10 + rotation));
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, 25 + rotation));
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, 40 + rotation));
    }

    private void Chase()
    {
        if (isAwake)
        {
            target.position = player.transform.position;
            if (!isAttacking) {
                //player = GameObject.Find("Player");
                if (target.position.x > gameObject.transform.position.x)
                {
                    isFacingRight = true;
                    if(facingDirection < 0)
                    {
                        Flip();
                    }
                }
                else if (target.position.x < gameObject.transform.position.x)
                {
                    isFacingRight = false;
                    if(facingDirection > 0)
                    {
                        Flip();
                    }
                }
            rb.velocity = new Vector2(walkSpeed * facingDirection, rb.velocity.y);
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void Dash()
    {
        rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void Wake(Collider2D collision)
    {
        isAwake = true;
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }

    private void UpdateAnim()
    {
        anim.SetBool("IsAttacking", isAttacking);
        anim.SetFloat("yVector", rb.velocity.y);
        anim.SetBool("Grounded", onFloor);
    }

    private void Damage(float[] attackDetails)
    {
        currentHP -= attackDetails[0];
        if(currentHP < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void CheckFloor()
    {
        onFloor = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    
    void Update()
    {
        CheckAttack();
        UpdateAnim();
        Chase();
        CheckFloor();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
