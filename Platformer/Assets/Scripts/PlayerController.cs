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

    public float apexHeight;
    public float apexTime = 0.1f;
    public float terminalSpeed = 2f;
    public float coyoteTime = 0.25f;

    
    public float timeToReachMaxSpeed = 1;
    public float maxSpeed = 3.5f;

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (playerInput.x == 0)
            {
                if (currentFacingDirection == FacingDirection.left)
                {
                    playerInput = Vector3.left * moveSpeed * dashSpeed;
                }
                else
                {
                    playerInput = Vector3.right * moveSpeed * dashSpeed;
                }


            }
            playerInput = playerInput * dashSpeed;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || coyoteTime > 0))
        {
            isJumping = true;
            rb.gravityScale = 0;
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
        }
        else
        {
            if (apexTime > 0)
            {
                apexTime -= 1 * Time.deltaTime;
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = 1;
            }
            
            isJumping = false;
            currentJumpTime = 0;
        }


        if (rb.velocity.y < -terminalSpeed)
        {
            rb.velocity = new Vector2(velocity.x, -terminalSpeed);
        }

        velocity = playerInput;

        rb.velocity += new Vector2(velocity.x, velocity.y) * acceleration * Time.deltaTime;

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }

        Debug.Log(rb.velocity.x);
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
