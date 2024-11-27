using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 velocity;
    public Rigidbody2D rb;

    public bool isJumping;
    public float maxJumpTime;

    public float currentJumpTime;

    public float apexHeight;

    public float apexTime = 0.1f;

    public float terminalSpeed = 2f;

    public float coyoteTime = 0.25f;

    
    public float timeToReachMaxSpeed = 1;
    public float maxSpeed = 5;

    private float acceleration;
    

    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        acceleration = maxSpeed / timeToReachMaxSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        
        if (Input.GetKey(KeyCode.A))
        {
            playerInput = Vector3.left * 3;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerInput = Vector3.right * 3;
        }
        else
        {
            playerInput = Vector3.zero;
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

        //velocity = playerInput;
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {   
        if (isJumping == true && maxJumpTime > currentJumpTime)
        {
            rb.gravityScale = 0;
            playerInput.y += 1 + acceleration;
            apexTime = 0.1f;
            currentJumpTime += 0.5f * Time.deltaTime;
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

        transform.position += new Vector3(playerInput.x, playerInput.y) * Time.deltaTime;
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
       


        Debug.DrawRay(transform.position, Vector3.down, Color.red);

        

        return Physics2D.Raycast(transform.position, Vector3.down, 0.7f);


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
        else
        {
            return FacingDirection.right;
        }
        
        
    }
}
