using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 velocity;
    public Rigidbody2D rb;

    public bool isJumping;
    public float maxJumpTime;
    public float jumpSpeed;
    public float currentJumpTime;

    public float moveSpeed = 3;
    public float dashSpeed = 2;
    public float dashTimer = 0f;
    public bool isDashing = false;

    public float apexHeight;
    public float apexTime = 0.1f;
    public float terminalSpeed = 2f;
    public float coyoteTime = 0.25f;

    public float rocketBootsFuel = 1f;
    public float rocketBootsSpeed = 3.5f;
    public bool rocketBootsFlying = false;

    public SpriteRenderer rocketBoots;
    
    
    public float timeToReachMaxSpeed = 1;
    public float maxSpeed = 0.75f;

    private float acceleration;

    public int health = 10;

    public enum FacingDirection
    {
        left, right
    }
    public FacingDirection currentFacingDirection = FacingDirection.right;

    public enum CharacterState
    {
        idle, walk, jump, die
    }
    public CharacterState currentCharacterState = CharacterState.idle;
    public CharacterState previousCharacterState = CharacterState.idle;

    // Start is called before the first frame update
    void Start()
    {
        acceleration = maxSpeed / timeToReachMaxSpeed;
        rocketBoots.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (previousCharacterState != currentCharacterState)
        {

        }
        previousCharacterState = currentCharacterState;

        currentFacingDirection = GetFacingDirection();
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        
        if (Input.GetKey(KeyCode.A))
        {
            playerInput = Vector3.left * moveSpeed * acceleration;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerInput = Vector3.right * moveSpeed * acceleration;
        }
        else
        {
            playerInput = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.LeftShift) && dashTimer >= 1)
        {
            isDashing = true;
            
            maxSpeed = 5;
            dashTimer = 0;
        }

        




        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || coyoteTime > 0))
        {
            isJumping = true;
            rb.gravityScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && rocketBootsFuel > 0.8)
        {
            rocketBootsFlying = true;
        }

        if (!IsGrounded())
        {
            coyoteTime -= Time.deltaTime;
        }
        else
        {
            coyoteTime = 0.25f;
        }

        switch (currentCharacterState)
        {
            case CharacterState.die:
                // Do Nothing (:
                break;
            case CharacterState.jump:
                if (IsGrounded())
                {
                    if (IsWalking()) {
                        currentCharacterState = CharacterState.walk;
                    }
                    else
                    {
                        currentCharacterState = CharacterState.idle;
                    }
                }
                break;
            case CharacterState.walk:
                if (!IsWalking()) {
                    currentCharacterState = CharacterState.idle;
                }
                if (!IsGrounded()) {
                    currentCharacterState = CharacterState.jump;
                }
                break;
            case CharacterState.idle:
                //Are we walking?
                if (IsWalking()) {
                    currentCharacterState = CharacterState.walk;
                }
                //Are we jumping?
                if (!IsGrounded()) {
                    currentCharacterState = CharacterState.jump;
                }
                break;
        }
        if (IsDead())
        {
            currentCharacterState = CharacterState.die;
        }

        //velocity = playerInput;
        MovementUpdate(playerInput);
        
    }

    private void MovementUpdate(Vector2 playerInput)
    {   

        // jump code
        if (isJumping == true && maxJumpTime > currentJumpTime)
        {
            rb.gravityScale = 0;
            playerInput.y += jumpSpeed + acceleration * Time.deltaTime;
            apexTime = 0.1f;
            currentJumpTime += 1f * Time.deltaTime;
            if (currentJumpTime >= maxJumpTime)
            {
                isJumping = false;
                currentJumpTime = 0f;
            }
        }
        else if (rocketBootsFlying)
        {
            rocketBoots.enabled = true;
            rb.gravityScale = 0;
            playerInput.y += rocketBootsSpeed + acceleration/2 * Time.deltaTime;
            rocketBootsFuel -= 1 * Time.deltaTime;
            if (rocketBootsFuel <= 0)
            {
                rocketBoots.enabled = false;
                rocketBootsFlying = false;
            }
        }
        else
        {
            
            if (apexTime > 0 && !isJumping)
            {
                apexTime -= 1 * Time.deltaTime;
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = 1;
            }
            //rocket fuel recharges when falling
            rocketBootsFuel = Mathf.Clamp(rocketBootsFuel + 1*Time.deltaTime, 0, 1);
        }

        // dashing code

        if (isDashing == true && dashTimer < 0.25f)
        {
            if (currentFacingDirection == FacingDirection.left)
            {
                rb.AddForce(Vector3.left * dashSpeed * acceleration);
            }
            else
            {
                rb.AddForce(Vector3.right * dashSpeed * acceleration);
            }   
        }
        else 
        {
            maxSpeed = 3.5f;
        }
        dashTimer += 1 * Time.deltaTime;
        dashTimer = Mathf.Clamp(dashTimer, 0, 1);
        // gravity terminal speed

        if (rb.velocity.y < -terminalSpeed)
        {
            rb.velocity = new Vector2(velocity.x, -terminalSpeed);
        }
        if (rb.velocity.y > terminalSpeed)
        {
            rb.velocity = new Vector2(velocity.x, terminalSpeed);
        }

        velocity = playerInput;

        rb.velocity += new Vector2(velocity.x, velocity.y) * acceleration * Time.deltaTime;

        // velocity.x maxSpeed code

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }
public bool IsWalking()
    {
        if (velocity.x > 0 || velocity.x < 0){
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsGrounded()
    {
        Vector2 RaycastPositionLeft = transform.position + new Vector3(-0.5f, 0);
        Vector2 RaycastPositionRight = transform.position + new Vector3(0.5f, 0);

        Debug.DrawRay(RaycastPositionLeft, Vector3.down, Color.red);

        Debug.DrawRay(RaycastPositionRight, Vector3.down, Color.red);

        return Physics2D.Raycast(RaycastPositionLeft, Vector3.down, 0.7f) || Physics2D.Raycast(RaycastPositionRight, Vector3.down, 0.7f);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void onDeathAnimationComplete()
    {
        gameObject.SetActive(false);
    }

    public FacingDirection GetFacingDirection()
    {
        if (velocity.x > 0)
        {
            return FacingDirection.right;
        }
        if (velocity.x < 0)
        {
            return FacingDirection.left;
        }
        return currentFacingDirection;
        
    }
}
