using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D playerRb;
    public bool detected = false;

    void Update()
    {
       if (detected == true)
        {
            playerRb.AddRelativeForce(transform.position - player.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        detected = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detected = false;
    }

   
}
