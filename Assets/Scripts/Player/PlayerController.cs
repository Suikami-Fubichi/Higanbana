using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private variables for movement and action tracking
    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100;
    private float knockbackStartTime;

    [SerializeField]
    private float knockbackDuration;

    // Variables to track player jumps and direction
    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    // Booleans to manage player state
    private bool isFacingRight = true;
    private bool onFloor;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    public bool canMove;
    private bool canFlip;
    private bool isDashing;
    private bool knockback;

    // References to the Rigidbody2D and Animator components
    private Rigidbody2D rb;
    private Animator anim;

    // Public variables for movement settings
    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float dashTime = 0.2f;
    public float dashSpeed = 25.0f;
    public float distanceBetweenImages = 0.1f;
    public float dashCooldown = 2.5f;

    [SerializeField]
    private Vector2 knockbackSpeed;

    // Directions for wall hopping and wall jumping
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    // References to ground and wall checks
    public Transform groundCheck;
    public Transform wallCheck;

    // Layer mask for what is considered ground
    public LayerMask whatIsGround;

    // Initialize variables and components
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is used to run through each constant check
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckDash();
        CheckKnockback();
    }

    // Set the values of touching floor and wall to their respective variables
    private void CheckSurroundings()
    {
        onFloor = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    // Check if the player can jump
    private void CheckIfCanJump()
    {
        if (onFloor && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    // Check if the player is wall sliding
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    // Return the current dashing status
    public bool GetDashStatus()
    {
        return isDashing;
    }

    // Apply knockback to the player in the given direction
    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    // Check if knockback should end
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    // FixedUpdate is used to apply physics-based movement
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    // Flip the player's direction based on input
    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
    }

    // Slow down the player's movement speed
    public void SlowMovement()
    {
        movementSpeed = 3.5f;
    }

    // Reset the player's movement speed to normal
    public void NormalMovement()
    {
        movementSpeed = 10.0f;
    }

    // Disable the player's ability to flip direction
    public void DisableFlip()
    {
        canFlip = false;
    }

    // Enable the player's ability to flip direction
    public void EnableFlip()
    {
        canFlip = true;
    }

    // Disable the player's ability to move
    public void DisableMove()
    {
        canMove = false;
    }

    // Enable the player's ability to move
    public void EnableMove()
    {
        canMove = true;
    }

    // Flip the player to face the opposite direction
    private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    // Update the player's animations based on movement
    private void UpdateAnimations()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("onFloor", onFloor);
    }

    // Check for player input and handle actions accordingly
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")/* || Input.GetKeyDown(KeyCode.W)*/)
        {
            if (onFloor || (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!onFloor && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;
                turnTimer = turnTimerSet;
            }
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump") /*|| !Input.GetKey(KeyCode.W) && checkJumpMultiplier*/)
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= (lastDash + dashCooldown))
                AttemptToDash();
        }
    }

    // Attempt to perform a dash action
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    // Return the direction the player is facing
    public int GetFacingDirection()
    {
        return facingDirection;
    }

    // Check if the player should continue dashing
    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }

    // Check if the player should jump based on input and conditions
    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!onFloor && isTouchingWall && movementInputDirection != 0 && movementInputDirection != -facingDirection)
            {
                WallJump();
            }
            else if (onFloor)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    // Perform a normal jump action
    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    // Perform a wall jump action
    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
        }
    }

    // Apply movement based on player input and state
    private void ApplyMovement()
    {
        if (!onFloor && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    // Draw gizmos for debugging ground and wall checks
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}