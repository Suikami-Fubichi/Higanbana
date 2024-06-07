using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput, isAttacking;

    private int attackString;

    private float lastInputTime = Mathf.NegativeInfinity;
    private float attackDamage;
    //variable attackdmg^

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
        CheckStringCombo();
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                //Attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            //Perform Attack1
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                anim.SetBool("attack1", true);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new input
            gotInput = false;
        }
    }

    private void CheckStringCombo()
    {
        if(Time.time >= lastInputTime + 0.7f)
        {
            attackString = 0;
            
        }
        anim.SetInteger("attackString", attackString);
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attack1Damage);
            //Instantiate hit particle
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
        anim.SetInteger("attackString", attackString);
        switch (attackString)
        {
            case 0:
                attackDamage = 10;
                break;
            case 1:
                attackDamage = 20;
                break;
            case 2: 
                attackDamage = 30;
                break;
            default:
                attackDamage = 10;
                break;
        }
        attackString++;
        if (attackString >= 3)
        {
            attackString = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

}
