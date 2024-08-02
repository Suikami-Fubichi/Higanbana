using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput, isAttacking;

    private int attackString;

    private float lastInputTime = Mathf.NegativeInfinity;
    private float attackDamage;
    private float[] attackDetails = new float[2];
    //variable attackdmg^

    //Ability Vars
    //private float abilityCooldown = 1.0f;
    private float maxHoldTime = 1.0f;
    private float holdTimeScale = 0.25f;
    private float holdTimeStart = -100;
    private float holdTimeLeft;
    private float abilityTime = 2.0f;
    private float abilityStartTime;
    private float abilityTimeLeft;
    //private float abilityVelocity = 30.0f;
    private float lastAbility = -100;
    private string comboString = "";

    private bool isAbility;
    private bool isHolding;
    private Vector2 abilityDirection;

    private Animator anim;

    private Rigidbody2D rb;

    private PlayerController PC;
    private PlayerStats PS;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
        CheckStringCombo();
        CheckAbility();
        CheckCombo();
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
        if (Input.GetMouseButtonDown(1))
        {
            if (combatEnabled)
            {
                //if (Time.time >= (lastAbility + abilityCooldown))
                //{
                    EnterComboMode();
                    //AttemptToAbility();
                //}
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ExitComboMode();
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            //Perform Attack1
            if (!isAttacking)
            {
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

        attackDetails[0] = attackDamage;
        attackDetails[1] = transform.position.x;

        foreach (Collider2D collider in detectedObjects)
        {
            if (collider.transform.parent != null) { 
                collider.transform.parent.SendMessage("Damage", attackDetails);
            }
            else
            {
                collider.transform.SendMessage("Damage", attackDetails);
            }
            //Instantiate hit particle
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
        anim.SetInteger("attackString", attackString);
        attackString++;
        if (attackString >= 3)
        {
            attackString = 0;
        }
    }

    private void Damage(float[] attackDetails)
    {
        if (!PC.GetDashStatus())
        {
            int direction;

            PS.DecreaseHealth(attackDetails[0]);

            if (attackDetails[1] < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            PC.Knockback(direction);
        }
    }

    private void EnterComboMode()
    {
        isHolding = true;
        Time.timeScale = holdTimeScale;
        holdTimeStart = Time.unscaledTime;
    }

    private void CheckCombo()
    {
        if (isHolding)
        {
            comboString += ReadCombo();
            if(comboString.Length == 3)
            {
                Debug.Log(comboString);
            }
        }
        if(comboString.Equals("ASD") || comboString.Equals("DSA"))
        {
            AttemptToAbility();
            comboString = "";
            ExitComboMode();
        }
        if (comboString.Length > 3 || Time.unscaledTime >= holdTimeStart + maxHoldTime)
        {
            comboString = "";
            ExitComboMode();
        }
    }

    private string ReadCombo()
    {
        string temp = "";
        if (Input.GetKeyDown(KeyCode.W))
        {
            temp += "W";
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            temp += "S";
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            temp += "D";
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            temp += "A";
        }
        return temp;
    }

    private void ExitComboMode()
    {
        isHolding = false;
        Time.timeScale = 1f;
    }

    private void AttemptToAbility()
    {
        isAbility = true;
        abilityTimeLeft = abilityTime;
        lastAbility = Time.time;
    }

    private void CheckAbility()
    {
        if (isAbility)
        {
            if (abilityTimeLeft > 0)
            {
                anim.SetBool("isAbility", isAbility);
                abilityTimeLeft -= Time.deltaTime;

            }
        }
    }

    private void FinishAbility1()
    {
        isAbility = false;

        anim.SetBool("isAbility", isAbility);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

}
