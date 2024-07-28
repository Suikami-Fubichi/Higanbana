using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerriController : MonoBehaviour
{
    private Animator anim;

    private Rigidbody2D rb;

    [SerializeField]
    private Transform topleft, topright, botleft, botright;

    [SerializeField]
    private LayerMask whatIsPlayer;

    private Vector2 botLeft, topRight;

    private bool isAttacking;

    [SerializeField]
    private float maxHP;
    private float currentHP;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        botLeft.Set(botleft.position.x, botleft.position.y);
        topRight.Set(topright.position.x, topright.position.y);
    }

    private void CheckAttack()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isAttacking = true;
            anim.Play("3hitCombo");
        }
    }

    private void CheckInRange()
    {
        Collider2D hit = Physics2D.OverlapArea(botLeft, topRight, whatIsPlayer);
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }

    private void UpdateAnim()
    {
        anim.SetBool("IsAttacking", isAttacking);
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

    
    void Update()
    {
        CheckAttack();
        UpdateAnim();
    }

    private void OnDrawGizmos()
    {
        Vector2 botLeft = new Vector2(botleft.position.x, botleft.position.y);
        Vector2 botRight = new Vector2(botright.position.x, botright.position.y);
        Vector2 topRight = new Vector2(topright.position.x, topright.position.y);
        Vector2 topLeft = new Vector2(topleft.position.x, topleft.position.y);

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
