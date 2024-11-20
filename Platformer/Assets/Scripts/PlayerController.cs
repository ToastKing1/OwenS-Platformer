using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 velocity;
    public Rigidbody2D rb;

    public bool isJumping;
    public float maxJumpTime;

    public float gravity = 2;

    public float currentJumpTime;

    /*
    public float timeToReachMaxSpeed;
    public float maxSpeed;

    private float acceleration;
    */
    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        //acceleration = maxSpeed / timeToReachMaxSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        
        if (Input.GetKey(KeyCode.A))
        {
            playerInput = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerInput = Vector3.right;
        }
        else
        {
            playerInput = Vector3.zero;
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            isJumping = true;
        }

        
            if (!IsGrounded() && !isJumping)
            {
                playerInput.y -= gravity;
            }

        
        
        velocity = playerInput;
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        
        if (isJumping == true && maxJumpTime > currentJumpTime)
        {
            playerInput.y += 2;
            currentJumpTime += 0.5f * Time.deltaTime;
        }
        else
        {
            isJumping = false;
            currentJumpTime = 0;
        }

        Debug.Log(playerInput.y);

        transform.position += new Vector3(playerInput.x*2, playerInput.y) * Time.deltaTime;


        Debug.Log(IsGrounded());
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
