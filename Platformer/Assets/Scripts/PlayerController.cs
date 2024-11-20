using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 playerMovementInput;
    public Rigidbody2D rb;
    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        
        if (Input.GetKey(KeyCode.A))
        {
            playerInput = Vector3.left * 2;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerInput = Vector3.right * 2;
        }
        else
        {
            playerInput = Vector3.zero;
        }
        playerMovementInput = playerInput;
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        transform.position += new Vector3(playerInput.x, playerInput.y) * Time.deltaTime;

        Debug.Log(IsGrounded());
    }

    public bool IsWalking()
    {
        if (playerMovementInput.x > 0 || playerMovementInput.x < 0){
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector3.down, 0.1f);
    }

    public FacingDirection GetFacingDirection()
    {
        if (playerMovementInput.x > 0)
        {
            return FacingDirection.right;
        }
        if (playerMovementInput.x < 0)
        {
            return FacingDirection.left;
        }
        else
        {
            return FacingDirection.right;
        }
        
        
    }
}
