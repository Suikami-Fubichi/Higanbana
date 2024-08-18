using UnityEngine;
using Random = UnityEngine.Random;

public class JerriController : MonoBehaviour
{
    // Components
    private Animator anim;  // Animator component for handling animations
    private Rigidbody2D rb; // Rigidbody2D component for physics-based movement

    // Transform references for various positions
    [SerializeField]
    private Transform projPos, rainPos, groundCheck;

    // Layer masks to define what is considered the player and the ground
    [SerializeField]
    private LayerMask whatIsPlayer, whatIsGround;

    // Projectile and particle effect prefabs
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject player, hitParticle;

    // Variables for controlling character's behavior
    private Vector2 botLeft, topRight;
    private bool isAttacking;       // Tracks if the character is currently attacking
    private bool isAwake;           // Tracks if the character is "awake" or active
    private bool isFacingRight;     // Tracks the character's facing direction
    private bool onFloor;           // Tracks if the character is on the ground

    // Character stats
    [SerializeField]
    private float maxHP, jumpForce, dashSpeed, walkSpeed, groundCheckRadius;
    private float currentHP;        // Current health of the character

    // Direction the character is facing
    private float facingDirection = -1;

    // Target (player) reference
    private Transform target;

    // Initialize components and variables
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");  // Find the player object in the scene
        target = player.transform;           // Get the player's transform
        currentHP = maxHP;                   // Set the current HP to max HP
    }

    // Manual Input Testing Attacks
    /*private void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("3hitCombo");  // Play 3-hit combo animation
        }
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("BackupShot"); // Play backup shot animation
        }
        if (Input.GetKeyUp(KeyCode.C) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("ArrowRain");  // Play arrow rain animation
        }
        if (Input.GetKeyUp(KeyCode.Z) && !isAttacking)
        {
            isAttacking = true;
            anim.Play("JumpShot");   // Play jump shot animation
        }
    }*/

    // Trigger melee attack
    public void TriggerMelee()
    {
        isAttacking = true;
        anim.Play("3hitCombo");
    }

    // Trigger backup shot
    public void TriggerBackup()
    {
        isAttacking = true;
        anim.Play("BackupShot");
    }

    // Trigger arrow rain attack
    public void TriggerRain()
    {
        isAttacking = true;
        anim.Play("ArrowRain");
    }

    // Trigger jump shot attack
    public void TriggerJumpShot()
    {
        isAttacking = true;
        anim.Play("JumpShot");
    }

    // Get the current attacking status
    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    // Get the current facing direction
    public float GetFacingDirection()
    {
        return facingDirection;
    }

    // Spawn a projectile from the specified position
    private void spawnProjectile()
    {
        float rotation = 0;
        if (isFacingRight)
        {
            rotation = 180;  // Adjust rotation if facing right
        }
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, rotation));
    }

    // Spawn a rain of arrows with different angles
    private void RainArrow()
    {
        float rotation = 0;
        if (isFacingRight)
        {
            rotation = 135; // Adjust rotation if facing right
        }
        Instantiate(projectile, rainPos.position, Quaternion.Euler(0.0f, 0.0f, 10 + rotation));
        Instantiate(projectile, rainPos.position, Quaternion.Euler(0.0f, 0.0f, 25 + rotation));
        Instantiate(projectile, rainPos.position, Quaternion.Euler(0.0f, 0.0f, 40 + rotation));
    }

    // Spawn multiple projectiles during a jump shot
    private void JumpShot()
    {
        float rotation = 0;
        if (isFacingRight)
        {
            rotation = 135; // Adjust rotation if facing right
        }
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, 10 + rotation));
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, 25 + rotation));
        Instantiate(projectile, projPos.position, Quaternion.Euler(0.0f, 0.0f, 40 + rotation));
    }

    // Chase the player if awake
    private void Chase()
    {
        if (isAwake)
        {
            target.position = player.transform.position;
            if (!isAttacking)
            {
                // Determine which direction to face based on player's position
                if (target.position.x > gameObject.transform.position.x)
                {
                    isFacingRight = true;
                    if (facingDirection < 0)
                    {
                        Flip();
                    }
                }
                else if (target.position.x < gameObject.transform.position.x)
                {
                    isFacingRight = false;
                    if (facingDirection > 0)
                    {
                        Flip();
                    }
                }
                // Move towards the player
                rb.velocity = new Vector2(walkSpeed * facingDirection, rb.velocity.y);
            }
        }
    }

    // Make the character jump
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Make the character dash forward
    private void Dash()
    {
        rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
    }

    // Make the character dash backward
    private void BackDash()
    {
        rb.velocity = new Vector2(dashSpeed * 2 * facingDirection * -1, rb.velocity.y);
    }

    // Flip the character's facing direction
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    // Wake the character when collided with the player
    public void Wake(Collider2D collision)
    {
        isAwake = true;
    }

    // Finish the current attack
    public void FinishAttack()
    {
        isAttacking = false;
    }

    // Update the animator parameters
    private void UpdateAnim()
    {
        anim.SetBool("IsAttacking", isAttacking);
        anim.SetFloat("yVector", rb.velocity.y);
        anim.SetBool("Grounded", onFloor);
    }

    // Handle taking damage
    private void Damage(float[] attackDetails)
    {
        currentHP -= attackDetails[0];
        Instantiate(hitParticle, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        if (currentHP < 0)
        {
            Die();
        }
    }

    // Handle the character's death
    private void Die()
    {
        Destroy(gameObject);
    }

    // Check if the character is on the ground
    private void CheckFloor()
    {
        onFloor = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnim();  // Update animation states
        Chase();       // Handle chasing behavior
        CheckFloor();  // Check if the character is grounded
    }

    // Draw ground check gizmo in the editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
