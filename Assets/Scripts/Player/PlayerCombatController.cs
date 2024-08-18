using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    // Configuration variables
    [SerializeField]
    private bool combatEnabled;            // Is combat enabled
    [SerializeField]
    private float inputTimer, attack1Radius; // Time window for input and radius for attack detection
    [SerializeField]
    private Transform attack1HitBoxPos;    // Position of the hitbox for attack 1
    [SerializeField]
    private LayerMask whatIsDamageable;    // Layer mask for objects that can be damaged

    // Combat state variables
    private bool gotInput, isAttacking;
    private int attackString;              // Tracks the current attack combo
    private float lastInputTime = Mathf.NegativeInfinity;
    private float attackDamage;            // Damage value of the current attack
    private float[] attackDetails = new float[2]; // Array to store attack details (damage, position)

    // Ability variables
    private float maxHoldTime = 1.0f;      // Maximum time to hold input for abilities
    private float holdTimeScale = 0.25f;   // Time scale when holding input for an ability
    private float holdTimeStart = -100;    // Start time for holding input
    private float holdTimeLeft;            // Time left to hold input for an ability
    private float abilityTime = 2.0f;      // Duration of the ability
    private float abilityStartTime;        // Start time for the ability
    private float abilityTimeLeft;         // Time left for the ability duration
    private float lastAbility = -100;      // Last time an ability was used
    private string comboString = "";       // String to track the current combo input

    private bool isAbility;                // Is the player using an ability
    private bool isHolding;                // Is the player holding input for an ability
    private Vector2 abilityDirection;      // Direction of the ability

    // Components
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController PC;
    private PlayerStats PS;

    private void Start()
    {
        // Initialize components and set initial states
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled); // Enable or disable combat based on initial setting
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check for combat input, attack, and ability states
        CheckCombatInput();
        CheckAttacks();
        CheckStringCombo();
        CheckAbility();
        CheckCombo();
    }

    private void CheckCombatInput()
    {
        // Check for left mouse button input for attack
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time; // Register the input time
            }
        }

        // Check for right mouse button input to enter combo mode
        if (Input.GetMouseButtonDown(1))
        {
            if (combatEnabled)
            {
                EnterComboMode(); // Start combo mode
            }
        }
        // Check for releasing right mouse button to exit combo mode
        else if (Input.GetMouseButtonUp(1))
        {
            ExitComboMode();
        }
    }

    private void CheckAttacks()
    {
        // Check if the player received attack input
        if (gotInput)
        {
            PC.SlowMovement(); // Slow down the player's movement during attack

            if (!isAttacking)
            {
                // Determine if player is dashing and play appropriate animation
                if (PC.GetDashStatus())
                {
                    anim.Play("ability1"); // Play ability animation during dash
                }
                else
                {
                    // Set attack damage based on the current attack combo string
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
                    anim.SetBool("attack1", true); // Trigger attack animation
                    anim.SetBool("isAttacking", isAttacking);
                }
            }
        }

        // Reset input if too much time has passed since last input
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckStringCombo()
    {
        // Reset attack combo string if too much time has passed since last input
        if (Time.time >= lastInputTime + 0.7f)
        {
            attackString = 0;
        }
        anim.SetInteger("attackString", attackString); // Update animator with current attack string
    }

    private void CheckAttackHitBox()
    {
        // Detect objects in the attack hitbox and apply damage
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails[0] = attackDamage; // Set damage value
        attackDetails[1] = transform.position.x; // Set position for knockback direction

        foreach (Collider2D collider in detectedObjects)
        {
            if (collider.transform.parent != null)
            {
                collider.transform.parent.SendMessage("Damage", attackDetails); // Send damage message to parent
            }
            else
            {
                collider.transform.SendMessage("Damage", attackDetails); // Send damage message directly
            }
            // Instantiate hit particle (if needed)
        }
    }

    private void FinishAttack1()
    {
        // Reset attack state and return to normal movement
        isAttacking = false;
        PC.NormalMovement();

        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
        anim.SetInteger("attackString", attackString);
        attackString++;
        if (attackString >= 3)
        {
            attackString = 0; // Reset attack string after a full combo
        }
    }

    private void Damage(float[] attackDetails)
    {
        // Apply damage to the player
        if (!PC.GetDashStatus())
        {
            int direction;

            PS.DecreaseHealth(attackDetails[0]); // Decrease player health

            // Determine knockback direction
            if (attackDetails[1] < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            PC.Knockback(direction); // Apply knockback to player
        }
    }

    private void EnterComboMode()
    {
        // Enter combo mode and slow down time
        isHolding = true;
        Time.timeScale = holdTimeScale;
        holdTimeStart = Time.unscaledTime;
    }

    private void CheckCombo()
    {
        if (isHolding)
        {
            comboString += ReadCombo(); // Read combo input
            if (comboString.Length == 3)
            {
                Debug.Log(comboString); // Output combo string for debugging
            }
        }

        // Check for specific combo strings to trigger abilities
        if (comboString.Equals("ASD") || comboString.Equals("DSA"))
        {
            AttemptToAbility(); // Trigger ability
            comboString = "";
            ExitComboMode();
        }
        else if (comboString.Equals("AAD") || comboString.Equals("DDA"))
        {
            anim.Play("Starburst Steven"); // Play special ability animation
            isAttacking = true;
            comboString = "";
            ExitComboMode();
        }

        // Reset combo string if it becomes too long or time runs out
        if (comboString.Length > 3 || Time.unscaledTime >= holdTimeStart + maxHoldTime)
        {
            comboString = "";
            ExitComboMode();
        }
    }

    private string ReadCombo()
    {
        // Read and return combo input as a string
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
        // Exit combo mode and reset time scale
        isHolding = false;
        Time.timeScale = 1f;
    }

    private void AttemptToAbility()
    {
        // Trigger an ability
        isAbility = true;
        abilityTimeLeft = abilityTime;
        lastAbility = Time.time;
    }

    private void CheckAbility()
    {
        // Check and handle ability duration
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
        // Finish ability and reset state
        isAbility = false;
        anim.SetBool("isAbility", isAbility);
    }

    private void ChangeDamage(float dmg)
    {
        // Change the attack damage value
        attackDamage = dmg;
    }

    private void OnDrawGizmos()
    {
        // Draw the hitbox gizmo in the editor
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
